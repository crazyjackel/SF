using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelPopUpClickable : PopUpClickable
{
    [SerializeField]
    private string m_level;
    public string LevelName => m_level;

}
