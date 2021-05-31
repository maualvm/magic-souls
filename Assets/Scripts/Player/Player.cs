using System;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    [SerializeField]
    private int RegenSpeed = 5;
    private float speed;
    private float normalSpeed = 6f;
    private float RunningSpeed = 8f;
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
    private Animator animator;

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

    InventorySystem inventorySystem;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    private bool canMove = true;
    private bool canRun = true;
    public bool isDead { get; private set; }

    public static event Action PlayerKilled, PlayerRespawned;
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
    public float damageModifier = 1f;
    public float range = 1f;

    public float fireRate = 1.0f;
    public float nextFire;

    public int waterLevel = 1;
    public int fireLevel = 1;
    public int earthLevel = 1;
    public int airLevel = 1;

    public string currentSpell = "Water";

    public bool fireResistance = false;
    public bool waterResistance = false;
    public bool earthResistance = false;
    public bool airResistance = false;

    private Camera camera;

    private void OnEnable()
    {
        HUD.Respawned += Respawn;
        HUD.SpellChanged += ChangeSpell;
        ShopSystem.ShieldBought += UseShield;
        ShopSystem.StrongPotionBought += UseStrongPotion;
        ShopSystem.WeakPotionBought += UseWeakPotion;
    }

    private void OnDisable()
    {
        HUD.Respawned -= Respawn;
        HUD.SpellChanged -= ChangeSpell;
        ShopSystem.ShieldBought -= UseShield;
        ShopSystem.StrongPotionBought -= UseStrongPotion;
        ShopSystem.WeakPotionBought -= UseWeakPotion;
    }

    void Start()
    {
        camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
        inventorySystem = GetComponent<InventorySystem>();
        rotation.y = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        speed = normalSpeed;
        isDead = false;
        Respawn();

    }

    void Update()
    {
        Debug.Log($"Player speed is: {speed}");
        if(currentHealth <= 0) {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isDying", true);
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
                speed = normalSpeed;
            }
        }

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;

            // If the player is not trying to run, always regenerate stamina until 100 and set speed to normal
            if(!Input.GetKey(KeyCode.LeftShift)) {
                if(stamina >= 100) {
                    stamina = 100;
                }
                if(stamina <= 100 && stamina > 0) {
                    canRun = true;
                }
                speed = normalSpeed;

                if(curSpeedX == 0 && curSpeedY == 0 && characterController.isGrounded) {
                    ChangeStamina(RegenSpeed * 3);

                } else {
                    ChangeStamina(RegenSpeed);
                }
            }

            // If the player decides to run, discharge stamina and change speed to running
            if(canRun && Input.GetKey(KeyCode.LeftShift)) {
                speed = RunningSpeed;
                ChangeStamina(-10);
                if(stamina <= 0) {
                    canRun = false;
                    speed = normalSpeed;
                }
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && !animator.GetBool("isAttacking"))
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isDying", false);
                animator.SetBool("isRunning", true);
            } else
            {
                if(currentHealth > 0)
                {
                    animator.SetBool("isAttacking", false);
                    animator.SetBool("isDying", false);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isIdle", true);
                }
                
            }

            if(currentHealth <= 0)
            {
                animator.SetBool("isRunning", false);
            }

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            //Play walking sound
            if(moveDirection.x != 0 || moveDirection.y != 0 || moveDirection.z != 0)
            {
                if(speed == RunningSpeed)
                    AudioManager.PlaySound(AudioManager.Sound.Running, transform.position);
                else 
                    AudioManager.PlaySound(AudioManager.Sound.Walking, transform.position);
            }

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

        if(Input.GetButtonDown("Fire1") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            Shoot();
            animator.SetBool("isDying", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", true);
        }


        // Use health potion
        if(Input.GetKeyDown(KeyCode.E)) {
            UseHealthPotion();
        }
        // Use stamina potion
        if(Input.GetKeyDown(KeyCode.F)) {
            UseStaminaPotion();
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
                    base_damage = 10f;
                break;
                case 2:
                    base_damage = 200f;
                break;
                case 3:
                    base_damage = 350f;
                break;
                case 4:
                    base_damage = 800f;
                break;
                default:
                    base_damage = 800f;
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

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 20f)) {
            Debug.DrawRay(camera.transform.position, camera.transform.forward * hit.distance, Color.yellow, 5);
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>(); 

            // The player hits an enemy, so we calculate the corresponding damage
            if(enemy != null) {
                var enemyElement = enemy.gameObject.GetComponent<Enemy>().GetEnemyData().Element;

                // Set the base_damage according to the spell level 
                var rb = enemy.GetComponent<Rigidbody>();
                switch (currentSpell)
                {
                    case "Water":
                        AudioManager.PlaySound(AudioManager.Sound.WaterAttack, hit.point);
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
                        AudioManager.PlaySound(AudioManager.Sound.FireAttack, hit.point);
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
                        AudioManager.PlaySound(AudioManager.Sound.EarthAttack, hit.point);
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

                    case "Air":
                        total_damage = 0;
                        AudioManager.PlaySound(AudioManager.Sound.AirAttack, hit.point);
                        if (rb != null) {
                            Vector3 direction = enemy.transform.position - transform.position;
                            direction.y = 0;
                            GetSpellDamage("Air");
                            rb.isKinematic = false;
                            rb.AddForce(direction.normalized * base_damage, ForceMode.Impulse);
                        }
                    break;
                    
                };
                Debug.Log($"The player dealt {total_damage} to the enemy of type {enemyElement}!\nThe player used spell of type {currentSpell}");
                enemy.ReceiveDamage(total_damage);
            }
        }
    }

    private void UseStrongPotion(string type) {
        StartCoroutine(StrongEffect(type));
    }

    

    IEnumerator StrongEffect(string type) {
       switch (type)
       {
           case "Fire":
            fireResistance = true;
            yield return new WaitForSeconds(300f);
            fireResistance = false;
           break;
           case "Air":
            airResistance = true;
            yield return new WaitForSeconds(300f);
            airResistance = false;
           break;
           case "Water":
            waterResistance = true;
            yield return new WaitForSeconds(300f);
            waterResistance = false;
           break;
           case "Earth":
            earthResistance = true;
            yield return new WaitForSeconds(300f);
            earthResistance = false;
           break;
           default:
           break;
       }
        
    }

    private void UseWeakPotion(string type) {
        StartCoroutine(WeakEffect(type));
    }

    IEnumerator WeakEffect(string type) {
        switch (type)
       {
           case "Fire":
            fireResistance = true;
            yield return new WaitForSeconds(120f);
            fireResistance = false;
           break;
           case "Air":
            airResistance = true;
            yield return new WaitForSeconds(120f);
            airResistance = false;
           break;
           case "Water":
            waterResistance = true;
            yield return new WaitForSeconds(120f);
            waterResistance = false;
           break;
           case "Earth":
            earthResistance = true;
            yield return new WaitForSeconds(120f);
            earthResistance = false;
           break;
           default:
           break;
       }
    }

    private void UseHealthPotion() {
        if(inventorySystem.HealthPotions > 0 && currentHealth < maxHealth) {
            Debug.Log("The player healed himself.");
            ReceiveDamage(-15);
            inventorySystem.ModifyHealthPotions(-1);
        }
    }

    private void UseStaminaPotion() {
        if(inventorySystem.StaminaPotions > 0) {
            Debug.Log($"The player healed his stamina. Regen speed = {RegenSpeed * 6}");
            StartCoroutine("StaminaEffect");
            inventorySystem.ModifyStaminaPotions(-1);
        }
    }

    IEnumerator StaminaEffect() {
        RegenSpeed = 10;
        yield return new WaitForSeconds(30f);
        RegenSpeed = 5;
    }
    private void UseShield() {
        StartCoroutine(ShieldEffect());
    }

    IEnumerator ShieldEffect() {
        damageModifier = 0.9f;
        yield return new WaitForSeconds(120f);
        damageModifier = 1f;
    }

    private void ChangeStamina(float changeAmount)
    {
        stamina += changeAmount * Time.deltaTime;
        StaminaChanged?.Invoke(stamina, maxStamina);
    }

    public void Die() {
        if(!isDead)
        {
            AudioManager.PlaySound(AudioManager.Sound.PlayerDeath, transform.position);
            canMove = false;
            canRun = false;
            PlayerKilled?.Invoke();
            Cursor.lockState = CursorLockMode.None;
            isDead = true;
        }
    }

    public void Respawn() {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerRespawned?.Invoke();
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

        animator.SetBool("isDying", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isIdle", true);
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
        Damage *= damageModifier;
        
        if(Damage > 0 && !isDead)
            AudioManager.PlaySound(AudioManager.Sound.PlayerDamaged);

        currentHealth -= Damage;
        if (currentHealth < 0)
            currentHealth = 0;
        else if (currentHealth > maxHealth)
            currentHealth = maxHealth;
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