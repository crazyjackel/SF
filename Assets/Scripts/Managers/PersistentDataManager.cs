using Bewildered;
using RedMoon.Injector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentDataManager : BaseManager
{
    private Board _selectBoard;
    public Board selectedBoard
    {
        get
        {
            return _selectBoard;
        }
        set
        {
            _selectBoard = value;
        }
    }

    [SerializeField]
    UDictionary<string, Board> levels = new UDictionary<string, Board>();
    public UDictionary<string, Board> Levels => levels;

    private void Awake()
    {
        if (DepInjector.GetProvider<PersistentDataManager>() != null) Destroy(this.gameObject);
        selectedBoard = levels.FirstOrDefault().Value;
        DontDestroyOnLoad(this.gameObject);
    }
}
