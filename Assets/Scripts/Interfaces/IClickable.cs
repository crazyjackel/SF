using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IClickable
{
    void Click(PointerEventData data);
}

public abstract class ClickableBehavior : MonoBehaviour, IClickable, IPointerClickHandler
{
    public abstract void Click(PointerEventData data);
    public void OnPointerClick(PointerEventData eventData)
    {
        Click(eventData);
    }
}