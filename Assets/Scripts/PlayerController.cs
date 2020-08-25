using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    public float jumpForce;
    public float groundRayLength;
    public float timer;

    public bool onJumppad, onGoal;

    public LayerMask jumpPadLayer;
    public LayerMask goalLayer;

    public TextMeshProUGUI countText, winText, timerText;

    public GameObject jumpParticles;
    public GameObject otherStuffToSpawn;
    //public GameObject namePicker; //
    public GameObject nameView; //

    public int count;
    public int maxCount;

    public Material playerOneMat, playerTwoMat;

    float moveHorizontal;
    float moveVertical;

    bool hasWon;
    bool finishedCountDown;
    bool countingDown;
    //bool iAmLocal;
    bool playingParticles;

    //[SyncVar]
    //public bool pickingName;

    [SyncVar]
    public string playerName;
    public string playerOneName, playerTwoName;

    Rigidbody rb;

    public TMP_InputField usernameInput;

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(-1, 10, -20);
        Camera.main.transform.SetParent(null);
        Camera.main.GetComponent<CameraController>().FindPlayer(gameObject);
        //namePicker = Camera.main.transform.Find("MyName").gameObject;
        //namePicker.SetActive(true); //
        //namePicker.GetComponentInChildren<Button>().onClick.AddListener(PickedName);

    }

    void Start()
    {
        timer = 0;
        Instantiate(otherStuffToSpawn, transform.position, Quaternion.identity, transform);
        nameView = transform.Find("OtherNececcities(Clone)").transform.Find("NameView").gameObject;
        nameView.transform.parent = null;
        //namePicker.SetActive(false);
        //if (isLocalPlayer)
        //{
        //    namePicker.SetActive(true);
        //}
        //if (isLocalPlayer)
        //{
        //    ChooseName();
        //}
        //pickingName = true; //
        //iAmLocal = GetComponentInParent<ArenaSpawn>().isLocalPlayer;
        hasWon = false;
        finishedCountDown = false;
        countingDown = false;
        winText = transform.Find("OtherNececcities(Clone)").transform.Find("Canvas").transform.Find("WinText").GetComponent<TextMeshProUGUI>();
        countText = transform.Find("OtherNececcities(Clone)").transform.Find("Canvas").transform.Find("CountText").GetComponent<TextMeshProUGUI>();
        timerText = transform.Find("OtherNececcities(Clone)").transform.Find("Canvas").transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        usernameInput = transform.Find("OtherNececcities(Clone)").transform.Find("Canvas").transform.Find("UsernameInput").GetComponent<TMP_InputField>();
        jumpParticles = transform.Find("OtherNececcities(Clone)").transform.Find("JumpParticles").gameObject;
        jumpParticles.transform.SetParent(null);
        winText.gameObject.SetActive(false);
        countText.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        //maxCount = GameObject.FindGameObjectsWithTag("Pickup").Length;
        count = 0;
        SetCountText();
        jumpParticles.GetComponentInChildren<ParticleSystem>().Stop();
        PlayerChecker.playersConnected.Add(gameObject);
        if (FindObjectOfType<PlayerChecker>().playersConCheck.Count == 1)
        {
            GetComponent<MeshRenderer>().material = playerOneMat;
            usernameInput.text = playerOneName;
            playerName = playerOneName;
        }
        else
        {
            GetComponent<MeshRenderer>().material = playerTwoMat;
            usernameInput.text = playerTwoName;
            playerName = playerTwoName;
        }
        nameView.GetComponentInChildren<TextMeshProUGUI>().text = playerName;
    }

    [Command]
    public void NameChange(string newName)
    {
        playerName = newName;
    }

    //public void ChooseName()
    //{
    //    PushNameOnClient();
    //    PushNameOnServer();
    //    //if (!isServer)
    //    //{
    //    //    PushNameOnClient();
    //    //}
    //    //else
    //    //{
    //    //    PushNameOnServer();
    //    //}
    //    /*if (!hasAuthority)
    //    {
    //        PushNameOnClient();
    //    }
    //    else
    //    {
    //        PushNameOnServer();
    //    }*/
    //    namePicker.SetActive(false);
    //}

    //[Command]
    //public void PushNameOnClient()
    //{
    //    myName = namePicker.GetComponent<NamePicker>().myName;
    //    //nameView.GetComponentInChildren<TextMeshProUGUI>().text = myName;
    //    //gameObject.name = myName;
    //    pickingName = false;
    //    Debug.Log("Pushed " + myName + " from client", gameObject);
    //}

    //public void PushNameOnServer()
    //{
    //    //myName = namePicker.GetComponent<NamePicker>().myName;
    //    nameView.GetComponentInChildren<TextMeshProUGUI>().text = myName;
    //    gameObject.name = myName;
    //    pickingName = false;
    //    Debug.Log("Pushed " + myName + " from server", gameObject);
    //}

    private void Update()
    {
        if(playerName != usernameInput.text)
        {
            NameChange(usernameInput.text);
            nameView.GetComponentInChildren<TextMeshProUGUI>().text = playerName;
            if (isLocalPlayer)
            {
                NameChange(usernameInput.text);
            }
            //playerName = usernameInput.text;
        }
        if (!PlayerChecker.playersReady)
        {
            winText.gameObject.SetActive(true);
            winText.text = "Waiting for other player";
        }
        else if (PlayerChecker.playersReady)
        {
            if (!countingDown)
            {
                winText.gameObject.SetActive(true);
                //PushNameOnServer();
                StartCoroutine(CountDown());
            }
            if (!hasWon && finishedCountDown)
            {
                winText.gameObject.SetActive(false);
                GetInputs();
                CheckForGameOver();
                GroundedCheck();
                CheckForJumpParticles();
                CheckForGoal();
                TimerFunc();
            }
        }
        nameView.transform.position = transform.position;
    }

    /*public void PickedName() //
    {
        Debug.Log("Picked name", gameObject);
        myName = namePicker.GetComponentInChildren<TMP_InputField>().text;
        nameView.GetComponentInChildren<TextMeshProUGUI>().text = myName;
        gameObject.name = myName;
        //FindObjectOfType<PlayerChecker>().AddPlayerName(myName);
        PlayerChecker.playerNames.Add(myName);
        namePicker.SetActive(false);
        pickingName = false;
    }*/

    IEnumerator CountDown()
    {
        countingDown = true;
        winText.text = "3";
        yield return new WaitForSeconds(1f);
        winText.text = "2";
        yield return new WaitForSeconds(1f);
        winText.text = "1";
        yield return new WaitForSeconds(1f);
        winText.text = "GO!";
        yield return new WaitForSeconds(1f);
        finishedCountDown = true;
        StopCoroutine(CountDown());
        yield return null;
    }

    public void TimerFunc()
    {
        timer += Time.deltaTime;
        timerText.text = Convert.ToString(Convert.ToInt32(timer));
    }

    void CheckForGameOver()
    {
        if (EndGame.gameIsOver && !hasWon)
        {
            countText.gameObject.SetActive(false);
            winText.gameObject.SetActive(true);
            winText.text = "You lost!";
            Invoke("QuitGame", 2f);
        }
    }

    void GetInputs()
    {
        if (!isLocalPlayer)
            return;
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && onJumppad)
        {
            DoJump();
        }
    }

    void GroundedCheck()
    {
        onJumppad = Physics.Raycast(transform.position, Vector3.down, groundRayLength, jumpPadLayer);
        onGoal = Physics.Raycast(transform.position, Vector3.down, groundRayLength, goalLayer);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red, groundRayLength);
    }

    void CheckForJumpParticles()
    {
        if (onJumppad)
        {
            jumpParticles.transform.position = transform.position;
            if (!playingParticles)
            {
                jumpParticles.GetComponentInChildren<ParticleSystem>().Play();
                playingParticles = true;
            }
        }
        else
        {
            jumpParticles.GetComponentInChildren<ParticleSystem>().Stop();
            playingParticles = false;
        }
    }

    void CheckForGoal()
    {
        if (onGoal)
        {
            EndTheGame();
        }
    }

    void EndTheGame()
    {
        hasWon = true;
        FindObjectOfType<EndGame>().GameOver();
        //countText.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
        winText.text = "You won with time: " + timerText.text;
        Invoke("QuitGame", 2f);
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        if (!hasWon && PlayerChecker.playersReady)
        {
            rb.AddForce(movement * speed);
        }
    }

    void DoJump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            count++;
            SetCountText();
            other.gameObject.SetActive(false);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count;
        if (count >= maxCount)
        {
            countText.text = "Count: max!";
        }
    }

    void QuitGame()
    {
        EndGame.gameIsOver = false;
        //EndGame.playersReady = false;
        FindObjectOfType<EndGame>().GetComponent<NetworkManager>().StopHost();
    }
}
