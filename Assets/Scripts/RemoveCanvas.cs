using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RemoveCanvas : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponentInParent<PlayerController>().isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
