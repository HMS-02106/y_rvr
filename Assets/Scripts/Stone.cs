using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class Stone : MonoBehaviour, IColorCountChangeNotifier
{
    private static readonly Dictionary<StoneStatus, Color> StatusColors = new Dictionary<StoneStatus, Color>() {
        { StoneStatus.Empty, new Color(0,0,0,0) },
        { StoneStatus.Black, UnityEngine.Color.black },
        { StoneStatus.White, UnityEngine.Color.white },
    };
    private Subject<Unit> statusChangedSubject = new();
    private Subject<int> blackStoneCountSubject = new();
    private Subject<int> whiteStoneCountSubject = new();


    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private StoneStatus stoneStatus;

    public StoneStatus Status
    {
        get => stoneStatus;
        set
        {
            if (stoneStatus == value) return;
            // 現在の色に応じて色数の変化を通知する
            switch (value)
            {
                case StoneStatus.Black:
                    blackStoneCountSubject.OnNext(1); break;
                case StoneStatus.White:
                    whiteStoneCountSubject.OnNext(1); break;
            }
            switch (stoneStatus)
            {
                case StoneStatus.Black:
                    blackStoneCountSubject.OnNext(-1); break;
                case StoneStatus.White:
                    whiteStoneCountSubject.OnNext(-1); break;
            }
            this.stoneStatus = value;
            this.spriteRenderer.color = StatusColors[value];
        }
    }
    public StoneColor? Color => stoneStatus.ToStoneColor();
    public bool IsExist => stoneStatus == StoneStatus.White || stoneStatus == StoneStatus.Black;
    public Observable<Unit> ObservableStatusChanged => statusChangedSubject.AsObservable();
    public Observable<int> ObservableBlackStoneCount => blackStoneCountSubject.AsObservable();
    public Observable<int> ObservableWhiteStoneCount => whiteStoneCountSubject.AsObservable();

    public bool IsOpposite(StoneColor color)
    {
        return color switch
        {
            StoneColor.Black => Color == StoneColor.White,
            StoneColor.White => Color == StoneColor.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    void Start()
    {
        // 初期値が与えられていない場合のみ、Emptyで初期化する
        if (Status == StoneStatus.NonInitialized)
        {
            Status = StoneStatus.Empty;
        }
    }
}

