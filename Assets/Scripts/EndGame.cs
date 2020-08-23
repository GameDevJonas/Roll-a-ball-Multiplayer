using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EndGame : NetworkBehaviour
{
    public static bool gameIsOver = false;
    public static bool playersReady = false;

    public void GameOver()
    {
        gameIsOver = true;
    }

    private void Update()
    {
        if (GetComponent<NetworkManager>().numPlayers >= 2)
        {
            playersReady = true;
        }
        else
        {
            playersReady = false;
        }
    }
}
