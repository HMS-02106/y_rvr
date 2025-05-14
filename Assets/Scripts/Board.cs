using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtility.Linq;
using UnityUtility.Collections;
using UnityUtility.Enums;
using R3;
using MoreLinq.Extensions;
using System.Threading.Tasks;

public class Board : MonoBehaviour, IObservableScore, IObservableCurrentTurnColor
{
    [SerializeField]
    private Square originalSquare;

    private Matrix<Square> squareMatrix;
    private ScoreManager scoreManager;
    private ReactiveProperty<StoneColor> currentStoneColor = new(StoneProvider.initialStoneColor);

    private TaskCompletionSource<StoneColor> squareGenerateCompletedTask = new();

    public IReadOnlyMatrix<Square> Squares => squareMatrix;

    /// <summary>
    /// ターンが始まった時、またはターンが変わったときに、現在のターンの色を取得が発行される
    /// </summary>
    /// <returns></returns>
    public Observable<StoneColor> CurrentStoneColor =>
        Observable
            .FromAsync(async ct => await squareGenerateCompletedTask.Task) // マス目の生成が終わるまで待つ
            .Skip(1) // Task完了による通知はスキップする
            .Concat(currentStoneColor);
    public Observable<int> ObservableBlackScore => scoreManager.ObservableBlackScore;
    public Observable<int> ObservableWhiteScore => scoreManager.ObservableWhiteScore;

    public SquareSequence GetDirectionSquareSequence(MatrixIndex origin, Direction8 direction) => new SquareSequence(
        squareMatrix.GetDirectionEnumerable(origin, direction)
            .TakeWhile(square => square.IsStoneExists)
            .ToList());

    void Start() {
        // PlayerPrefからサイズを取得
        var x = PlayerPrefs.GetInt("ReversiWidth", 8);
        var y = PlayerPrefs.GetInt("ReversiHeight", 8);
        Vector2Int size = new Vector2Int(x, y);

        squareMatrix = new Matrix<Square>(size.y, size.x);

        StoneFlipper flipper = new StoneFlipper(this);
        StoneProvider stoneProvider = new StoneProvider();
        SquarePlaceableInfoProvider squarePlaceableInfoProvider = new SquarePlaceableInfoProvider(size, flipper, this);

        // マス目を順に生成
        EnumerableFactory
            .FromVector2Int(size)
            .ForEach(coord => {
                Square square = Instantiate(originalSquare, transform);
                var squareSize = square.SpriteSize;
                square.transform.position = new Vector2(coord.x * squareSize.x, coord.y * squareSize.y);
                square.debugText.text = coord.ToString();

                MatrixIndex index = new MatrixIndex(coord.y, coord.x);
                // NOTE: これStoneProviderから提供してもらって、Select(_ => StoneProvider.Provide)でStoneを受け取り、それをValidateすべきだよなあ

                // マスにマウスが乗ったら、石の色を取得してValidateし、OKならBorderを変える
                square.ObservableEnter
                    .Where(_ => squarePlaceableInfoProvider.Current.IsPlaceable(index))
                    .Select(_ => flipper.GetFlippableSquareSequencesPerDirection(index, stoneProvider.GetCurrentStoneColor()))
                    .Where(sq => sq.Count() > 0)
                    .SubscribeAwait(async (sq, ct) =>
                    {
                        square.BorderStatus = BorderStatus.Selected;
                        sq.ForEach(sq => sq.BorderStatus = BorderStatus.Predicted);
                        await square.ObservableExit.FirstAsync();
                        square.BorderStatus = BorderStatus.None;
                        sq.ForEach(sq => sq.BorderStatus = BorderStatus.None);
                    });

                // マスをクリックしたら、石の色を取得してひっくり返る石を取得し、OKなら石を置く
                square.ObservableClick
                    .Select(_ => stoneProvider.GetCurrentStoneColor())
                    .Where(stoneColor => flipper.Put(index, stoneColor))
                    .Subscribe(stoneColor =>
                    {
                        // 石を置いたので、次に置く色の色を変える
                        var nextColor = stoneProvider.Switch();
                        // ターンが変わったことを通知する
                        currentStoneColor.Value = nextColor;
                    });

                // 行列にセット
                squareMatrix.Set(square, index);
            });

        // ターンが変わったら、全てのマスのBorderをリセット
        CurrentStoneColor.Subscribe(_ => squareMatrix.ForEach(sq => sq.BorderStatus = BorderStatus.None));

        // スコアを管理する
        scoreManager = new ScoreManager(squareMatrix);
        
        // 中心に持ってくる
        this.transform.position = new Vector2(
            this.transform.position.x - originalSquare.SpriteSize.x * (size.x - 1) / 2,
            this.transform.position.y - originalSquare.SpriteSize.y * (size.y - 1) / 2
        );

        // 中心のマスに初期石をセットする
        squareMatrix.Get(size.x / 2 - 1, size.y / 2 - 1).StoneStatus = StoneStatus.White;
        squareMatrix.Get(size.x / 2, size.y / 2).StoneStatus = StoneStatus.White;
        squareMatrix.Get(size.x / 2, size.y / 2 - 1).StoneStatus = StoneStatus.Black;
        squareMatrix.Get(size.x / 2 - 1, size.y / 2).StoneStatus = StoneStatus.Black;

        // マス目の生成が終わったのでタスクを完了する
        squareGenerateCompletedTask.SetResult(stoneProvider.GetCurrentStoneColor());
    }

    private class ScoreManager : IObservableScore
    {
        private ReactiveProperty<int> blackScoreSubject = new(0);
        private ReactiveProperty<int> whiteScoreSubject = new(0);
        public Observable<int> ObservableBlackScore => blackScoreSubject.AsObservable();
        public Observable<int> ObservableWhiteScore => whiteScoreSubject.AsObservable();

        public ScoreManager(IEnumerable<IColorCountChangeNotifier> colorCountChangeNotifiers)
        {
            foreach (var notifier in colorCountChangeNotifiers)
            {
                notifier.ObservableBlackStoneCount.Subscribe(count => blackScoreSubject.Value += count);
                notifier.ObservableWhiteStoneCount.Subscribe(count => whiteScoreSubject.Value += count);
            }
        }
    }
}
