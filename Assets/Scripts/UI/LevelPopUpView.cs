using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelPopUpView : View<LevelPopUpViewModel>
{
    public override void OnActivation(LevelPopUpViewModel viewModel, CompositeDisposable disposable)
    {
        Root.style.position = Position.Absolute;
        Root.style.left = new StyleLength(viewModel.Position.x);
        Root.style.top = new StyleLength(Screen.currentResolution.height - viewModel.Position.y);

        var label1 = Root.Q<Label>("nameLabel");
        label1.text = viewModel.Name;
        
        var level1 = Root.Q<Button>("playButton");
        level1.BindClick(viewModel.OnClick).AddTo(disposable);
    }
}
