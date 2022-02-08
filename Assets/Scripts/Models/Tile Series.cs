using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

public class TileSeries
{
    const int tileSize = 150;

    public List<Tile> Tiles { get; private set; }

    public IReadOnlyReactiveProperty<Texture2D> Texture2D { get; private set; }
    public IReadOnlyReactiveProperty<Vector2> ClampedOffset { get; private set; }
    public IReactiveProperty<Vector2> Offset { get; private set; }
    public bool isRow { get; private set; }

    public TileSeries(List<Tile> tiles, bool isRow = false)
    {
        this.Tiles = tiles;
        this.isRow = isRow;

        this.Texture2D = this.Tiles.Select(x => x.TileColor).CombineLatest().Select(x => MakeTexture2d(x)).ToReactiveProperty();
        Offset = new ReactiveProperty<Vector2>(Vector2.zero);
        this.ClampedOffset = Offset.Select(x => ClampVec(x)).ToReactiveProperty();

        SetupTiles();
    }

    private Vector2 ClampVec(Vector2 clamp)
    {
        float clampX = Mathf.Clamp(clamp.x, -tileSize * Tiles.Count, tileSize * Tiles.Count);
        float clampY = Mathf.Clamp(clamp.y, -tileSize * Tiles.Count, tileSize * Tiles.Count);
        return isRow ? new Vector2(clampX, clamp.x) : new Vector2(clamp.y, clampY);
    }

    private void SetupTiles()
    {
        for(int i = 0; i < Tiles.Count; i++)
        {
            var tile = Tiles[i];
            tile.AddRow(this, i);
        }
    }

    public Texture2D MakeTexture2d(IList<Color> colors)
    {
        if (isRow)
        {
            Texture2D returnTexture = new Texture2D(colors.Count, 1, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Point
            };

            var data = returnTexture.GetRawTextureData<Color32>();
            for (int i = 0; i < colors.Count; i++)
            {
                data[i] = colors[i];
            }

            returnTexture.Apply();
            TextureScaler.scale(returnTexture, tileSize * colors.Count, tileSize, FilterMode.Point);

            return returnTexture;
        }
        else
        {
            Texture2D returnTexture = new Texture2D(1, colors.Count, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Point
            };

            var data = returnTexture.GetRawTextureData<Color32>();
            for (int i = 0; i < colors.Count; i++)
            {
                data[i] = colors[i];
            }

            returnTexture.Apply();
            TextureScaler.scale(returnTexture, tileSize, tileSize * colors.Count, FilterMode.Point);

            return returnTexture;
        }
    }
}