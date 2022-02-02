using RedMoon.Injector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSpawnManager : BaseManager
{
    public GameObject prefab;

    private OverlayViewModel overlayViewModel;
    private CompositeDisposable overlayViewModelDisposable;
    public override void NewProviderAvailable(IProvider newProvider)
    {
        if(DepInjector.MapProvider(newProvider, ref overlayViewModel))
        {
            overlayViewModelDisposable = new CompositeDisposable();
            overlayViewModel.OnEndDrag.Subscribe(ev => OnEndDrag(ev)).AddTo(overlayViewModelDisposable);
        }
    }

    private void OnEndDrag(EndDragEvent ev)
    {
        var gameObject = Instantiate(prefab);
        gameObject.transform.position = ev.position.ToScreenPoint();
    }

    public override void ProviderRemoved(IProvider removeProvider)
    {
        if(DepInjector.UnmapProvider(removeProvider, ref overlayViewModel))
        {
            overlayViewModelDisposable.Dispose();
        }
    }
}
