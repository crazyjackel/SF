using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayView : View<OverlayViewModel>
{
    public override void OnActivation(OverlayViewModel viewModel, CompositeDisposable disposable)
    {
        var drag = Root.Query(className: "sf-drag");
        drag.ForEach(ele => ele.AddManipulator(new DragAndDrop(ele)));
        drag.ForEach(ele => BindDragEvents(ele, viewModel, disposable));
    }

    private void BindDragEvents(VisualElement ele, OverlayViewModel viewModel, CompositeDisposable disposable)
    {
        ele.BindCallback(viewModel.OnBeginDrag).AddTo(disposable);
        ele.BindCallback(viewModel.OnDrag).AddTo(disposable);
        ele.BindCallback(viewModel.OnEndDrag).AddTo(disposable);
    }
}
