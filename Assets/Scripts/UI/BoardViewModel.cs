using RedMoon.ReactiveKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Operators;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardViewModel : ViewModel<BoardViewModel>
{
    [SerializeField]
    private Board board;

    private Tile[,] tiles;
    private List<TileSeries> Rows;
    private List<TileSeries> Columns;

    public VisualElement[,] LoadSlots(VisualElement element, VisualTreeAsset m_ItemColumnTemplate, VisualTreeAsset m_ItemSlotTemplate)
    {
        VisualElement[,]  slots = new VisualElement[board.Width, board.Height];
        for (uint i = 0; i < board.Width; i++)
        {
            VisualElement VisualElement = m_ItemColumnTemplate.CloneTree();
            element.Add(VisualElement);
            for (uint j = 0; j < board.Height; j++)
            {
                VisualElement column = VisualElement.Q("Column");
                VisualElement slot = m_ItemSlotTemplate.CloneTree();
                column.Add(slot);
                slots[i, j] = slot;
            }
        }
        return slots;
    }
    public Tile GetTile(int x, int y)
    {
        try
        {
            return tiles[x, y];
        }
        catch (Exception)
        {
            return null;
        }
    }
    public List<TileSeries> GenerateTileSeries(Tile[,] tiles, List<TileSeriesData> data, bool isRow = true)
    {
        //Add More Comments Explaining what is going on.

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

                Tile tile = tiles[OffsetX, OffsetY] ?? new Tile(ele.colors[i]);
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

    public IDisposable BindButtonToAxis(Button positive, Button negative, TileSeries series)
    {
        CompositeDisposable disp = new CompositeDisposable();
        if (series != null)
        {
            positive?.BindCallback<ClickEvent>(x => series.MoveTiles(1)).AddTo(disp);
            negative?.BindCallback<ClickEvent>(x => series.MoveTiles(-1)).AddTo(disp);
        }
        else
        {
            positive?.RemoveFromHierarchy();
            negative?.RemoveFromHierarchy();
        }
        return disp;
    }
    public IDisposable BindVisualElementToBackground(VisualElement element, Tile tile)
    {
        CompositeDisposable disposables = new CompositeDisposable();
        if (tile == null || element == null) return disposables;

        element.AddManipulator(new TextureDragger(tile.Row, tile.Column));

        tile.TileSprite.Subscribe(spr =>
        {
            element.style.minWidth = spr.texture.width;
            element.style.minHeight = spr.texture.height;
            element.style.backgroundImage = new StyleBackground(spr);
        }).AddTo(disposables);

        tile.TileOffset.Subscribe(x =>
        {
            element.style.left = -x.x;
            element.style.top = -x.y;
        }).AddTo(disposables);

        return disposables;
    }

    public override void OnInitialization()
    {
        base.OnInitialization();
        LoadTiles();
    }

    private void LoadTiles()
    {
        //Begin Filling out Tiles
        tiles = new Tile[board.Width, board.Height];
        Rows = GenerateTileSeries(tiles, board.Rows, true);
        Columns = GenerateTileSeries(tiles, board.Cols, false);
    }
}
