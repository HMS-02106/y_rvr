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
    public StoneColor? StoneColor => stone.Color;

    public bool IsBlack => StoneStatus == StoneStatus.Black;
    public bool IsWhite => StoneStatus == StoneStatus.White;

    public Observable<int> ObservableBlackStoneCount => stone.ObservableBlackStoneCount;
    public Observable<int> ObservableWhiteStoneCount => stone.ObservableWhiteStoneCount;
}
