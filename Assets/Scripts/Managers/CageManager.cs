using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CageManager : BaseManager
{
    [SerializeField]
    private SaveFile m_save;

    [SerializeField]
    private Sprite CageCompleteSprite;

    [SerializeField]
    private GameObject spawnPrefab;

    public List<Cage> Cages = new List<Cage>();

    protected override void OnEnable()
    {
        base.OnEnable();
        Cages = this.GetComponentsInChildren<Cage>().ToList();

        foreach(Cage cage in Cages)
        {
            var levelName = cage.Clickable.LevelName;
            if (m_save.IsLevelComplete(levelName))
            {
                cage.SpriteRenderer.sprite = CageCompleteSprite;
                if (spawnPrefab)
                {
                    //randomize the animal's spawn point around the cage
                    GameObject animal = Instantiate(spawnPrefab, cage.gameObject.transform);
                    animal.transform.position = new Vector3(
                        Random.Range(cage.gameObject.transform.position.x - 1f, cage.gameObject.transform.position.x + 1f),
                        Random.Range(cage.gameObject.transform.position.y - 1f, cage.gameObject.transform.position.y + 1f),
                        0f);
                }
            }
            else if (cage.PreviousLevelName != "" && !m_save.IsLevelComplete(cage.PreviousLevelName))
            {
                cage.gameObject.SetActive(false);
            }
        }
    }
}
