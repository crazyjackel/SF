using Bewildered;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelPopUpClickable : PopUpClickable
{
    static UHashSet<LevelPopUpClickable> popups = new UHashSet<LevelPopUpClickable>();

    [SerializeField]
    private string m_level;
    public string LevelName => m_level;

    private void Start()
    {
        popups.Add(this);
    }

    public override void Click(PointerEventData data)
    {
        base.Click(data);
        if (m_popUp.activeInHierarchy)
        {
            popups.Where(x => x != this && x.m_popUp != null).ForEach((x) => {
                x.m_popUp?.SetActive(false);
            });
        }
    }
}
