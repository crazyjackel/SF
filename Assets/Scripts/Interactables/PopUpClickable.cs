using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUpClickable : ClickableBehavior
{
    [SerializeField]
    private GameObject m_popUpPrefab;

    private GameObject m_popUp;

    public override void Click(PointerEventData data)
    {
        if (m_popUp == null)
        {
            m_popUpPrefab.SetActive(false);
            m_popUp = Instantiate(m_popUpPrefab);
        }

        if (m_popUp.activeInHierarchy)
        {
            m_popUp.SetActive(false);
        }
        else
        {
            SetupGameObject(m_popUp, data);
            m_popUp.SetActive(true);
        }
    }
    protected virtual void SetupGameObject(GameObject f_popUp, PointerEventData data)
    {
        IPopUp[] popUps = f_popUp.GetComponentsInChildren<IPopUp>();
        foreach (IPopUp pop in popUps) pop.PopUp(data);
    }
}
