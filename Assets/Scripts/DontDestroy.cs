using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static List<GameObject> audioes = new List<GameObject>();
    void Start()
    {
        audioes.Add(this.gameObject);
        if(audioes.Count > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
