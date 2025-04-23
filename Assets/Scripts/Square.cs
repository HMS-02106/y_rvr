using System.Collections;
using System.Collections.Generic;
using R3;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtility.Collections;
using UnityUtility.Extensions;

/// <summary>
/// リバーシ盤面のマス
/// </summary>
public class Square : MouseHandleableMonoBehaviour // マウスを検知したらBoardにValidateを依頼するだけ。Squareが自発的にStoneを置いたりBoarderを変えたりしない。
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SquareBorder border;
    [SerializeField]
    private Stone stone;

    // for debug
    public TextMeshPro debugText;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();
    public bool IsStoneExists => stone.IsExist;
    public Observable<R3.Unit> ObservableStoneChanged => stone.ObservableStatusChanged;

    public StoneStatus StoneStatus
    {
        get => stone.Status;
        set => stone.Status = value;
    }
    public BorderStatus BorderStatus
    {
        get => border.Status;
        set => border.Status = value;
    }

    public bool IsBlack => StoneStatus == StoneStatus.Black;
    public bool IsWhite => StoneStatus == StoneStatus.White;
    
    // 仮置き、後で消す
    public Stone Stone => stone;
}
