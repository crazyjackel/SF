using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedMoon.ReactiveKit;
using UniRx;
using UnityEngine.UIElements;
using RedMoon.Injector;

public class OverlayViewModel : ViewModel<OverlayViewModel>, IProvider
{
    public ReactiveCommand<BeginDragEvent> OnBeginDrag { get; private set; } = new ReactiveCommand<BeginDragEvent>();
    public ReactiveCommand<OnDragEvent> OnDrag { get; private set; } = new ReactiveCommand<OnDragEvent>();
    public ReactiveCommand<PointerDownEvent> OnEndDrag { get; private set; } = new ReactiveCommand<PointerDownEvent>();

}
