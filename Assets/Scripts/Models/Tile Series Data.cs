using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSeriesData 
{
    [SerializeField]
    public List<TileElement> colors;
    [SerializeField]
    public Vector2Int Offset;
    [SerializeField]
    public uint ID;
}

[Serializable]
public class TileElement
{
    [SerializeField]
    public SmartColor color;
    [SerializeField]
    public bool isWinTile;
}