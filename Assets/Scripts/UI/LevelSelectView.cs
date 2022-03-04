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
        var level4 = Root.Q<Button>("level4Button");
        var level5 = Root.Q<Button>("level5Button");

        level1.BindCallback(viewModel.OnClick, "level1").AddTo(disposable);
        level2.BindCallback(viewModel.OnClick, "level2").AddTo(disposable);
        level3.BindCallback(viewModel.OnClick, "level3").AddTo(disposable);
        level4.BindCallback(viewModel.OnClick, "level4").AddTo(disposable);
        level5.BindCallback(viewModel.OnClick, "level5").AddTo(disposable);
    }
}
