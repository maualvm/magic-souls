using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class TPC : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public float stamina = 100f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;
    public bool canRun = true;

    [SerializeField]
    private int estoEsUnaprueba;
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float currentHealth;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;

        estoEsUnaprueba = 10000;
        Respawn();
    }

    void Update()
    {
        estoEsUnaprueba++;
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
    }
    public void ReceiveDamage(float Damage)
    {

        currentHealth -= Damage;
    }

    public void Respawn() {
        currentHealth = maxHealth;
    }
}