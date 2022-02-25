using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectView : View<LevelSelectViewModel>
{
    public override void OnActivation(LevelSelectViewModel viewModel, CompositeDisposable disposable)
    {
        var level1 = Root.Q<Button>("level1Button");
        var level2 = Root.Q<Button>("level2Button");
        var level3 = Root.Q<Button>("level3Button");

        level1.BindCallback(viewModel.OnClick, "level1").AddTo(disposable);
        level2.BindCallback(viewModel.OnClick, "level2").AddTo(disposable);
        level3.BindCallback(viewModel.OnClick, "level3").AddTo(disposable);
    }
}
