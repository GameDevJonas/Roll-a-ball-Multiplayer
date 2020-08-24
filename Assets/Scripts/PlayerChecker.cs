using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    public static bool playersReady;

    public static List<GameObject> playersConnected = new List<GameObject>();

    void Start()
    {
        playersReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayers();
    }

    void CheckForPlayers()
    {
        if (!playersReady)
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
            else if (playersConnected[0] != playersConnected[1])
            {
                playersReady = true;
            }
        }
    }
}
