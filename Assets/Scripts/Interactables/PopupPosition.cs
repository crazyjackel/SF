using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPosition : MonoBehaviour, IPopUp
{
    public void PopUp(Vector3 f_pos)
    {
        this.transform.position = f_pos;
    }

}
