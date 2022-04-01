using RedMoon.Injector;
using RedMoon.ReactiveKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectViewModel : ViewModel<LevelSelectViewModel>
{
    [SerializeField]
    private GameConstants constants;

    ReactiveProperty<PersistentDataManager> _pDataManager = new ReactiveProperty<PersistentDataManager>();

    public ReactiveCommand<(ClickEvent, string)> OnClick;

    public override bool CanInitialize()
    {
        return _pDataManager.HasValue;
    }

    public override void OnInitialization()
    {
        OnClick = new ReactiveCommand<(ClickEvent, string)>();
        OnClick.Subscribe(x =>
        {
            var ce = x.Item1;
            var str = x.Item2;
            var data = _pDataManager.Value;
            try
            {
                data.selectedBoard = new KeyValuePair<string, Board>(str, constants.Levels[str]);
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
}
