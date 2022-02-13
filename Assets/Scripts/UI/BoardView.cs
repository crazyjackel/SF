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

        /*
        //Take Care of Rows
        Dictionary<int, List<HashSet<Tile>>> rowMerger = new Dictionary<int, List<HashSet<Tile>>>();
        foreach(var row in board.Rows)
        {
            //Create Tiles from Row Data
            HashSet<Tile> series_Tiles = new HashSet<Tile>();
            for(int i = 0; i < row.colors.Count; i++)
            {
                //Things to Update in Futre:
                //1. For Rows, tiles array should be set only once, if row is attempting to override skip row being added.
                //2. Make sure i doesn't fall outside bounds
                Tile tile = new Tile(row.colors[i]);
                series_Tiles.Add(tile);
                tiles[row.Offset.x + i, row.Offset.y] = tile;
            }
            //Make Sure ID Key Exists
            if (!rowMerger.ContainsKey(row.ID)) rowMerger.Add(row.ID, new List<HashSet<Tile>>());
            rowMerger[row.ID].Add(series_Tiles);
        }

        List<TileSeries> rows = new List<TileSeries>();
        //Generate Tile Series
        foreach (var key in rowMerger.Keys)
        {
            var list = rowMerger[key].SelectMany(d => d).ToList();
            rows.Add(new TileSeries(list, true));
        }*/

        List<TileSeries> Rows = GenerateTileSeries(tiles, board.Rows, true);
        List<TileSeries> Columns = GenerateTileSeries(tiles, board.Cols, false);

        //bring it all together.
        for(int x = 0; x < tiles.GetLength(0); x++)
        {
            for(int y = 0; y < tiles.GetLength(1); y++)
            {
                var slot = slots[x, y];
                var tile = tiles[x, y];
                var background = slot.Q("SlotScroller");
                var border = slot.Q("Border");
                var up = slot.Q("Up");
                var down = slot.Q("Down");
                var left = slot.Q("Left");
                var right = slot.Q("Right");

                if(tile == null)
                {
                    background.RemoveFromHierarchy();
                    up.RemoveFromHierarchy();
                    down.RemoveFromHierarchy();
                    border.RemoveFromHierarchy();
                    left.RemoveFromHierarchy();
                    right.RemoveFromHierarchy();
                    continue;
                }

                if (tile.Column != null)
                {
                    up.BindCallback<ClickEvent>(x => tile.Column.MoveTiles(1)).AddTo(disposable);
                    down.BindCallback<ClickEvent>(x => tile.Column.MoveTiles(-1)).AddTo(disposable);
                }
                else
                {
                    up.RemoveFromHierarchy();
                    down.RemoveFromHierarchy();
                }

                if (tile.Row != null)
                {
                    right.BindCallback<ClickEvent>(x => tile.Row.MoveTiles(1)).AddTo(disposable);
                    left.BindCallback<ClickEvent>(x => tile.Row.MoveTiles(-1)).AddTo(disposable);
                }
                else
                {
                    right.RemoveFromHierarchy();
                    left.RemoveFromHierarchy();
                }


                background.AddManipulator(new TextureDragger(tile.Row, tile.Column));
                BindVisualElementToBackground(tile, background, disposable);

            }
        }

    }
    
    public List<TileSeries> GenerateTileSeries(Tile[,] tiles, List<TileSeriesData> data, bool isRow = true)
    {
        //Take Care of Rows
        Dictionary<int, List<HashSet<Tile>>> merger = new Dictionary<int, List<HashSet<Tile>>>();
        foreach (var ele in data)
        {
            //Create Tiles from Row Data
            HashSet<Tile> series_Tiles = new HashSet<Tile>();
            for (int i = 0; i < ele.colors.Count; i++)
            {
                int OffsetX = ele.Offset.x;
                int OffsetY = ele.Offset.y;
                if (isRow) OffsetX += i;
                else OffsetY += i;

                if (OffsetX >= tiles.GetLength(0) || OffsetY >= tiles.GetLength(1)) continue;

                //Things to Update in Futre:
                //1. For Rows, tiles array should be set only once, if row is attempting to override skip row being added.
                //2. Make sure i doesn't fall outside bounds
                Tile tile = tiles[OffsetX, OffsetY] ?? new Tile(ele.colors[i].GetColor());
                series_Tiles.Add(tile);
                tiles[OffsetX, OffsetY] = tile;
            }
            //Make Sure ID Key Exists
            if (!merger.ContainsKey(ele.ID)) merger.Add(ele.ID, new List<HashSet<Tile>>());
            merger[ele.ID].Add(series_Tiles);
        }

        List<TileSeries> series = new List<TileSeries>();
        //Generate Tile Series
        foreach (var key in merger.Keys)
        {
            var list = merger[key].SelectMany(d => d).ToList();
            series.Add(new TileSeries(list, isRow));
        }
        return series;
    }

    public void BindVisualElementToBackground(Tile tile, VisualElement element, CompositeDisposable disposables)
    {
        tile.TileSprite.Subscribe(spr =>
        {
            element.style.minWidth = spr.texture.width;
            element.style.minHeight = spr.texture.height;
            element.style.backgroundImage = new StyleBackground(Background.FromTexture2D(spr.texture));
        }).AddTo(disposables);

        tile.TileOffset.Subscribe(x =>
        {
            element.style.left = -x.x;
            element.style.top = -x.y;
        });
    }
}
