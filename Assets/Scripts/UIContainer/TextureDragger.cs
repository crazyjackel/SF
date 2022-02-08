using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

class TextureDragger : MouseManipulator
{
    #region Init
    private Vector2 m_Start;
    protected bool m_Active;
    protected bool isFirst;
    protected bool moveY;

    private TileSeries Row;
    public TextureDragger(TileSeries Row)
    {
        this.Row = Row;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false;
    }
    #endregion

    #region Registrations
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
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
            m_Start = e.localMousePosition;
            isFirst = true;
            m_Active = true;
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

        Vector2 diff = (e.localMousePosition - m_Start) / 30;
        if (isFirst)
        {
            isFirst = false;
            if(Math.Abs(diff.x) > Math.Abs(diff.y))
            {
                moveY = false;
            }
            else
            {
                moveY = true;
            }
        }

        if (moveY)
        {

            target.style.top = target.layout.y + diff.y;
        }
        else
        {
            Row.Offset.Value = Row.Offset.Value - new Vector2(diff.x, 0);
        }

        e.StopPropagation();
    }
    #endregion

    #region OnMouseUp
    protected void OnMouseUp(MouseUpEvent e)
    {
        if (!m_Active || !target.HasMouseCapture() || !CanStopManipulation(e))
            return;

        m_Active = false;
        Row.Offset.Value = new Vector2(0, 0);
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