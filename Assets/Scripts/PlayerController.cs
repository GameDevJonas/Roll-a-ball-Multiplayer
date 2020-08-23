using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    public float jumpForce;
    public float groundRayLength;

    public bool onJumppad;

    public LayerMask whatNotToHit;

    public TextMeshProUGUI countText, winText;

    public GameObject jumpParticles;
    public GameObject otherStuffToSpawn;

    float moveHorizontal;
    float moveVertical;

    public int count;
    public int maxCount;

    bool hasWon;
    //bool iAmLocal;
    bool playingParticles;
    bool playersReady;

    Rigidbody rb;

    public List<GameObject> playersConnected = new List<GameObject>();

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(-1, 10, -20);
        Camera.main.transform.SetParent(null);
        Camera.main.GetComponent<CameraController>().FindPlayer(gameObject);
    }

    void Start()
    {
        Instantiate(otherStuffToSpawn, transform.position, Quaternion.identity, transform);
        //iAmLocal = GetComponentInParent<ArenaSpawn>().isLocalPlayer;
        hasWon = false;
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
        playersConnected.Capacity = 2;
        playersConnected.Add(gameObject);
    }

    private void Update()
    {
        if (!hasWon && playersReady)
        {
            GetInputs();
            CheckForGameOver();
            GroundedCheck();
            CheckForJumpParticles();
        }

        if (playersConnected.Count < 2)
        {
            CheckForPlayers();
        }
        //FIKS DETTE
        if (!playersReady)
        {
            winText.gameObject.SetActive(true);
            winText.text = "Waiting for other player";
        }
        else if (playersReady)
        {
            winText.gameObject.SetActive(false);
        }
    }

    void CheckForPlayers()
    {
        if ((playersConnected[0] == playersConnected[1]) || playersConnected[1] == null)
        {
            playersReady = false;
            playersConnected.Add(GameObject.FindGameObjectWithTag("Player"));
        }
        else
            playersReady = true;
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
        onJumppad = Physics.Raycast(transform.position, Vector3.down, groundRayLength, whatNotToHit);
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
        if (!hasWon && EndGame.playersReady)
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
