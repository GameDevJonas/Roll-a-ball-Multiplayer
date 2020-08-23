using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    Vector3 offset;

    bool foundPlayer = false;

    void FindOffset()
    {
        offset = transform.position - player.transform.position;
        foundPlayer = true;
    }

    public void FindPlayer(GameObject playerSelect)
    {
        player = playerSelect;
        FindOffset();
    }

    void LateUpdate()
    {
        if(foundPlayer)
        transform.position = player.transform.position + offset;
    }
}
