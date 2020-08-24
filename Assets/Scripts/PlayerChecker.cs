using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChecker : MonoBehaviour
{
    public static bool playersReady;

    public static List<GameObject> playersConnected = new List<GameObject>();

    public static List<string> playerNames = new List<string>();

    public List<GameObject> playersConCheck = new List<GameObject>();
    public List<string> playerCheck = new List<string>();

    void Start()
    {
        playersReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayers();
        playerCheck = playerNames;
        playersConCheck = playersConnected;
        Debug.Log(playersConnected.Count);
        Debug.Log(playerNames.Count);
    }

    public void AddPlayerName(string n)
    {
        playerNames.Add(n);
    }

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
            else
            {
                playerNames.Add(playersConnected[1].GetComponent<PlayerController>().myName);
                playersReady = true;
            }
        }
    }
}
