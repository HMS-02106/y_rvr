using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtility.Extensions;

/// <summary>
/// リバーシ盤面のマス
/// </summary>
public class Square : MouseHandleableMonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SquareBorder border;
    [SerializeField]
    private Stone stone;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();

    void Start()
    {
        this.OnEnter += () => border.Status = SquareBorder.BorderType.Selected;
        this.OnExit += () => border.Status = SquareBorder.BorderType.None;
        this.OnDown += () => border.Status = SquareBorder.BorderType.Pressed;
        this.OnUp += () => border.Status = SquareBorder.BorderType.None;
        this.OnDragOut += () => border.Status = SquareBorder.BorderType.None;
        this.OnClick += () =>
        {
            if (stone.IsEmpty) {
                stone.SetBlack();
            } else {
                stone.TurnOver();
            }
        };
    }
}
