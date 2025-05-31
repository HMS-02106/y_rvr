using System;
using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;
using UnityUtility.Extensions;

/// <summary>
/// リバーシ盤面のマス
/// </summary>
public class Square : MouseHandleableMonoBehaviour, IColorCountChangeNotifier // マウスを検知したらBoardにValidateを依頼するだけ。Squareが自発的にStoneを置いたりBoarderを変えたりしない。
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SquareBorder border;
    [SerializeField]
    private Stone stone;
    [SerializeField]
    private Dictionary<StoneColor, ParticleSystem> effectDictionary;

    private Subject<int> blackStoneCountSubject = new();
    private Subject<int> whiteStoneCountSubject = new();

    // for debug
    public TextMeshPro debugText;
    private int score;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();
    public bool IsStoneExists => stone.IsExist;

    public StoneStatus StoneStatus
    {
        get => stone.Current;
        set => stone.SetStatus(value);
    }
    public BorderStatus BorderStatus
    {
        get => border.Status;
        set => border.Status = value;
    }
    public StoneColor? StoneColor => stone.Current.ToStoneColor();

    public Observable<int> ObservableBlackStoneCount => blackStoneCountSubject.AsObservable();
    public Observable<int> ObservableWhiteStoneCount => whiteStoneCountSubject.AsObservable();

    public int Score
    {
        get => score;
        set
        {
            score = value;
            if (debugText != null)
            {
                debugText.text = score.ToString();
            }
        }
    }

    public Action SetPreviewStone(StoneColor color)
    {
        stone.SetPreview(color);
        return stone.ClearPreview;
    }

    void Awake()
    {
        // 初期値が与えられていない場合のみ、Emptyで初期化する
        if (stone.Current == StoneStatus.NonInitialized)
        {
            stone.SetStatus(StoneStatus.Empty);
        }

        stone.ObservableChangeInfo.Subscribe(changeInfo =>
        {
            if (changeInfo.CurrentStatus.HasValue)
            {
                switch (changeInfo.CurrentStatus.Value)
                {
                    case global::StoneColor.Black:
                        blackStoneCountSubject.OnNext(score); break;
                    case global::StoneColor.White:
                        whiteStoneCountSubject.OnNext(score); break;
                }
                // 石を置いたエフェクト再生
                effectDictionary[changeInfo.CurrentStatus.Value].Play();
            }
            if (changeInfo.PreviousStatus.HasValue)
            {
                switch (changeInfo.PreviousStatus.Value)
                {
                    case global::StoneColor.Black:
                        blackStoneCountSubject.OnNext(-score); break;
                    case global::StoneColor.White:
                        whiteStoneCountSubject.OnNext(-score); break;
                }
            }
        });
    }
}
