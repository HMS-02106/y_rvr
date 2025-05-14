using R3;

public interface IObservableCurrentTurnColor
{
    Observable<StoneColor> ObservableCurrentStoneColor { get; }
}