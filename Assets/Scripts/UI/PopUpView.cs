using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpView : View<LevelPopUpViewModel>
{
    public override void OnActivation(LevelPopUpViewModel viewModel, CompositeDisposable disposable)
    {
        Root.style.position = Position.Absolute;
        Root.style.left = new StyleLength(viewModel.Position.x);
        Root.style.top = new StyleLength(Screen.currentResolution.height - viewModel.Position.y);
    }

}
