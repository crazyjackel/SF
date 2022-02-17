using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class BoardView : View<BoardViewModel>
{
    [SerializeField]
    VisualTreeAsset m_ItemColumnTemplate;
    [SerializeField]
    VisualTreeAsset m_ItemSlotTemplate;

    public override void OnActivation(BoardViewModel viewModel, CompositeDisposable disposable)
    {
        CompositeDisposable gameplayDisposable = new CompositeDisposable();

        VisualElement element = Root.Q("Board");

        VisualElement overlay = Root.Q("Overlay");
        overlay.style.display = DisplayStyle.None;

        Button button = overlay.Q<Button>("NextButton");
        button.BindClick(viewModel.LoadNextLevelCommand).AddTo(disposable);


        var slots = viewModel.LoadSlots(element, m_ItemColumnTemplate, m_ItemSlotTemplate);

        for (int x = 0; x < slots.GetLength(0); x++)
        {
            for (int y = 0; y < slots.GetLength(1); y++)
            {
                var slot = slots[x, y];
                var tile = viewModel.GetTile(x, y);
                if (tile == null)
                {
                    var container = slot.Q("Container");
                    container?.RemoveFromHierarchy();
                    continue;
                }

                var background = slot.Q("SlotScroller");
                var up = slot.Q<Button>("Up");
                var down = slot.Q<Button>("Down");
                var left = slot.Q<Button>("Left");
                var right = slot.Q<Button>("Right");
                var border = slot.Q("Border");

                Debug.Log(tile.InitialColor.GetColor());

                if (tile.IsWinTile && !tile.InitialColor.Equals(SmartColor.Default))
                {
                    if (border != null)
                    {
                        border.style.SetBorderColor(tile.InitialColor.GetColor());
                    };

                    var elements = new VisualElement[] { up, down, left, right };
                    elements.Where(x => x != null).ForEach(x => x.style.unityBackgroundImageTintColor = tile.InitialColor.GetColor());
                }

                viewModel.BindButtonToAxis(left, right, tile.Row).AddTo(gameplayDisposable);
                viewModel.BindButtonToAxis(up, down, tile.Column).AddTo(gameplayDisposable);
                viewModel.BindBackgroundToTile(background, tile).AddTo(gameplayDisposable);
                viewModel.BindBorderToTileSeries(background, border, tile).AddTo(gameplayDisposable);
            }
        }

        gameplayDisposable.AddTo(disposable);

        viewModel.IsInWinState.Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(x =>
        {
            if (x)
            {
                gameplayDisposable.Dispose();
                overlay.style.display = DisplayStyle.Flex;
            }


        }).AddTo(disposable);
    }
}
