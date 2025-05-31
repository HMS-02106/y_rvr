using System;

public static class StoneStatusColorExtension
{
    public static StoneStatus ToStoneStatus(this StoneColor stoneColor)
    {
        return stoneColor switch
        {
            StoneColor.White => StoneStatus.White,
            StoneColor.Black => StoneStatus.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(stoneColor), stoneColor, null)
        };
    }
    public static StoneColor? ToStoneColor(this StoneStatus stoneStatus)
    {
        return stoneStatus switch
        {
            StoneStatus.White => StoneColor.White,
            StoneStatus.Black => StoneColor.Black,
            _ => null
        };
    }
}