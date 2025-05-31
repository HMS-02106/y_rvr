using System.Collections.Generic;
using UnityUtility.Collections;

public class SquarePlaceableInfo
{
    private readonly List<MatrixIndex> placeableIndexes = new();

    public SquarePlaceableInfo(IEnumerable<MatrixIndex> placeableIndexes)
    {
        this.placeableIndexes.AddRange(placeableIndexes);
    }

    public bool IsPlaceable(MatrixIndex index)
    {
        return placeableIndexes.Contains(index);
    }

    public bool IsAnyPlaceable() => placeableIndexes.Count > 0;
}