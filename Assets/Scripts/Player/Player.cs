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
    private float maxStamina = 100f;

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
    public static event Action<float, float> PlayerDamaged;
    public static event Action<float, float> StaminaChanged;
    public static event Action<bool> TriggeredShop;

    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float currentHealth;
    private float Damage;
    
    protected GameObject Target;

    public float base_damage = 5f;

    public float generic_damage;
    public float total_damage = 5f;
    public float range = 100f;

    public int waterLevel = 1;
    public int fireLevel = 1;
    public int earthLevel = 1;
    public int airLevel = 1;

    public string currentSpell = "Water";

    private Camera camera;

    private void OnEnable()
    {
        HUD.Respawned += Respawn;
        HUD.SpellChanged += ChangeSpell;
    }


    private void OnDisable()
    {
        HUD.Respawned -= Respawn;
        HUD.SpellChanged -= ChangeSpell;
    }

    void Start()
    {
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();
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

            float damage = 5 * Time.deltaTime;
            ReceiveDamage(damage);
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
            float damage = 5 * Time.deltaTime;
            ReceiveDamage(damage);
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
                //stamina -= 10 * Time.deltaTime;
                ChangeStamina(-10 * Time.deltaTime);
            }
            if(!canRun && stamina < 100) {
                speed = 7.5f;
                //stamina += 5 * Time.deltaTime;
                ChangeStamina(5 * Time.deltaTime);
                if (stamina >= 25)
                    canRun = true;
            }

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
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

        if(Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    public void GetSpellDamage(string typeOfSpell) {
        
        if(typeOfSpell == "Fire") {
            switch (fireLevel) {
                case 1:
                    base_damage = 5f;
                break;
                case 2:
                    base_damage = 10f;
                break;
                case 3:
                    base_damage = 25f;
                break;
                case 4:
                    base_damage = 50f;
                break;
                default:
                    base_damage = 50f;
                break;
            }
        }

        if(typeOfSpell == "Water") {
            switch (waterLevel) {
                case 1:
                    base_damage = 5f;
                break;
                case 2:
                    base_damage = 10f;
                break;
                case 3:
                    base_damage = 25f;
                break;
                case 4:
                    base_damage = 50f;
                break;
                default:
                    base_damage = 50f;
                break;
            }
        }

        if(typeOfSpell == "Earth") {
            switch (earthLevel) {
                case 1:
                    base_damage = 5f;
                break;
                case 2:
                    base_damage = 10f;
                break;
                case 3:
                    base_damage = 25f;
                break;
                case 4:
                    base_damage = 50f;
                break;
                default:
                    base_damage = 50f;
                break;
            }

        }

        if(typeOfSpell == "Air") {
            switch (airLevel) {
                case 1:
                    base_damage = 5f;
                break;
                case 2:
                    base_damage = 10f;
                break;
                case 3:
                    base_damage = 25f;
                break;
                case 4:
                    base_damage = 50f;
                break;
                default:
                    base_damage = 50f;
                break;
            }
        }
    }

    private void ChangeSpell(string element)
    {
        currentSpell = element;
    }

    public void Shoot() {
        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range)) {
            Debug.DrawRay(camera.transform.position, camera.transform.forward * hit.distance, Color.yellow, 5);
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>(); 

            // The player hits an enemy, so we calculate the corresponding damage
            if(enemy != null) {
                var enemyElement = enemy.gameObject.GetComponent<Enemy>().GetEnemyData().Element;

                // Set the base_damage according to the spell level 
                
                switch (currentSpell)
                {
                    case "Water":
                        GetSpellDamage("Water");
                        if (enemyElement == "Water") {
                            
                            total_damage = base_damage * -1;
                        }
                        else if(enemyElement == "Fire") {
                            total_damage = base_damage * 2;
                        }
                        else {
                            total_damage = base_damage;
                        }
                    break;

                    case "Fire":
                        GetSpellDamage("Fire");
                        if(enemyElement == "Fire") {
                            total_damage = base_damage * -1;
                        }
                        else if(enemyElement == "Earth") {
                            total_damage = base_damage * 2;
                        }
                        else {
                            total_damage = base_damage;
                        }
                    break;

                    case "Earth":
                        GetSpellDamage("Earth");
                        if(enemyElement == "Earth") {
                            total_damage = base_damage * -1;
                        }
                        else if(enemyElement == "Water") {
                            total_damage = base_damage * 2;
                        }
                        else {
                           total_damage = base_damage;
                        }
                    break;
                    
                }
                Debug.Log($"The player dealt {total_damage} to the enemy of type {enemyElement}!\nThe player used spell of type {currentSpell}");
                enemy.ReceiveDamage(total_damage);
            }
        }
    }

    private void ChangeStamina(float changeAmount)
    {
        stamina += changeAmount;
        StaminaChanged?.Invoke(stamina, maxStamina);
    }

    public void Die() {
        canMove = false;
        canRun = false;
        PlayerKilled?.Invoke();
        Cursor.lockState = CursorLockMode.None;
    }

    public void Respawn() {
        Cursor.lockState = CursorLockMode.Locked;
        canMove = true;
        canRun = true;
        currentHealth = maxHealth;
        PlayerDamaged?.Invoke(currentHealth, maxHealth);
        stamina = maxStamina;
        StaminaChanged?.Invoke(stamina, maxStamina);
        var respawnPosition = new Vector3(152.2615f, 1, 142.5762f);
        // turn off characterController so that it does not override transform.position
        characterController.enabled = false;
        gameObject.transform.position = respawnPosition;
        characterController.enabled = true;

        onFireTimer = 5;
        bleedingTimer = 10;
        exhaustedTimer = 5;
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

    public void ReceiveDamage(float Damage)
    {
        currentHealth -= Damage;
        if (currentHealth < 0)
            currentHealth = 0;
        PlayerDamaged?.Invoke(currentHealth, maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Shop")
        {
            TriggeredShop?.Invoke(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shop")
        {
            TriggeredShop?.Invoke(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}