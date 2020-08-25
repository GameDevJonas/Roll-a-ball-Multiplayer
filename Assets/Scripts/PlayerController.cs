using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    public float jumpForce;
    public float groundRayLength;

    public bool onJumppad;

    public LayerMask whatToHit;

    public TextMeshProUGUI countText, winText;

    public GameObject jumpParticles;
    public GameObject otherStuffToSpawn;
    public GameObject namePicker; //
    public GameObject nameView; //

    public int count;
    public int maxCount;

    float moveHorizontal;
    float moveVertical;

    bool hasWon;
    bool finishedCountDown;
    bool countingDown;
    //bool iAmLocal;
    bool playingParticles;

    [SyncVar]
    public bool pickingName;

    [SyncVar]
    public string myName;

    Rigidbody rb;

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
        if (isLocalPlayer)
        {
            pickingName = true;
        }
        Instantiate(otherStuffToSpawn, transform.position, Quaternion.identity, transform);
        nameView = transform.Find("OtherNececcities(Clone)").transform.Find("NameView").gameObject;
        nameView.transform.parent = null;
        namePicker.SetActive(false);
        if (isLocalPlayer)
        {
            namePicker.SetActive(true);
        }
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
    }


    public void ChooseName()
    {
        PushNameOnServer();
        PushNameOnClient();
        /*if (!hasAuthority)
        {
            PushNameOnClient();
        }
        else
        {
            PushNameOnServer();
        }*/
        namePicker.SetActive(false);
    }

    [Command]
    public void PushNameOnClient()
    {
        Debug.Log("Pushed " + myName + " from client", gameObject);
        myName = namePicker.GetComponent<NamePicker>().myName;
        nameView.GetComponentInChildren<TextMeshProUGUI>().text = myName;
        gameObject.name = myName;
        pickingName = false;
    }

    public void PushNameOnServer()
    {
        Debug.Log("Pushed " + myName + " from server", gameObject);
        myName = namePicker.GetComponent<NamePicker>().myName;
        nameView.GetComponentInChildren<TextMeshProUGUI>().text = myName;
        gameObject.name = myName;
        pickingName = false;
    }

    private void Update()
    {
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
        onJumppad = Physics.Raycast(transform.position, Vector3.down, groundRayLength, whatToHit);
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
            hasWon = true;
            FindObjectOfType<EndGame>().GameOver();
            countText.gameObject.SetActive(false);
            winText.gameObject.SetActive(true);
            winText.text = "You won!";
            Invoke("QuitGame", 2f);
        }
    }

    void QuitGame()
    {
        EndGame.gameIsOver = false;
        //EndGame.playersReady = false;
        FindObjectOfType<EndGame>().GetComponent<NetworkManager>().StopHost();
    }
}
