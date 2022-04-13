using Bewildered;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Constants", menuName = "SF/Constants")]
public partial class GameConstants : ScriptableObject
{
    [Header("Level Settings")]
    [SerializeField]
    private string m_playLevel;
    public string PlayLevel => m_playLevel;

    [SerializeField]
    private string m_levelSelectLevel;
    public string LevelSelect => m_levelSelectLevel;

    [SerializeField]
    LevelDict levels = new LevelDict();
    public LevelDict Levels => levels;
}

[Serializable]
public class LevelDict : UDictionary<string, Board>
{

}
