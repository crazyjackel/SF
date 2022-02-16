using RedMoon.Injector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

/*
public class ToolUsageManager : BaseManager
{
    public GameObject prefab;

    private OverlayViewModel overlayViewModel;
    private CompositeDisposable overlayViewModelDisposable;

    private void OnEndDrag(EndDragEvent ev)
    {
        if (ev == null || ev.element == null) return;

        //Use Position to get the closest nearby collider that has Tool Interactable
        var position = ev.position.ToScreenPoint();
        var cols = Physics2D.OverlapCircleAll(position, 3);
        var col = cols.OrderBy(x => Vector3.Distance(x.transform.position, position)).FirstOrDefault(x => x.gameObject.TryGetComponent<ToolInteractable>(out _));

        if (col == null) return;

        //Allocate Event
        if (col.TryGetComponent(out ToolInteractable tool))
        {
            var name = ev.element.name;
            switch (name)
            {
                case "Machette":
                    tool.Interact(Tooltype.Machette);
                    break;
                case "Hammer":
                    tool.Interact(Tooltype.Hammer);
                    break;
                case "Cutter":
                    tool.Interact(Tooltype.Cutter);
                    break;
                default:
                    tool.Interact(Tooltype.Any);
                    break;
            }


        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        overlayViewModelDisposable.Dispose();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        overlayViewModelDisposable.Dispose();
    }
    public override void NewProviderAvailable(IProvider newProvider)
    {
        if (DepInjector.MapProvider(newProvider, ref overlayViewModel))
        {
            overlayViewModelDisposable = new CompositeDisposable();
            overlayViewModel.OnEndDrag.Subscribe(ev => OnEndDrag(ev)).AddTo(overlayViewModelDisposable);
        }
    }
    public override void ProviderRemoved(IProvider removeProvider)
    {
        if (DepInjector.UnmapProvider(removeProvider, ref overlayViewModel))
        {
            overlayViewModelDisposable.Dispose();
        }
    }
}
*/