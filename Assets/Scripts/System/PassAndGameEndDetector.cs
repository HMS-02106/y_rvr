using System;
using UnityEngine;
using R3;
using System.Linq;

/// <summary>
/// 置ける石がなくなったらパスし、両者とも連続でパスしたらゲーム終了と検知する
/// </summary>
public class PassAndGameEndDetector : MonoBehaviour
{
    public event Action OnGameEnd;
    public event Action OnPass;
    public void StartDetection(SquarePlaceableInfoProvider squarePlaceableInfoProvider, IObservableCurrentTurnColor observableCurrentTurnColor)
    {
        int passCount = 0;
        // ターンが変わるたびに置ける場所があるかチェックする
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
                OnGameEnd?.Invoke();
            }
            else
            {
                // パス
                Debug.Log("Pass");
                OnPass?.Invoke();
            }
        })
        .AddTo(this);
    }
}
