using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
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
        LoadBoard(element, viewModel.board, disposable);
    }

    public void LoadBoard(VisualElement element, Board board, CompositeDisposable disposable)
    {
        //Draw Board
        VisualElement[,] slots = new VisualElement[board.Width, board.Height];
        for(uint i = 0; i < board.Width; i++)
        {
            VisualElement VisualElement = m_ItemColumnTemplate.CloneTree();
            element.Add(VisualElement);
            for(uint j = 0; j < board.Height; j++)
            {
                VisualElement column = VisualElement.Q("Column");
                VisualElement slot = m_ItemSlotTemplate.CloneTree();
                column.Add(slot);
                slots[i, j] = slot;
            }
        }

        //Begin Filling out Tiles
        Tile[,] tiles = new Tile[board.Width, board.Height];

        //Take Care of Rows
        Dictionary<int, List<List<Tile>>> rowMerger = new Dictionary<int, List<List<Tile>>>();
        foreach(var row in board.Rows)
        {
            //Create Tiles from Row Data
            List<Tile> series_Tiles = new List<Tile>();
            for(int i = 0; i < row.colors.Count; i++)
            {
                //Things to Update in Futre:
                //1. For Rows, tiles array should be set only once, if row is attempting to override skip row being added.
                Tile tile = new Tile(row.colors[i]);
                series_Tiles.Add(tile);
                tiles[row.Offset.x + i, row.Offset.y] = tile;
            }
            //Make Sure ID Key Exists
            if (!rowMerger.ContainsKey(row.ID)) rowMerger.Add(row.ID, new List<List<Tile>>());
            rowMerger[row.ID].Add(series_Tiles);
        }

        List<TileSeries> rows = new List<TileSeries>();
        //Generate Tile Series
        foreach (var key in rowMerger.Keys)
        {
            var list = rowMerger[key].SelectMany(d => d).ToList();
            rows.Add(new TileSeries(list, true));
        }

        //bring it all together.
        for(int x = 0; x < tiles.GetLength(0); x++)
        {
            for(int y = 0; y < tiles.GetLength(1); y++)
            {
                var slot = slots[x, y];
                var tile = tiles[x, y];
                var background = slot.Q("SlotScroller");

                if(tile == null)
                {
                    slot.RemoveFromHierarchy();
                    continue;
                }

                background.AddManipulator(new TextureDragger(tile.Row));
                BindVisualElementToBackground(tile, background, disposable);

            }
        }

    }

    public void BindVisualElementToBackground(Tile tile, VisualElement element, CompositeDisposable disposables)
    {
        element.style.minWidth = tile.TileSprite.Value.texture.width;
        tile.TileSprite.Subscribe(x =>
        {
            element.style.backgroundImage = new StyleBackground(x);
        }).AddTo(disposables);
        tile.Offset.Subscribe(x =>
        {
            element.style.left = -x.x;
        });
    }
}
