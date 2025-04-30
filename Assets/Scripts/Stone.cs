using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private static readonly Dictionary<StoneStatus, Color> StatusColors = new Dictionary<StoneStatus, Color>() {
        { StoneStatus.Empty, new Color(0,0,0,0) },
        { StoneStatus.Black, UnityEngine.Color.black },
        { StoneStatus.White, UnityEngine.Color.white },
    };
    private Subject<Unit> statusChangedSubject = new Subject<Unit>();

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private StoneStatus stoneStatus;

    public StoneStatus Status
    {
        get => stoneStatus;
        set
        {
            this.stoneStatus = value;
            this.spriteRenderer.color = StatusColors[value];
            this.statusChangedSubject.OnNext(Unit.Default);
        }
    }
    public StoneColor? Color => stoneStatus.ToStoneColor();
    public bool IsExist => stoneStatus == StoneStatus.White || stoneStatus == StoneStatus.Black;
    public Observable<Unit> ObservableStatusChanged => statusChangedSubject.AsObservable();

    public bool IsOpposite(StoneColor color)
    {
        return color switch
        {
            StoneColor.Black => Color == StoneColor.White,
            StoneColor.White => Color == StoneColor.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public void TurnOver()
    {
        if (Status == StoneStatus.Black)
        {
            Status = StoneStatus.White;
        }
        else if (Status == StoneStatus.White)
        {
            Status = StoneStatus.Black;
        }
        else
        {
            throw new InvalidOperationException("石が置かれていないため裏返しできません");
        }
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

