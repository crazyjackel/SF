using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CageManager : BaseManager
{
    [SerializeField]
    private SaveFile file;

    [SerializeField]
    private Sprite CageCompleteSprite;

    public List<Cage> Cages = new List<Cage>();

    protected override void OnEnable()
    {
        base.OnEnable();
        Cages = this.GetComponentsInChildren<Cage>().ToList();

        foreach(Cage cage in Cages)
        {
            var levelName = cage.Clickable.LevelName;
            if (file.IsLevelComplete(levelName))
            {
                cage.SpriteRenderer.sprite = CageCompleteSprite;
            }
            else if (cage.PreviousLevelName != "" && !file.IsLevelComplete(cage.PreviousLevelName))
            {
                cage.gameObject.SetActive(false);
            }
        }
    }
}
