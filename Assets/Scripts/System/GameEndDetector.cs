using UnityEngine;
using R3;
using System.Linq;

public class GameEndDetector
{
    public GameEndDetector(SquarePlaceableInfoProvider squarePlaceableInfoProvider, Board board)
    {
        // ターンが変わるたびにEmptyの個数を数える
        board.ObservableCurrentStoneColor.Where(_ => !squarePlaceableInfoProvider.Current.IsAnyPlaceable()).Subscribe(_ =>
        {
            Debug.Log("Game End");
        });
    }
}
