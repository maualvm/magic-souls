using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public bool canSpawn;

    public static event Action<string> EnteredArea;

    [SerializeField]
    string area;
    // Start is called before the first frame update
    void Start()
    {
        canSpawn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canSpawn = true;
            EnteredArea?.Invoke(area);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canSpawn = false;
            EnteredArea?.Invoke("Town");
        }
    }
}
