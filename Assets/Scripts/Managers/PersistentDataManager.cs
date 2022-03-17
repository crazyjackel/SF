using Bewildered;
using RedMoon.Injector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentDataManager : BaseManager
{
    [SerializeField]
    private GameConstants constants;

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

    private void Awake()
    {
        if (DepInjector.GetProvider<PersistentDataManager>() != null) Destroy(this.gameObject);
        selectedBoard = constants.Levels.FirstOrDefault().Value;
        DontDestroyOnLoad(this.gameObject);
    }
}
