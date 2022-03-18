using Bewildered;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save", menuName = "SF/Save")]
public class SaveFile : ScriptableObject
{
    [SerializeField]
    private bool resetOnPlay;
    [SerializeField]
    private GameConstants constants;

    //Private Fields are Variables that you want to control access to, but also save between sessions, unless reset on play is true.
    [SerializeField]
    private UHashSet<string> m_levelsCompleted;

    //Public fields are variables that you want to be saved and modified between game sessions.

    //Properties are Variables that you only want to remember while playing between scenes. Should not be saved between sessions.
    public Vector3? m_cameraPos { get; set; }
    public float? m_cameraScroll { get; set; }

    public int LevelCompleted => m_levelsCompleted.Count;
    public void CompleteLevel(string levelName)
    {
        if (constants.Levels.ContainsKey(levelName)) m_levelsCompleted.Add(levelName);
    }
    private void OnEnable()
    {
        if (resetOnPlay)
        {
            m_levelsCompleted.Clear();
        }
    }
}
