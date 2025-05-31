using System.Threading.Tasks;
using R3;
using UnityEngine;

public interface IObservableCurrentTurnColor
{
    Observable<StoneColor> ObservableCurrentStoneColor { get; }
}

public class TurnManager : MonoBehaviour, IObservableCurrentTurnColor
{
    public static readonly StoneColor initialStoneColor = StoneColor.White;
    private ReactiveProperty<StoneColor> currentStoneColor = new(initialStoneColor);
    private TaskCompletionSource<StoneColor> squareGenerateCompletedTask = new();

    /// <summary>
    /// 現在の石の色を取得する
    /// </summary>
    /// <returns></returns>
    public StoneColor Current => currentStoneColor.Value;

    public Observable<StoneColor> ObservableCurrentStoneColor =>
        Observable
            .FromAsync(async ct => await squareGenerateCompletedTask.Task) // マス目の生成が終わるまで待つ
            .Skip(1) // Task完了による通知はスキップする
            .Concat(currentStoneColor); // ターンが変わったら通知する;;

    /// <summary>
    /// 石を入れ替える
    /// </summary>
    /// <returns>入れ替え後の石の色</returns>
    public void Switch()
    {
        currentStoneColor.Value = currentStoneColor.Value == StoneColor.Black ? StoneColor.White : StoneColor.Black;
    }

    public void SetSquareGenerateCompleted()
    {
        squareGenerateCompletedTask.TrySetResult(currentStoneColor.Value);
    }
}