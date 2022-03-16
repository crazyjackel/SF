using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuView : View<MainMenuViewModel>
{
    public override void OnActivation(MainMenuViewModel viewModel, CompositeDisposable disposable)
    {
        var button1 = Root.Q<Button>("worldMapButton");

        button1.BindClick(viewModel.Onclick).AddTo(disposable);
    }
}
