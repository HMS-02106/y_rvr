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

    private StoneStatus stoneStatus = StoneStatus.NonInitialized;
    private Subject<int> blackStoneCountSubject = new();
    private Subject<int> whiteStoneCountSubject = new();

    // for debug
    public TextMeshPro debugText;
    private int score;

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
                    blackStoneCountSubject.OnNext(score); break;
                case StoneStatus.White:
                    whiteStoneCountSubject.OnNext(score); break;
            }
            switch (stoneStatus)
            {
                case StoneStatus.Black:
                    blackStoneCountSubject.OnNext(-score); break;
                case StoneStatus.White:
                    whiteStoneCountSubject.OnNext(-score); break;
            }
            this.stoneStatus = value;
            stone.SetStatus(value);

            // 石の色に応じてエフェクトを再生する
            var color = value.ToStoneColor();
            if (color.HasValue) {
                effectDictionary[color.Value].Play();
            }
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

    void Start()
    {
        // 初期値が与えられていない場合のみ、Emptyで初期化する
        if (StoneStatus == StoneStatus.NonInitialized)
        {
            StoneStatus = StoneStatus.Empty;
        }
    }
}
