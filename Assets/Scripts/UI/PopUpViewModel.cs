using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PopViewModel<VM> : ViewModel<VM>, IPopUp where VM : PopViewModel<VM>
{
    public abstract void PopUp(PointerEventData f_pos);
}