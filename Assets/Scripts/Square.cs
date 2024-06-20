using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtility.Extensions;

/// <summary>
/// リバーシ盤面のマス
/// </summary>
public class Square : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SquareBorder border;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();

    void OnMouseEnter() {
        border.Status = SquareBorder.BorderType.Selected;
    }

    void OnMouseExit() {
        border.Status = SquareBorder.BorderType.None;
    }

    void OnMouseDown() {
        border.Status = SquareBorder.BorderType.Pressed;
    }

    void OnMouseUp() {
        border.Status = SquareBorder.BorderType.Selected;
    }
}
