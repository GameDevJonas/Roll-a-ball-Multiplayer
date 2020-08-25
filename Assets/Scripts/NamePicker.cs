using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class NamePicker : NetworkBehaviour
{
    [SyncVar]
    public string myName;

    public GameObject whatToDisable;

    bool loadedSceneTwo;
    void Start()
    {
        loadedSceneTwo = false;
        //if (!GetComponentInParent<PlayerController>().isLocalPlayer)
        //{
        //    gameObject.SetActive(false);
        //}
        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!loadedSceneTwo && SceneManager.GetActiveScene().buildIndex == 1)
        //{
        //    //Invoke("SyncNames", 1f);
        //    PlayerChecker.playerNames.Add(myName);
        //    Debug.Log("Added " + myName + " to list");
        //    loadedSceneTwo = true;
        //}
    }

    [Command]
    public void SyncNames()
    {
        PlayerChecker.playerNames.Add(myName);
    }

    
    public void OnDonePress()
    {
        myName = GetComponentInChildren<TMP_InputField>().text;
        Debug.Log("Added " + myName + " to list");
        PlayerChecker.playerNames.Add(myName);
        PlayerController player = GetComponentInParent<PlayerController>();
        player.ChooseName();
        //whatToDisable.SetActive(false);
    }
}
