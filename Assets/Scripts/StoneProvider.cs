// 今からおこうとしているStoneを提供する


public class StoneProvider
{
    public static readonly StoneColor initialStoneColor = StoneColor.White;
    private bool isBlack = initialStoneColor == StoneColor.Black;

    public StoneColor GetStoneColor()
    {
        return isBlack ? StoneColor.Black : StoneColor.White;
    }

    public void Switch() => isBlack = !isBlack;
}