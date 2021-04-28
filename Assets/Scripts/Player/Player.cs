using System;
using UnityEngine;

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
            Debug.Log($"{curSpeedX}");
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
    }
}