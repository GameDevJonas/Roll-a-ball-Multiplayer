using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float groundRayLength;

    public bool onJumppad;

    public LayerMask whatNotToHit;

    public TextMeshProUGUI countText, winText;

    public ParticleSystem jumpParticles;

    float moveHorizontal;
    float moveVertical;

    int count;
    int maxCount;

    bool hasWon;

    Rigidbody rb;

    void Start()
    {
        hasWon = false;
        winText.gameObject.SetActive(false);
        countText.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        maxCount = GameObject.FindGameObjectsWithTag("Pickup").Length;
        count = 0;
        SetCountText();
        jumpParticles.Stop();
    }

    private void Update()
    {
        if (!hasWon)
        {
            GetInputs();
            GroundedCheck();
            CheckForJumpParticles();
        }
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

    void GroundedCheck()
    {
        onJumppad = Physics.Raycast(transform.position, Vector3.down, groundRayLength, whatNotToHit);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red, groundRayLength);
    }

    void CheckForJumpParticles()
    {
        if (onJumppad)
        {
            jumpParticles.Play();
            jumpParticles.GetComponentInParent<Transform>().position = transform.position;
        }
        else
        {
            jumpParticles.Stop();
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
            countText.gameObject.SetActive(false);
            winText.gameObject.SetActive(true);
        }
    }
}
