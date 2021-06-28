using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FireBall : MonoBehaviour
{
    Vector3 Target;
    Vector3 start;
    protected float Animation;
    private Rigidbody rigidbody;
    public static event Action<float> FireBallCollides;
    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        Target = temp.transform.position;
        start = transform.position;
        Destroy(gameObject, 5f);
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(start, Target, 5f);
        

        if(transform.position.y >= 0)
        {
            Vector3 dir = Target - transform.position;
            rigidbody.AddForce(dir.normalized * 3, ForceMode.Force);
           // transform.position.Set(transform.position.x, 0, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Terrain")
        {
            //Destroy(gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FireBallCollides?.Invoke(10f);
        }
    }
}
