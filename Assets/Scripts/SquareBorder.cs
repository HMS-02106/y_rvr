using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBorder : MonoBehaviour
{
    private Dictionary<BorderStatus, Color> borderColors = new Dictionary<BorderStatus, Color>() {
        { BorderStatus.None, Color.clear },
        { BorderStatus.Selected, Color.white },
        { BorderStatus.Pressed, Color.yellow },
    };

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private BorderStatus status;
    public BorderStatus Status { 
        get => status;
        set {
            spriteRenderer.color = borderColors[value];
            status = value;
        }    
    }

    void Start() {
        this.Status = BorderStatus.None;
    }
}

public enum BorderStatus {
    None,
    Selected,
    Pressed,
}