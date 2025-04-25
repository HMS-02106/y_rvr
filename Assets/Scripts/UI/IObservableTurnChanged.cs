using R3;

public interface IObservableTurnChanged
{
    Observable<StoneStatus> ObservableTurnChanged { get; }
}