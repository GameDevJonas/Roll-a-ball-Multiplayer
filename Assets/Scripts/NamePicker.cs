using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class NamePicker : NetworkBehaviour
{
    //[SyncVar]
    public string myName;

    public GameObject whatToDisable;

    GameObject doneButton;

    bool loadedSceneTwo;
    bool isPlayerOne;
    void Start()
    {
        doneButton = GetComponentInChildren<Button>().gameObject;
        loadedSceneTwo = false;
        if (FindObjectOfType<NetworkManager>().numPlayers <= 1)
        {
            isPlayerOne = true;
            doneButton.SetActive(false);
        }
        if (FindObjectOfType<NetworkManager>().numPlayers == 2)
        {
            isPlayerOne = false;
            doneButton.SetActive(false);
        }
        //if (!GetComponentInParent<PlayerController>().isLocalPlayer)
        //{
        //    gameObject.SetActive(false);
        //}
        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(FindObjectOfType<NetworkManager>().numPlayers);
        //if (!loadedSceneTwo && SceneManager.GetActiveScene().buildIndex == 1)
        //{
        //    //Invoke("SyncNames", 1f);
        //    PlayerChecker.playerNames.Add(myName);
        //    Debug.Log("Added " + myName + " to list");
        //    loadedSceneTwo = true;
        //}
        if(isPlayerOne && FindObjectOfType<NetworkManager>().numPlayers > 1)
        {
            doneButton.SetActive(true);
        }
    }

    //[Command]
    //public void SyncNames()
    //{
    //    //PlayerChecker.playerNames.Add(myName);
    //}


    public void OnDonePress()
    {
        //Debug.Log("Name added: " + GetComponentInChildren<TMP_InputField>().text);
        //if (FindObjectOfType<NetworkManager>().numPlayers == 2)
        //{
        //    ServerPress();
        //    ClientSync();
        //    //ClientPress();
        //}
        //else
        //{
        //    //ServerPress();
        //    //ClientPress();
        //}
        ////if (isClient)
        ////{
        ////    ClientPress();
        ////}
        ////else
        ////{
        ////    ServerPress();
        ////}
    }

    //public void ServerPress()
    //{
    //    //myName = GetComponentInChildren<TMP_InputField>().text;
    //    //FindObjectOfType<PlayerChecker>().AddPlayerServer(myName);
    //    PlayerController player = GetComponentInParent<PlayerController>();
    //    player.ChooseName();
    //    //whatToDisable.SetActive(false);
    //}

    //[Command]
    //public void ClientSync()
    //{
    //    myName = GetComponentInChildren<TMP_InputField>().text;
    //    Debug.Log("Added " + myName + " to list");
    //    FindObjectOfType<PlayerChecker>().AddPlayerName(myName);
    //    //PlayerController player = GetComponentInParent<PlayerController>();
    //    //player.ChooseName();
    //    //whatToDisable.SetActive(false);
    //}
}
