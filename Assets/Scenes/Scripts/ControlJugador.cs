using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    private float _posicionPiso;
    private bool _saltandoArriba, _saltandoAbajo;

    [SerializeField]
    private Vector3 VelocidadMovimiento;
    
    [SerializeField]
    private float TopeSalto;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var brinca = Input.GetAxis("Jump") > 0.0f;
        if (!_saltandoArriba && !_saltandoAbajo && brinca)
        {
            _saltandoArriba = true;
            _posicionPiso = transform.position.y;
        }
        if (_saltandoArriba && (transform.position.y - _posicionPiso) > TopeSalto)
        {
            _saltandoAbajo = true;
            _saltandoArriba = false;
        }
        if(_saltandoAbajo && transform.position.y <= _posicionPiso)
        {
            _saltandoAbajo = false;
            transform.position = new Vector3(transform.position.x, _posicionPiso, transform.position.z);
        }
        var xFactor = Input.GetAxis("Horizontal");
        var zFactor = Input.GetAxis("Vertical");

        var jumpVal = 0.0f;
        if (_saltandoArriba)
        {
            jumpVal = VelocidadMovimiento.y;
        } else if (_saltandoAbajo)
        {
            jumpVal = VelocidadMovimiento.y *- 1;
        }

        var finalMoveVector = new Vector3(VelocidadMovimiento.x *xFactor, jumpVal , VelocidadMovimiento.z *zFactor);
        gameObject.transform.Translate(finalMoveVector * Time.deltaTime, Space.World);
       
    }

    private void OnTriggerEnter(Collider other){
        var renderer = other.gameObject.GetComponent<MeshRenderer>();
        if(renderer != null) {
            renderer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other){
        var renderer = other.gameObject.GetComponent<MeshRenderer>();
        if(renderer != null) {
            renderer.enabled = true;
        }
    }
}
