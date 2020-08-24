using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTesting : MonoBehaviour
{
    public LayerMask whatToHit;

    public float groundRayLength;

    float moveHorizontal;
    float moveVertical;

    Rigidbody rb;

    bool onJumppad;
    bool playingParticles;


    void Start()
    {
        
    }

    void Update()
    {
        GetInputs();
        GroundedCheck();
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

    void DoJump()
    {

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

}
