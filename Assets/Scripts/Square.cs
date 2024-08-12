using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtility.Extensions;

public interface ISquare {
    IStone Stone { get; }
}

/// <summary>
/// リバーシ盤面のマス
/// </summary>
public class Square : MouseHandleableMonoBehaviour, ISquare // マウスを検知したらBoardにValidateを依頼するだけ。Squareが自発的にStoneを置いたりBoarderを変えたりしない。
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SquareBorder border;
    [SerializeField]
    private Stone stone;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();
    public IStone Stone => stone;

    public StoneStatus StoneStatus { 
        get => stone.Status;
        set => stone.Status = value;
    }
    public BorderStatus BorderStatus {
        get => border.Status;
        set => border.Status = value;
    }
}
