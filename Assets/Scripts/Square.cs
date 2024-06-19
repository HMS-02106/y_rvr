using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtility.Extensions;

/// <summary>
/// リバーシ盤面のマス
/// </summary>
public class Square : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public Vector2 SpriteSize => spriteRenderer.bounds.size.DisZ();
    
    void Start()
    {
        
    }
}
