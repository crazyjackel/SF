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
    public IReactiveProperty<Color> TileColor { get; set; }
    public IReadOnlyReactiveProperty<Sprite> TileSprite { get; private set; }
    public IReadOnlyReactiveProperty<float> RowOffset { get; private set; }
    public TileSeries Row { get; private set; }
    public TileSeries Column { get; private set; }
    public int RowPosition { get; private set; }

    public Tile(Color InitialColor, bool isWinTile = false)
    {
        this.IsWinTile = isWinTile;
        this.TileColor = new ReactiveProperty<Color>(InitialColor);
        this.IsCorrectColor = TileColor.DistinctUntilChanged().Select(x => !IsWinTile || (x == InitialColor)).ToReactiveProperty();
        
    }
    public void AddColumn(TileSeries Column, int position)
    {

    }
    public void AddRow(TileSeries Row, int position)
    {
        this.Row = Row;
        this.RowPosition = position;
        this.TileSprite = Row.Texture2D.Select(x => Sprite.Create(x, new Rect(0.0f, 0.0f, x.width, x.height), new Vector2(0.5f, 0.5f), 150)).ToReactiveProperty();
        this.RowOffset = Row.AdjustedOffset.Select(x => x + 150 * RowPosition).ToReactiveProperty();
    }
}
