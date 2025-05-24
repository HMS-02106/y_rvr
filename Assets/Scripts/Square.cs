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

    private StoneStatus stoneStatus = StoneStatus.NonInitialized;
    private Subject<int> blackStoneCountSubject = new();
    private Subject<int> whiteStoneCountSubject = new();

    // for debug
    public TextMeshPro debugText;
    private int point;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();
    public bool IsStoneExists => stoneStatus == StoneStatus.White || stoneStatus == StoneStatus.Black;


    public StoneStatus StoneStatus
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
            stone.SetStatus(value);
        }
    }
    public BorderStatus BorderStatus
    {
        get => border.Status;
        set => border.Status = value;
    }
    public StoneColor? StoneColor => stoneStatus.ToStoneColor();


    public Observable<int> ObservableBlackStoneCount => blackStoneCountSubject.AsObservable();
    public Observable<int> ObservableWhiteStoneCount => whiteStoneCountSubject.AsObservable();

    public int Point
    {
        get => point;
        set
        {
            point = value;
            if (debugText != null)
            {
                debugText.text = point.ToString();
            }
        }
    }

    void Start()
    {
        // 初期値が与えられていない場合のみ、Emptyで初期化する
        if (StoneStatus == StoneStatus.NonInitialized)
        {
            StoneStatus = StoneStatus.Empty;
        }
    }
}
