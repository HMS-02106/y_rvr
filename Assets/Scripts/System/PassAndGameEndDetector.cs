using UnityEngine;
using R3;
using System.Linq;

/// <summary>
/// 置ける石がなくなったらパスし、両者とも連続でパスしたらゲーム終了と検知する
/// </summary>
public class PassAndGameEndDetector
{
    private int passCount = 0;
    public PassAndGameEndDetector(SquarePlaceableInfoProvider squarePlaceableInfoProvider, IObservableCurrentTurnColor observableCurrentTurnColor, TurnManager turnManager)
    {
        // ターンが変わるたびにEmptyの個数を数える
        observableCurrentTurnColor.ObservableCurrentStoneColor.Subscribe(_ =>
        {
            if (squarePlaceableInfoProvider.Current.IsAnyPlaceable())
            {
                // 置ける場所がある
                passCount = 0;
                return;
            }

            // 置ける場所がない
            // 2回連続でパスしたらゲーム終了
            if (++passCount >= 2)
            {
                // ゲーム終了
                Debug.Log("Game End");
            }
            else
            {
                // パス
                Debug.Log("Pass");
                turnManager.Switch();
            }
            
        });
    }
}
