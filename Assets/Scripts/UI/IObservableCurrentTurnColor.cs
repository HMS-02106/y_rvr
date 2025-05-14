using R3;

public interface IObservableCurrentTurnColor
{
    Observable<StoneColor> CurrentStoneColor { get; }
}