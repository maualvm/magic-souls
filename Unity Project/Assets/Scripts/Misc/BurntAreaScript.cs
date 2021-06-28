using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntAreaScript : MonoBehaviour
{
    // Start is called before the first frame update
 
    void Awake()
    {
        Destroy(gameObject, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
