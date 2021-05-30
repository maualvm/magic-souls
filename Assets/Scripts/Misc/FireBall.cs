using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Vector3 Target;
    Vector3 start;
    protected float Animation;
    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        Target = temp.transform.position;
        start = transform.position;
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(start, Target, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Terrain")
        {
            //Destroy(gameObject);
        }
        if (other.tag == "Player")
        {
            Debug.Log("BOLA LE DIO AL JUGADOR");
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
