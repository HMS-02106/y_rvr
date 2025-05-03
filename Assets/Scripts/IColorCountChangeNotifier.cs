using R3;
public interface IColorCountChangeNotifier
{
    Observable<int> ObservableBlackStoneCount { get; }
    Observable<int> ObservableWhiteStoneCount { get; }
}