using System;
using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 7.5f;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField]
    private float gravity = 20.0f;
    [SerializeField]
    private Transform playerCameraParent;
    [SerializeField]
    private float lookSpeed = 2.0f;
    [SerializeField]
    private float lookXLimit = 60.0f;
    [SerializeField]
    private float stamina = 100f;

    [SerializeField]
    private GameObject onFireFX;
    [SerializeField]
    private GameObject bleedingFX;
    [SerializeField]
    private GameObject exhaustedFX;

    public bool onFire;
    public float onFireTimer;

    public bool bleeding;
    public float bleedingTimer;

    public bool isExhausted;
    public float exhaustedTimer;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    private bool canMove = true;
    private bool canRun = true;

    public static event Action PlayerKilled;
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float currentHealth;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;

        Respawn();

    }

    void Update()
    {
        if(currentHealth <= 0) {
            Die();
        }

        if (onFire)
        {
            currentHealth = currentHealth -5 * Time.deltaTime;
            onFireTimer = onFireTimer +1 * Time.deltaTime;

            if(onFireTimer >= 5)
            {
                onFire = false;
                onFireTimer = 0f;
                Destroy(GameObject.Find("PlayerOnFire(Clone)")) ;
            }
        }

        if (bleeding)
        {
            currentHealth = currentHealth - 5 * Time.deltaTime;
            bleedingTimer = bleedingTimer + 1 * Time.deltaTime;

            if (bleedingTimer >= 9)
            {
                bleeding = false;
                bleedingTimer = 0f;
               Destroy(GameObject.Find("PlayerBleeding(Clone)"));
            }
        }

        if (isExhausted)
        {
            speed = 2f;
            exhaustedTimer = exhaustedTimer + 1 * Time.deltaTime;

            if (exhaustedTimer >= 5)
            {
                isExhausted = false;
                exhaustedTimer = 0f;
                Destroy(GameObject.Find("PlayerExhaust(Clone)"));
                speed = 7.5f;
            }
        }




        if (characterController.isGrounded)
        {
            
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            if(stamina <= 0) {
                canRun = false;
            }
            if(Input.GetKey(KeyCode.LeftShift) && canRun) {
                speed = 12.5f;
                stamina -= 10 * Time.deltaTime;
            }
            if(!canRun && stamina < 100) {
                speed = 7.5f;
                stamina += 5 * Time.deltaTime;
                if(stamina >= 25)
                    canRun = true;
            }
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
            //Debug.Log($"{curSpeedX}");
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
           

        }
    }

    public void Die() {
        Debug.Log("Se murio");
        transform.Translate(0, 10, 0);
        PlayerKilled?.Invoke();
    }
    public void ReceiveDamage(float Damage)
    {

        currentHealth -= Damage;
    }

    public void Respawn() {
        currentHealth = maxHealth;
        onFire = false;
    }

    public void SetOnFire()
    {
        if (onFire)
            return;
       //else
        GameObject childObject = Instantiate(onFireFX, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        childObject.transform.parent = this.transform;
        onFireTimer = 0;
        onFire = true;
        Debug.Log("Jugador is on fire!!");

    }

    public void ApplyBleed()
    {
        if (bleeding)
            return;
        //else
        GameObject childObject = Instantiate(bleedingFX, new Vector3 (transform.position.x, transform.position.y+0.5f, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
        childObject.transform.parent = this.transform;
        bleedingTimer = 0;
        bleeding = true;
        Debug.Log("Jugador is bleeding!!");
    }

    public void SetExhausted()
    {
        if (isExhausted)
            return;
        //else
        GameObject childObject = Instantiate(exhaustedFX, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        childObject.transform.parent = this.transform;
        exhaustedTimer = 0;
        isExhausted = true;
        Debug.Log("Jugador is exhausted!!");

    }
}