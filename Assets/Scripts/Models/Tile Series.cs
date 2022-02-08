using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

public class TileSeries
{
    public const int tileSize = 150;

    public List<Tile> Tiles { get; private set; }

    public IReadOnlyReactiveProperty<Texture2D> Texture2D { get; private set; }
    public IReadOnlyReactiveProperty<float> AdjustedOffset { get; private set; }
    public IReactiveProperty<float> Offset { get; private set; }
    public bool isRow { get; private set; }

    private bool doMove;

    public TileSeries(List<Tile> tiles, bool isRow = false)
    {
        this.Tiles = tiles;
        this.isRow = isRow;

        //Operation Order
        //Grab Tile Colors
        //Combine them all into an array of colors
        //Only Continue the Sequence if doMove is true
        //Generate a New Texture2D
        //Make this a ReactiveProperty
        doMove = true;
        this.Texture2D = this.Tiles
            .Select(x => x.TileColor)
            .CombineLatest()
            .Where((x, i) => doMove)
            .Select(x => MakeTexture2d(x))
            .ToReactiveProperty();

        this.Offset = new ReactiveProperty<float>(0);

        this.AdjustedOffset = Offset
            .Select(x => Mathf.Clamp(x, 0, tileSize * Tiles.Count * 2))
            .ToReactiveProperty();

        SetupTiles();
    }


    public void Move(int positions)
    {
        doMove = false;
        Color[] buffer = new Color[Tiles.Count];
        for (int i = 0; i < Tiles.Count; i++)
        {
            var Tile = Tiles[mod(i + positions, Tiles.Count)];
            buffer[i] = Tile.TileColor.Value;
        }


        for (int j = 0; j < Tiles.Count; j++)
        {
            if (j == Tiles.Count - 1) doMove = true;
            var Tile = Tiles[j];
            Tile.TileColor.Value = buffer[j];
        }
    }
    private int mod(int x, int mod)
    {
        int r = x % mod;
        return (r < 0) ? r + mod : r;
    }
    private void SetupTiles()
    {
        for (int i = 0; i < Tiles.Count; i++)
        {
            var tile = Tiles[i];
            tile.AddRow(this, i);
        }
    }
    public Texture2D MakeTexture2d(IList<Color> colors)
    {
        if (isRow)
        {
            Texture2D returnTexture = new Texture2D(colors.Count * 3, 1, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Point
            };

            var data = returnTexture.GetRawTextureData<Color32>();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    data[colors.Count * j + i] = colors[i];
                }
            }

            returnTexture.Apply();
            TextureScaler.scale(returnTexture, tileSize * colors.Count * 3, tileSize, FilterMode.Point);

            return returnTexture;
        }
        else
        {
            Texture2D returnTexture = new Texture2D(1, colors.Count * 3, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Point
            };

            var data = returnTexture.GetRawTextureData<Color32>();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    data[colors.Count * j + i] = colors[i];
                }
            }

            returnTexture.Apply();
            TextureScaler.scale(returnTexture, tileSize, tileSize * colors.Count * 3, FilterMode.Point);

            return returnTexture;
        }
    }
}