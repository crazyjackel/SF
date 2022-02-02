using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : PointerManipulator
{
    public DragAndDrop(VisualElement target)
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

    private Vector2 targetStartPosition { get; set; }
    private Vector3 pointerStartPosition { get; set; }

    private bool enabled { get; set; }


    private void PointerDownHandler(PointerDownEvent evt)
    {
        targetStartPosition = target.transform.position;
        pointerStartPosition = evt.position;
        target.CapturePointer(evt.pointerId);
        enabled = true;
        using (var customEvt = BeginDragEvent.GetPooled(evt))
        {
            customEvt.element = target;
            target.SendEvent(customEvt);
        }
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPosition;

            target.transform.position = new Vector2(targetStartPosition.x + pointerDelta.x, targetStartPosition.y + pointerDelta.y);

            using (var customEvt = OnDragEvent.GetPooled(evt))
            {
                customEvt.element = target;
                target.SendEvent(customEvt);
            }
        }
    }

    private void PointerUpHandler(PointerUpEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            target.transform.position = targetStartPosition;
            target.ReleasePointer(evt.pointerId);
            using (var customEvt = EndDragEvent.GetPooled(evt))
            {
                customEvt.element = target;
                target.SendEvent(customEvt);
            }
        }
    }
}
public class BeginDragEvent : PointerEventBase<BeginDragEvent>
{
    public VisualElement element { get; set; }
}
public class OnDragEvent : PointerEventBase<OnDragEvent>
{
    public VisualElement element { get; set; }
}
public class EndDragEvent : PointerEventBase<EndDragEvent> 
{
    public VisualElement element { get; set; }
}