
using System;
using UnityEngine;

public class MouseHandleableMonoBehaviour : MonoBehaviour {
    public enum MouseStatus {
        None,
        Hover,
        Down,
        DragIn,
        DragOut,
    }

    protected MouseStatus status;

    void OnMouseEnter() {
        if (Input.GetMouseButton(0)) {
            status = MouseStatus.DragIn;
        } else {
            status = MouseStatus.Hover;
            OnEnter?.Invoke();
        }
    }
    void OnMouseExit() {
        if (Input.GetMouseButton(0)) {
            status = MouseStatus.DragOut;
            OnDragOut?.Invoke();
        } else {
            status = MouseStatus.None;
            OnExit?.Invoke();
        }
    }
    void OnMouseDown() {
        status = MouseStatus.Down;
        OnDown?.Invoke();
    }
    void OnMouseUp() {
        if (status == MouseStatus.Down) {
            status = MouseStatus.None;
            OnClick?.Invoke();
        }
    }

    public event Action OnClick;
    public event Action OnDown;
    public event Action OnUp;
    public event Action OnEnter;
    public event Action OnExit;
    public event Action OnDragIn;
    public event Action OnDragOut;
}