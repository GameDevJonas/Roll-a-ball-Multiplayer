using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NamePicker : MonoBehaviour
{
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
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!loadedSceneTwo && SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerChecker.playerNames.Add(myName);
            loadedSceneTwo = true;
        }
    }

    public void OnDonePress()
    {
        myName = GetComponentInChildren<TMP_InputField>().text;
        whatToDisable.SetActive(false);
    }
}
