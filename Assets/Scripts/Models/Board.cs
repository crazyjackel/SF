using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Board", menuName ="SF/Board")]
public class Board : ScriptableObject
{
    [SerializeField]
    private uint width = 3;
    public uint Width => width;
    [SerializeField]
    private uint height = 3;
    public uint Height => height;

    [SerializeField]
    private List<TileSeriesData> rows = new List<TileSeriesData>();
    public List<TileSeriesData> Rows => rows;


    [SerializeField]
    private List<TileSeriesData> cols = new List<TileSeriesData>();
    public List<TileSeriesData> Cols => cols;
}
