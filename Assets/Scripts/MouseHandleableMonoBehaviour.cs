
using System;
using R3;
using UnityEngine;

public interface IObservableMouseHandler {
    Observable<Unit> ObservableClick { get; }
    Observable<Unit> ObservableDown { get; }
    Observable<Unit> ObservableUp { get; }
    Observable<Unit> ObservableEnter { get; }
    Observable<Unit> ObservableExit { get; }
    Observable<Unit> ObservableDragIn { get; }
    Observable<Unit> ObservableDragOut { get; }
}
public class MouseHandleableMonoBehaviour : MonoBehaviour, IObservableMouseHandler {
    public enum MouseStatus {
        None,
        Hover,
        Down,
        DragIn,
        DragOut,
    }

    private Subject<Unit> clickSubject = new Subject<Unit>();
    private Subject<Unit> downSubject = new Subject<Unit>();
    private Subject<Unit> upSubject = new Subject<Unit>();
    private Subject<Unit> enterSubject = new Subject<Unit>();
    private Subject<Unit> exitSubject = new Subject<Unit>();
    private Subject<Unit> dragInSubject = new Subject<Unit>();
    private Subject<Unit> dragOutSubject = new Subject<Unit>();


    protected MouseStatus status;

    void OnMouseEnter() {
        if (Input.GetMouseButton(0)) {
            status = MouseStatus.DragIn;
        } else {
            status = MouseStatus.Hover;
            enterSubject.OnNext(Unit.Default);
        }
    }
    void OnMouseExit() {
        if (Input.GetMouseButton(0)) {
            status = MouseStatus.DragOut;
            dragOutSubject.OnNext(Unit.Default);
        } else {
            status = MouseStatus.None;
            exitSubject.OnNext(Unit.Default);
        }
    }
    void OnMouseDown() {
        status = MouseStatus.Down;
        downSubject.OnNext(Unit.Default);
    }
    void OnMouseUp() {
        if (status == MouseStatus.Down) {
            status = MouseStatus.None;
            clickSubject.OnNext(Unit.Default);
        }
    }

    public Observable<Unit> ObservableClick => clickSubject.AsObservable();
    public Observable<Unit> ObservableDown => downSubject.AsObservable();
    public Observable<Unit> ObservableUp => upSubject.AsObservable();
    public Observable<Unit> ObservableEnter => enterSubject.AsObservable();
    public Observable<Unit> ObservableExit => exitSubject.AsObservable();
    public Observable<Unit> ObservableDragIn => dragInSubject.AsObservable();
    public Observable<Unit> ObservableDragOut => dragOutSubject.AsObservable();
}