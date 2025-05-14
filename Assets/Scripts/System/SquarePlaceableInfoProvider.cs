using System.Linq;
using R3;
using UnityEngine;
using UnityUtility.Collections;
using UnityUtility.Linq;

public class SquarePlaceableInfoProvider
{
    public SquarePlaceableInfo Current { get; private set; }
    public SquarePlaceableInfoProvider(Vector2Int size, StoneFlipper flipper, IObservableCurrentTurnColor turnChanged)
    {
        // ターンが変わったタイミングでSquarePlaceableInfoを更新する
        turnChanged.ObservableCurrentStoneColor
            .Subscribe(stoneColor =>
            {
                var placeableIndexes = EnumerableFactory
                    .FromVector2Int(size)
                    .Select(coord => new MatrixIndex(coord.y, coord.x))
                    .Where(index => flipper.GetFlippableSquareSequencesPerDirection(index, stoneColor).Count() > 0)
                    .ToList();
                Current = new SquarePlaceableInfo(placeableIndexes);
            });
    }
}