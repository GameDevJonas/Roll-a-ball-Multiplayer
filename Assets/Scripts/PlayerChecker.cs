using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChecker : NetworkBehaviour
{
    public static bool playersReady;

    public static List<GameObject> playersConnected = new List<GameObject>();

    public List<GameObject> playersConCheck = new List<GameObject>();

    void Start()
    {
        playersReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayers();
        //playerCheck = playerNames;
        playersConCheck = playersConnected;
        Debug.Log(playersConnected.Count);
        //Debug.Log(playerNames.Count);
    }

    //[Command]
    //public void AddPlayerName(string n)
    //{
    //    playerNames.Add(n);
    //}

    //public void AddPlayerServer(string n)
    //{
    //    playerNames.Add(n);
    //}

    void CheckForPlayers()
    {
        if (!playersReady && SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (playersConnected.Count < 2)
            {
                playersReady = false;
            }
            /*else if (playersConnected[0] == playersConnected[1])
            {
                playersReady = false;
                playersConnected.RemoveAt(1);
                playersConnected.Add(GameObject.FindGameObjectWithTag("Player"));
            }*/
            else //if(!playersConnected[0].GetComponent<PlayerController>().pickingName && !playersConnected[1].GetComponent<PlayerController>().pickingName)
            {
                //playerNames.Add(playersConnected[1].GetComponent<PlayerController>().myName);
                Invoke("PlayersReady", 1f);
            }
        }
    }

    void PlayersReady()
    {
        playersReady = true;
    }
}
