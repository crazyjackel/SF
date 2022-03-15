using RedMoon.Injector;
using RedMoon.ReactiveKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Operators;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoardViewModel : ViewModel<BoardViewModel>
{
    [SerializeField]
    private Board board;

    private Tile[,] tiles;
    private List<TileSeries> TileSeries;
    private List<TileSeries> Rows;
    private List<TileSeries> Columns;

    ReactiveProperty<PersistentDataManager> _pDataManager = new ReactiveProperty<PersistentDataManager>();

    public IReactiveCommand<ClickEvent> LoadNextLevelCommand { get; private set; }
    public IReadOnlyReactiveProperty<bool> IsInWinState { get; private set; }


    public void LoadBackground(VisualElement element)
    {
        if (board.Background == null) return;

        element.style.backgroundImage = board.Background;
    }
    public VisualElement[,] LoadSlots(VisualElement element, VisualTreeAsset m_ItemColumnTemplate, VisualTreeAsset m_ItemSlotTemplate)
    {
        VisualElement[,] slots = new VisualElement[board.Width, board.Height];
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
        Dictionary<uint, List<HashSet<Tile>>> merger = new Dictionary<uint, List<HashSet<Tile>>>();
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

                Tile tile = tiles[OffsetX, OffsetY] ?? new Tile(ele.colors[i].color, ele.colors[i].isWinTile);
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
            series.Add(new TileSeries(list, key, isRow));
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
    public IDisposable BindBackgroundToTile(VisualElement element, Tile tile)
    {
        CompositeDisposable disposables = new CompositeDisposable();
        if (tile == null || element == null) return disposables;

        element.AddManipulator(new TextureDragger(tile.Row, tile.Column));

        tile.TileSprite.Subscribe(spr =>
        {
            element.style.minWidth = spr.texture.width;
            element.style.minHeight = spr.texture.height;
            element.style.backgroundImage = new StyleBackground(spr.texture);
        }).AddTo(disposables);

        tile.TileOffset.Subscribe(x =>
        {
            element.style.left = -x.x;
            element.style.top = -x.y;
        }).AddTo(disposables);

        return disposables;
    }
    public IDisposable BindBorderToTileSeries(VisualElement element, VisualElement border, Tile tile)
    {
        CompositeDisposable disp = new CompositeDisposable();

        var row = tile.Row;
        var column = tile.Column;

        if (row != null)
        {
            element.BindCallback<MouseEnterEvent>(x => row.IsHover.Value = new HoverInfo(tile, true)).AddTo(disp);
            element.BindCallback<MouseLeaveEvent>(x => row.IsHover.Value = new HoverInfo(tile, false)).AddTo(disp);
        }

        if (column != null)
        {
            element.BindCallback<MouseEnterEvent>(x => column.IsHover.Value = new HoverInfo(tile, true)).AddTo(disp);
            element.BindCallback<MouseLeaveEvent>(x => column.IsHover.Value = new HoverInfo(tile, false)).AddTo(disp);
        }

        if (row != null && column != null)
        {
            row.IsHover
                .CombineLatest(column.IsHover, (x, y) => (x, y))
                .Subscribe((x) =>
                {
                    var rowInfo = x.x;
                    var colInfo = x.y;
                    if ((rowInfo.isHovered && tile == rowInfo.selectedTile) || (colInfo.isHovered && tile == colInfo.selectedTile)) return;

                    Color color =
                          (rowInfo.isHovered && colInfo.isHovered) ? new Color(1, 1, 0, 1)
                        : (rowInfo.isHovered) ? new Color(0.6f, 1, 0, 1)
                        : (colInfo.isHovered) ? new Color(1, 0.6f, 0, 1)
                        : new Color(0, 0, 0, 0);
                    border.style.backgroundColor = color;
                }).AddTo(disp);
        }
        else if (row != null)
        {
            row.IsHover.Subscribe(x =>
            {
                if (tile == x.selectedTile) return;
                Color color = x.isHovered ? new Color(0.6f, 1, 0, 1) : new Color(0, 0, 0, 0);
                border.style.backgroundColor = color;
            }).AddTo(disp);
        }
        else if (column != null)
        {
            column.IsHover.Subscribe(x =>
            {
                if (tile == x.selectedTile) return;
                Color color = x.isHovered ? new Color(1, 0.6f, 0, 1) : new Color(0, 0, 0, 0);
                border.style.backgroundColor = color;
            }).AddTo(disp);
        }

        return disp;
    }

    public override bool CanInitialize()
    {
        return _pDataManager.HasValue;
    }

    public override void OnInitialization()
    {
        base.OnInitialization();
        board = _pDataManager.Value.selectedBoard;
        LoadNextLevelCommand = new ReactiveCommand<ClickEvent>();
        LoadNextLevelCommand.Subscribe(x =>
        {
            Debug.Log("Loading Next Level...");
            SceneManager.LoadScene(_pDataManager.Value.LevelSelect, LoadSceneMode.Single);
        });
        LoadTiles();
    }

    private void RandomizeBoardState()
    {
        foreach (var move in board.Moves)
        {
            var list = (move.isRow) ? Rows : Columns;
            var series = list.FirstOrDefault(x => x.ID == move.id);
            if (series == null) continue;
            series.MoveTiles(move.moves);
        }

        for (int i = 0; i < board.NumberOfRandomMoves; i++) DoRandomMove();
    }

    private void DoRandomMove()
    {
        TileSeries randomTilesSeries = TileSeries.Random();
        randomTilesSeries.MoveTiles(UnityEngine.Random.Range(0, randomTilesSeries.Count));
    }

    private void LoadTiles()
    {
        //Begin Filling out Tiles
        tiles = new Tile[board.Width, board.Height];
        Rows = GenerateTileSeries(tiles, board.Rows, true);
        Columns = GenerateTileSeries(tiles, board.Cols, false);
        TileSeries = Rows.Concat(Columns).ToList();

        RandomizeBoardState();

        IsInWinState = tiles
            .ToList()
            .Where(x => x != null)
            .Select(x => x.IsCorrectColor)
            .CombineLatest()
            .Select(x => x.All(x => x))
            .ToReactiveProperty();

        while (IsInWinState.Value == true)
        {
            DoRandomMove();
        }
    }

    public override void NewProviderAvailable(IProvider newProvider)
    {
        DepInjector.MapProvider(newProvider, _pDataManager);
    }
}
