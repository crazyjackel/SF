using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileSeriesData 
{
    [SerializeField]
    public List<SmartColor> colors;
    [SerializeField]
    public Vector2Int Offset;
    [SerializeField]
    public int ID;
}
