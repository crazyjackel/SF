using RedMoon.Injector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelPopUpViewModel : PopViewModel<LevelPopUpViewModel>
{
    [SerializeField]
    private SaveFile m_save;

    [SerializeField]
    private GameConstants constants;

    [SerializeField]
    private Vector3 m_pos;
    public Vector3 Position => m_pos;

    [SerializeField]
    private string m_levelRef;
    [SerializeField]
    private Board m_selectedBoard;

    [SerializeField]
    private string m_name;
    public string Name => m_name;

    ReactiveProperty<PersistentDataManager> _pDataManager = new ReactiveProperty<PersistentDataManager>();

    public ReactiveCommand<ClickEvent> OnClick;



    public override bool CanInitialize()
    {
        return _pDataManager.HasValue;
    }

    public override void OnInitialization()
    {
        m_selectedBoard = constants.Levels[m_levelRef];
        m_name = m_selectedBoard.LevelName;
        OnClick = new ReactiveCommand<ClickEvent>(_pDataManager.Select(x => x != null));
        OnClick.Subscribe(x =>
        {
            var data = _pDataManager.Value;
            try
            {
                data.selectedBoard = new KeyValuePair<string, Board>(m_levelRef, m_selectedBoard);
                m_save.LevelSelect = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(constants.PlayLevel, LoadSceneMode.Single);
            }
            catch (Exception)
            {

            }
        });
    }

    public override void NewProviderAvailable(IProvider newProvider)
    {
        DepInjector.MapProvider(newProvider, _pDataManager);
    }
    public override void PopUp(PointerEventData f_pos)
    {
        m_pos = f_pos.position;

        LevelPopUpClickable l_vClick = f_pos.pointerClick?.GetComponentInChildren<LevelPopUpClickable>();
        if (l_vClick != null)
        {
            m_levelRef = l_vClick.LevelName;
        }
    }
}
