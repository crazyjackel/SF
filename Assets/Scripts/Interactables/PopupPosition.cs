using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupPosition : MonoBehaviour, IPopUp
{
    public void PopUp(PointerEventData f_pos)
    {
        this.transform.position = f_pos.position;
    }
}
