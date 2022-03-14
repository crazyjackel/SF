using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopUpViewModel<VM> : ViewModel<VM>, IPopUp where VM : PopUpViewModel<VM>
{
    public abstract void PopUp(Vector3 f_pos);
}


public class LevelPopUpViewModel : PopUpViewModel<LevelPopUpViewModel>
{
    [SerializeField]
    private Vector3 m_pos;
    public Vector3 Position => m_pos;

    public override void PopUp(Vector3 f_pos)
    {
        m_pos = f_pos;
    }
}