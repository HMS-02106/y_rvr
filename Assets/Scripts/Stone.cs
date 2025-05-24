using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private static readonly Dictionary<StoneStatus, Color> StatusColors = new Dictionary<StoneStatus, Color>() {
        { StoneStatus.Empty, new Color(0,0,0,0) },
        { StoneStatus.Black, Color.black },
        { StoneStatus.White, Color.white },
    };

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SetStatus(StoneStatus status)
    {
        this.spriteRenderer.color = StatusColors[status];
    }
}

