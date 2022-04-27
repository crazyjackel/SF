using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySongPlayers : MonoBehaviour
{
    //mark the current scene in the inspector
    public bool jungle;
    public bool desert;
    public bool swamp;
    public bool ice;

    public GameObject jungleSongPlayer;
    public GameObject desertSongPlayer;
    public GameObject swampSongPlayer;
    public GameObject iceSongPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        //temporary fix to delete other scene's audio players
        if(!jungle && GameObject.Find("JungleSongPlayer(Clone)") != null)
        {
            Destroy(GameObject.Find("JungleSongPlayer(Clone)"));
        }        

        if(!desert && GameObject.Find("DesertSongPlayer(Clone)") != null)
        {
            Destroy(GameObject.Find("DesertSongPlayer(Clone)"));
        }

        if (!swamp && GameObject.Find("SwampSongPlayer(Clone)") != null)
        {
            Destroy(GameObject.Find("SwampSongPlayer(Clone)"));
        }

        if (!ice && GameObject.Find("IceSongPlayer(Clone)") != null)
        {
            Destroy(GameObject.Find("IceSongPlayer(Clone)"));
        }
    }

    private void Start()
    {
        //spawn in the correct song player if one doesnt already exist
        if(jungle && GameObject.Find("JungleSongPlayer(Clone)") == null)
        {
            Instantiate(jungleSongPlayer);
        }

        if (desert && GameObject.Find("DesertSongPlayer(Clone)") == null)
        {
            Instantiate(desertSongPlayer);
        }

        if (swamp && GameObject.Find("SwampSongPlayer(Clone)") == null)
        {
            Instantiate(swampSongPlayer);
        }

        if (ice && GameObject.Find("IceSongPlayer(Clone)") == null)
        {
            Instantiate(iceSongPlayer);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
