using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtility.Linq;
using UnityUtility.Collections;
using UnityUtility.Enums;
using R3;
using MoreLinq.Extensions;
using System.Threading.Tasks;

public class Board : MonoBehaviour, IObservableScore
{
    [SerializeField]
    private Square originalSquare;
    [SerializeField]
    private TurnManager turnManager;
    [SerializeField]
    private PassAndGameEndDetector passAndGameEndDetector;

    private SquareMatrix squareMatrix;
    private ScoreManager scoreManager;

    public IReadOnlyMatrix<Square> Squares => squareMatrix;

    public Observable<int> ObservableBlackScore => scoreManager.ObservableBlackScore;
    public Observable<int> ObservableWhiteScore => scoreManager.ObservableWhiteScore;

    public SquareSequence GetDirectionSquareSequence(MatrixIndex origin, Direction8 direction) => new(
        squareMatrix.GetDirectionEnumerable(origin, direction)
            .TakeWhile(square => square.IsStoneExists)
            .ToList());

    void Awake() {
        Application.targetFrameRate = 30;

        var conf = ReversiConf.CreateFromPlayerPrefs();

        Vector2Int size = new(conf.ColumnSize, conf.RowSize);
        squareMatrix = new SquareMatrix(size.y, size.x);

        StoneFlipper flipper = new(this);
        SquarePlaceableInfoProvider squarePlaceableInfoProvider = new(size, flipper, turnManager, gameObject);

        // パスとゲーム終了の検知を開始して、パスしたらターンを変える
        passAndGameEndDetector.StartDetection(squarePlaceableInfoProvider, turnManager);
        passAndGameEndDetector.OnPass += _ => turnManager.Switch();

        // マス目を順に生成
        EnumerableFactory
            .FromVector2Int(size)
            .ForEach(coord => {
                Square square = Instantiate(originalSquare, transform);
                var squareSize = square.SpriteSize;
                square.transform.position = new Vector2(coord.x * squareSize.x, coord.y * squareSize.y);

                MatrixIndex index = new MatrixIndex(coord.y, coord.x);

                // マスにマウスが乗ったら、石の色を取得してValidateし、OKならBorderを変える
                square.ObservableEnter
                    .Where(_ => squarePlaceableInfoProvider.Current.IsPlaceable(index))
                    .Select(_ => flipper.GetFlippableSquareSequencesPerDirection(index, turnManager.Current))
                    .Where(sq => sq.Count() > 0)
                    .SubscribeAwait(async (sq, ct) =>
                    {
                        square.BorderStatus = BorderStatus.Selected;
                        var clearAction = square.SetPreviewStone(turnManager.Current);
                        sq.ForEach(sq => sq.BorderStatus = BorderStatus.Predicted);
                        await square.ObservableExit.FirstAsync();
                        square.BorderStatus = BorderStatus.None;
                        clearAction();
                        sq.ForEach(sq => sq.BorderStatus = BorderStatus.None);
                    })
                    .AddTo(this);

                // マスをクリックしたら、石の色を取得してひっくり返る石を取得し、OKなら石を置き、ターンを変える
                square.ObservableClick
                    .Select(_ => turnManager.Current)
                    .WhereAwait(async (stoneColor, c) => await flipper.Put(index, stoneColor))
                    .Subscribe(stoneColor => turnManager.Switch())
                    .AddTo(this);

                // 行列にセット
                squareMatrix.Set(square, index);
            });

        // ポイント配分種別に基づいて、マスにポイントを割り当てる
        PointAssigner.AssignTo(squareMatrix, conf.PointWeight, conf.MinPoint, conf.MaxPoint);

        // ターンが変わったら、全てのマスのBorderをリセット
        turnManager.ObservableCurrentStoneColor.Subscribe(_ => squareMatrix.ForEach(sq => sq.BorderStatus = BorderStatus.None)).AddTo(this);

        // スコアを管理する
        scoreManager = new ScoreManager(squareMatrix, gameObject);
        
        // 中心に持ってくる
        this.transform.position = new Vector2(
            this.transform.position.x - originalSquare.SpriteSize.x * (size.x - 1) / 2,
            this.transform.position.y - originalSquare.SpriteSize.y * (size.y - 1) / 2
        );

        // 中心のマスに初期石をセットする
        squareMatrix.SetInitialStones();

        // マス目の生成が終わったのでタスクを完了する
        turnManager.SetSquareGenerateCompleted();
    }

    private class ScoreManager : IObservableScore
    {
        private ReactiveProperty<int> blackScoreSubject = new(0);
        private ReactiveProperty<int> whiteScoreSubject = new(0);
        public Observable<int> ObservableBlackScore => blackScoreSubject.AsObservable();
        public Observable<int> ObservableWhiteScore => whiteScoreSubject.AsObservable();

        public ScoreManager(IEnumerable<IColorCountChangeNotifier> colorCountChangeNotifiers, GameObject gameObject)
        {
            foreach (var notifier in colorCountChangeNotifiers)
            {
                notifier.ObservableBlackStoneCount.Subscribe(count => blackScoreSubject.Value += count).AddTo(gameObject);
                notifier.ObservableWhiteStoneCount.Subscribe(count => whiteScoreSubject.Value += count).AddTo(gameObject);
            }
        }
    }
}
