using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTesting : MonoBehaviour
{
    public LayerMask whatToHit;

    public float groundRayLength;
    public float speed;
    public float jumpForce;

    public GameObject jumpParticles;

    public TextMeshProUGUI countText;

    float moveHorizontal;
    float moveVertical;

    Rigidbody rb;

    bool onJumppad;
    bool playingParticles;
    bool hasWon;

    int count;

    void Start()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(-1, 10, -20);
        Camera.main.transform.SetParent(null);
        Camera.main.GetComponent<CameraController>().FindPlayer(gameObject);
        rb = GetComponent<Rigidbody>();
        countText.gameObject.SetActive(true);
        hasWon = false;
        count = 0;
        SetCountText();
        jumpParticles.GetComponentInChildren<ParticleSystem>().Stop();
    }

    void Update()
    {
        GetInputs();
        GroundedCheck();
        CheckForJumpParticles();
    }
    void GetInputs()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && onJumppad)
        {
            DoJump();
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        if (!hasWon)
        {
            rb.AddForce(movement * speed);
        }
    }

    void DoJump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0));
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
    }

}
