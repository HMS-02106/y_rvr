using System.Collections.Generic;
using R3;
using Unity.VisualScripting;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private static readonly Dictionary<StoneStatus, Color> StatusColors = new() {
        { StoneStatus.Empty, new Color(0,0,0,0) },
        { StoneStatus.Black, Color.black },
        { StoneStatus.White, Color.white },
    };

    private static readonly Dictionary<StoneColor, Color> PreviewStoneColors = new() {
        { StoneColor.Black, Color.black.WithAlpha(0.5f) },
        { StoneColor.White, Color.white.WithAlpha(0.5f) },
    };

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Subject<ChangeInfo> changeInfoSubject = new();

    public StoneStatus Current { get; private set; } = StoneStatus.NonInitialized;
    public bool IsExist => Current == StoneStatus.White || Current == StoneStatus.Black;
    public Observable<ChangeInfo> ObservableChangeInfo => changeInfoSubject.AsObservable();

    public void SetStatus(StoneStatus newStatus)
    {
        if (Current == newStatus) return;
        spriteRenderer.color = StatusColors[newStatus];
        changeInfoSubject.OnNext(new ChangeInfo(
            Current.ToStoneColor(),
            newStatus.ToStoneColor()
        ));
        Current = newStatus;
    }

    public void SetPreview(StoneColor color)
    {
        if (IsExist) return; // Previewは石が存在しない時のみ有効
        this.spriteRenderer.color = PreviewStoneColors[color];
    }
    public void ClearPreview()
    {
        if (IsExist) return; // Previewは石が存在しない時のみ有効
        this.spriteRenderer.color = StatusColors[StoneStatus.Empty];
    }

    public struct ChangeInfo
    {
        public StoneColor? PreviousStatus { get; }
        public StoneColor? CurrentStatus { get; }

        public ChangeInfo(StoneColor? previous, StoneColor? current)
        {
            this.PreviousStatus = previous;
            this.CurrentStatus = current;
        }
    }
}

