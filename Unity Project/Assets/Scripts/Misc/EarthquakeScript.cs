using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeScript : MonoBehaviour
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
