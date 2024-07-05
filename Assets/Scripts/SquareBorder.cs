using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBorder : MonoBehaviour
{
    public enum BorderType {
        None,
        Selected,
        Pressed,
    }

    private Dictionary<BorderType, Color> borderColors = new Dictionary<BorderType, Color>() {
        { BorderType.None, Color.clear },
        { BorderType.Selected, Color.white },
        { BorderType.Pressed, Color.yellow },
    };

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private BorderType status;
    public BorderType Status { 
        get => status;
        set {
            spriteRenderer.color = borderColors[value];
            status = value;
        }    
    }

    void Start() {
        this.Status = BorderType.None;
    }
}
