using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private static readonly Dictionary<StatusType, Color> StatusColors = new Dictionary<StatusType,Color>() {
        { StatusType.Empty, new Color(0,0,0,0) },
        { StatusType.Black, Color.black },
        { StatusType.White, Color.white },
    };
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private StatusType status;

    private void SetStatus(StatusType status) {
        this.status = status;
        this.spriteRenderer.color = StatusColors[status];
    }

    public bool IsEmpty => status == StatusType.Empty;
    
    public void SetWhite() => SetStatus(StatusType.White);
    public void SetBlack() => SetStatus(StatusType.Black);
    public void TurnOver() {
        if (status == StatusType.Black) {
            SetStatus(StatusType.White);
        } else if (status == StatusType.White) {
            SetStatus(StatusType.Black);
        } else {
            throw new InvalidOperationException("石が置かれていないため裏返しできません");
        }
    }

    void Start() {
        SetStatus(StatusType.Empty);
    }

    enum StatusType {
        Empty,
        White,
        Black,
    }
}
