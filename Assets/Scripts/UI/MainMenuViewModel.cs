using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuViewModel : ViewModel<MainMenuViewModel>
{
    public IReactiveCommand<ClickEvent> Onclick { get; private set; }
    public override void OnInitialization()
    {
        Onclick = new ReactiveCommand<ClickEvent>();
        Onclick.Subscribe(x =>
        {
            Debug.Log("Loading Next Level...");
            SceneManager.LoadScene("LevelSelectSceneWorldJungle", LoadSceneMode.Single);
        });
    }
}
