using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlJugador : MonoBehaviour
{
    [SerializeField]
    private Vector3 VelocidadMovimiento;

    [SerializeField]
    private float stamina = 100f;

    private DateTime _wait;
    // Start is called before the first frame update
    void Start()
    {
        _wait = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        var xFactor = Input.GetAxis("Horizontal");
        var zFactor = Input.GetAxis("Vertical");
        if(Input.GetKeyDown(KeyCode.LeftShift) && stamina > 0) {
            VelocidadMovimiento.x += 1.5f;
            VelocidadMovimiento.z += 1.5f;
            stamina -= 5 / Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && stamina < 100) {
            VelocidadMovimiento.x -= 1.5f;
            VelocidadMovimiento.z -= 1.5f;
            stamina += 5 / Time.deltaTime;
            
        }
        var finalMoveVector = new Vector3(VelocidadMovimiento.x *xFactor, 0, VelocidadMovimiento.z *zFactor);

        gameObject.transform.Translate(finalMoveVector * Time.deltaTime, Space.World);
    }
}
