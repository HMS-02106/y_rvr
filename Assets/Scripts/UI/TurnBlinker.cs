using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TurnBlinker : SerializedMonoBehaviour
{
    [SerializeField]
    IObservableCurrentTurnColor observableTurnChanged;
    [SerializeField]
    private Outline blackOutline;
    [SerializeField]
    private Outline whiteOutline;

    private Vector2 defaultValue;
    private StoneColor nowTurn = StoneProvider.initialStoneColor;
    private Dictionary<StoneColor, Outline> stoneStatusOutlineMap;
    void Start()
    {
        // 白黒とそれに該当するOutlineを紐づける
        stoneStatusOutlineMap = new()
        {
            { StoneColor.Black, blackOutline },
            { StoneColor.White, whiteOutline },
        };

        // 初期値をインスペクタの値から取得する
        defaultValue = blackOutline.effectDistance;
        // 一旦アウトラインなしにする
        blackOutline.effectDistance = Vector2.zero;
        whiteOutline.effectDistance = Vector2.zero;

        // ターンが変わったら
        observableTurnChanged
            .CurrentStoneColor
            .Subscribe(color =>
            {
                stoneStatusOutlineMap[nowTurn].effectDistance = Vector2.zero;
                nowTurn = color;
            });

        int count = 0;
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => count++ % 2 == 0)
            .Subscribe(isFlash => stoneStatusOutlineMap[nowTurn].effectDistance = isFlash ? defaultValue : Vector2.zero);
    }
}
