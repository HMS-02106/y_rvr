using System;
using UnityEngine;

public readonly struct ReversiConf
{
    public int RowSize { get; }
    public int ColumnSize { get; }
    public int MinPoint { get; }
    public int MaxPoint { get; }
    public PointWeight PointWeight { get; }

    private ReversiConf(int rowSize, int columnSize, int minPoint, int maxPoint, PointWeight pointWeight)
    {
        RowSize = rowSize;
        ColumnSize = columnSize;
        MinPoint = minPoint;
        MaxPoint = maxPoint;
        PointWeight = pointWeight;
    }

    public static ReversiConf CreateFromPlayerPrefs()
    {
        var rowSize = PlayerPrefs.GetInt("ReversiHeight", 8);
        var columnSize = PlayerPrefs.GetInt("ReversiWidth", 8);
        var minPoint = PlayerPrefs.GetInt("ReversiMinPoint", 1);
        var maxPoint = PlayerPrefs.GetInt("ReversiMaxPoint", 10);
        var pointWeight = Enum.Parse<PointWeight>(PlayerPrefs.GetString("ReversiPointWeight", PointWeight.Flat.ToString()));

        return new ReversiConf(rowSize, columnSize, minPoint, maxPoint, pointWeight);
    }
}