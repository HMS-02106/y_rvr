using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtility.Extensions;

/// <summary>
/// リバーシ盤面のマス
/// </summary>
public class Square : MonoBehaviour/*, IMouseInputReceiver*/
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject border;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();

    void OnMouseOver()
    {
        border.SetActive(true);
    }

    void OnMouseExit()
    {
        border.SetActive(false);
    }

    void Start()
    {
        
    }
}
