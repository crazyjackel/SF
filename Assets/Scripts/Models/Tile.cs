using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UniRx;

[Serializable]
public class Tile
{
    private TileSeries Row { get; set; }
    private TileSeries Column { get; set; }

    public bool IsRowMovable => Row != null;
    public bool IsColumnMovable => Column != null;
    public bool IsWinTile { get; private set; }
    public IReadOnlyReactiveProperty<bool> IsCorrectColor { get; private set; }
    public IReactiveProperty<Color> TileColor { get; private set; }

    public Tile(TileSeries Row, TileSeries Column, Color InitialColor, bool isWinTile = false)
    {
        this.Row = Row;
        this.Column = Column;
        this.IsWinTile = isWinTile;
        this.TileColor = new ReactiveProperty<Color>(InitialColor);
        this.IsCorrectColor = TileColor.DistinctUntilChanged().Select(x => !IsWinTile || (x == InitialColor)).ToReactiveProperty();
    }
}
