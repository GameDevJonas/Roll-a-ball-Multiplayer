using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
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

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameIsOver = false;
            playersReady = false;
        }
    }
}
