using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldSelectView : View<WorldSelectViewModel>
{
    public override void OnActivation(WorldSelectViewModel viewModel, CompositeDisposable disposable)
    {
        Button next = Root.Q<Button>("NextButton");
        Button previous = Root.Q<Button>("PreviousButton");
        Button back = Root.Q<Button>("BackButton");

        next.BindClick(viewModel.OnClickNext);
        previous.BindClick(viewModel.OnClickPrevious);
        back.BindCallback(viewModel.OnClickBack);
    }
}
