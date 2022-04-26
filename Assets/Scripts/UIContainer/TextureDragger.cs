using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using RedMoon.ReactiveKit;
using UniRx.Operators;

class TextureDragger : MouseManipulator
{
    #region Init
    protected bool m_Active;
    protected bool isFirst;
    protected bool hasDirection;
    protected bool MoveColumn;

    private Vector2 m_Start;
    private TileSeries Row;
    private TileSeries Column;

    ReactiveProperty<Vector2> offset;
    IDisposable disp;
    public TextureDragger(TileSeries Row, TileSeries Column)
    {
        this.Row = Row;
        this.Column = Column;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        offset = new ReactiveProperty<Vector2>();
        m_Active = false;
    }
    #endregion

    #region Registrations
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);

        disp = offset
            .Where(x => m_Active && !hasDirection)
            .Buffer(TimeSpan.FromSeconds(0.15f))
            .Subscribe(ls =>
            {
                if (ls.Count < 1) return;

                hasDirection = true;

                Vector2 d = ls.Last();
                MoveColumn = Math.Abs(d.y) >= Math.Abs(d.x);
            });
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);

        disp?.Dispose();
    }
    #endregion

    #region OnMouseDown
    protected void OnMouseDown(MouseDownEvent e)
    {
        if (m_Active)
        {
            e.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(e))
        {
            m_Start = e.mousePosition;
            isFirst = true;
            m_Active = true; 
            hasDirection = false;
            target.CaptureMouse();
            e.StopPropagation();
        }
    }
    #endregion

    #region OnMouseMove
    protected void OnMouseMove(MouseMoveEvent e)
    {
        if (!m_Active || !target.HasMouseCapture())
            return;

        offset.Value = (e.mousePosition - m_Start);

        if (!hasDirection) 
            return;

        Column?.SetOffset(MoveColumn ? - offset.Value.y : 0);
        Row?.SetOffset(MoveColumn ? 0 : -offset.Value.x);

        e.StopPropagation();
    }
    #endregion

    #region OnMouseUp
    protected void OnMouseUp(MouseUpEvent e)
    {
        if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
            return;

        m_Active = false;

        TileSeries series = MoveColumn ? Column : Row;
        series?.MoveTiles(Mathf.RoundToInt(series.ClampedOffset.Value / TileSeries.tileSize));
        series?.ResetOffset();

        target.ReleaseMouse();
        e.StopPropagation();
    }
    #endregion
}



/*
class TextureDragger : PointerManipulator
{
    private Vector2 targetStartPosition { get; set; }
    private Vector3 pointerStartPosition { get; set; }

    protected bool m_Active;

    public TextureDragger(VisualElement target)
    {
        this.target = target;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
    }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        targetStartPosition = target.transform.position;
        pointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        m_Active = true;
    }
    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (!m_Active || !target.HasPointerCapture(evt.pointerId)) return;

        Vector2 diff = (evt.position - pointerStartPosition);
        diff = Math.Abs(diff.x) > Math.Abs(diff.y) ? new Vector2(diff.x, 0) : new Vector2(0, diff.y);

        target.style.top = Mathf.Clamp(target.layout.y + diff.y, -target.layout.height / 3, target.layout.height / 3);
        target.style.left = Mathf.Clamp(target.layout.x + diff.x, -target.layout.width / 3, target.layout.width / 3);
    }
    private void PointerUpHandler(PointerUpEvent evt)
    {
        if (m_Active && target.HasPointerCapture(evt.pointerId))
        {
            target.style.top = target.layout.y;
            target.style.left = target.layout.x;
            target.ReleasePointer(evt.pointerId);
        }
    }
}*/