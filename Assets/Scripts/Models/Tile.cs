using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UniRx;

public class Tile
{
    public bool IsWinTile { get; private set; }
    public IReadOnlyReactiveProperty<bool> IsCorrectColor { get; private set; }
    public ReactiveProperty<Color> TileColor { get; set; }

    public IReadOnlyReactiveProperty<Sprite> TileSprite { get; private set; }
    public IReadOnlyReactiveProperty<Vector2> TileOffset { get; private set; }

    private IReadOnlyReactiveProperty<Sprite> RowSprite { get; set; }
    private IReadOnlyReactiveProperty<float> RowOffset { get; set; }
    private IReadOnlyReactiveProperty<Sprite> ColSprite { get; set; }
    private IReadOnlyReactiveProperty<float> ColOffset { get; set; }

    public TileSeries Row { get; private set; }
    public TileSeries Column { get; private set; }
    public int RowPosition { get; private set; }
    public int ColPosition { get; private set; }

    public Tile(Color InitialColor, bool isWinTile = false)
    {
        this.IsWinTile = isWinTile;
        this.TileColor = new ReactiveProperty<Color>(InitialColor);
        this.IsCorrectColor = TileColor.DistinctUntilChanged().Select(x => !IsWinTile || (x == InitialColor)).ToReactiveProperty();

        this.TileColor.Subscribe(x =>
        {
            int y = 10;
        });
    }
    public void AddColumn(TileSeries Column, int position)
    {
        this.Column = Column;
        this.ColPosition = position;
        this.ColSprite = Column.Texture2D.Select(x => Sprite.Create(x, new Rect(0.0f, 0.0f, x.width, x.height), new Vector2(0.5f, 0.5f), 150)).ToReactiveProperty();
        this.ColOffset = Column.AdjustedOffset.Select(x => x + 125 * ColPosition).ToReactiveProperty();
        DoTile();
    }
    public void AddRow(TileSeries Row, int position)
    {
        this.Row = Row;
        this.RowPosition = position;
        this.RowSprite = Row.Texture2D.Select(x => Sprite.Create(x, new Rect(0.0f, 0.0f, x.width, x.height), new Vector2(0.5f, 0.5f), 150)).ToReactiveProperty();
        this.RowOffset = Row.AdjustedOffset.Select(x => x + 125 * RowPosition).ToReactiveProperty();
        DoTile();
    }

    private void DoTile()
    {
        this.TileSprite =
            (TileSprite == null)
            ? RowSprite ?? ColSprite
            : this.Row.Offset
                .CombineLatest(this.Column.Offset, (x, y) => y == 0.0f)
                .CombineLatest(this.RowSprite, (x,y) => x)
                .CombineLatest(this.ColSprite, (x,y) => x)
                .Select(x => x ? RowSprite.Value : ColSprite.Value)
                .ToReactiveProperty();

        this.TileOffset =
            (TileOffset == null)
            ? (RowOffset != null)
            ? RowOffset
                .Select(x => new Vector2(x, 0))
                .ToReactiveProperty()
            : ColOffset
                .Select(x => new Vector2(0, x))
                .ToReactiveProperty()
            : this.Row.Offset
                .CombineLatest(this.Column.Offset, (x, y) => y == 0.0f)
                .Select(x => x ? new Vector2(RowOffset.Value, 0) : new Vector2(0, ColOffset.Value))
                .ToReactiveProperty();
    }
}
