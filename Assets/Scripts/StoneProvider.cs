// 今からおこうとしているStoneを提供する


public interface IStoneProvider {
    Stone GetStone();
    StoneStatus GetNextStoneStatus();
    void Switch();
}
public class StoneProvider : IStoneProvider
{
    private bool isBlack = false;

    public Stone GetStone()
    {
        throw new System.NotImplementedException();
    }

    public StoneStatus GetNextStoneStatus()
    {
        return isBlack ? StoneStatus.Black : StoneStatus.White;
    }

    public void Switch() => isBlack = !isBlack;
}