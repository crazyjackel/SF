using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardView : View<BoardViewModel>
{
    [SerializeField]
    VisualTreeAsset m_ItemColumnTemplate;
    [SerializeField]
    VisualTreeAsset m_ItemSlotTemplate;

    public override void OnActivation(BoardViewModel viewModel, CompositeDisposable disposable)
    {
        VisualElement element = Root.Q("Board");
        LoadBoard(element, viewModel.board);
    }

    public void LoadBoard(VisualElement element, Board board)
    {
        for(uint i = 0; i < board.Width; i++)
        {
            VisualElement VisualElement = m_ItemColumnTemplate.CloneTree();
            element.Add(VisualElement);
            for(uint j = 0; j < board.Height; j++)
            {
                VisualElement column = VisualElement.Q("Column");
                VisualElement slot = m_ItemSlotTemplate.CloneTree();
                column.Add(slot);
            }
        }
    }
}
