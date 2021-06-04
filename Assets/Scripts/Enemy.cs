using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private ElementEnemyData enemy;

    [SerializeField]
    private float CurrentHealth;
    private float RangedAttackProbability;
    private string AttackPreference;

    [SerializeField]
    private float enemySpeedUpTime = 10f;

    [SerializeField]
    private GameObject throwable;
    [SerializeField]
    private GameObject throwableSec;

    private float mass;
    private Vector3 impulse;
    [SerializeField]
    private float pushTime = 5f;


    private NavMeshAgent navMeshAgent;

    private StateMachine stateMachine;

    private Animator animator;

    private GameObject Target;

    public static event Action<String, int> EnemyKilled;
    public static event Action<String> EtherealKilled;
    public static event Action<float, float, Enemy> EnemyHealthChanged;
    public static event Action WaterGargoyleSp;

    private void Awake()
    {
        //Get all necessary components
        Target = GameObject.Find("Jugador");
        if (Target == null)
        {
            Debug.LogError("Target not found!");
            return;
        }
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMesh Agent component missing!");
            return;
        }
        navMeshAgent.speed = enemy.enemyData.Speed;

        RangedAttackProbability = enemy.RangedAttackProbability;

        switch (enemy.AttackPreference)
        {
            case AttackTypes.Melee:
                AttackPreference = "melee";
                break;
            case AttackTypes.Ranged:
                AttackPreference = "ranged";
                break;
            case AttackTypes.Both:
                AttackPreference = ObtainAttackPreference();
                break;
        }

        stateMachine = new StateMachine();

        //States
        var patrol = new Patrol(enemy, navMeshAgent);
        var chase = new Chase(enemy, transform, Target.transform, navMeshAgent);
        var melee_attack = new MeleeAttack(enemy, navMeshAgent, Target, transform);
        var ranged_attack = new RangedAttack(enemy, navMeshAgent, Target, transform);
        var die = new Die(this, enemy);

        //Normal transitions
        At(patrol, chase, InDetectionRange());
        At(chase, patrol, OutOfDetectionRange());
        At(chase, melee_attack, InMeleeAttack());
        At(chase, ranged_attack, InRangedAttack());
        At(melee_attack, chase, OutOfAttackRange());
        At(melee_attack, ranged_attack, InRangedAttack());
        At(ranged_attack, chase, OutOfAttackRange());
        At(ranged_attack, melee_attack, InMeleeAttack());

        //Transitions that can happen at any time
        stateMachine.AddAnyTransition(die, IsDead());

        //Set initial state
        stateMachine.SetState(patrol);

        //Definitions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        Func<bool> InDetectionRange() => () => Vector3.Distance(transform.position, Target.transform.position) <= enemy.enemyData.DetectRange;
        Func<bool> OutOfDetectionRange() => () => Vector3.Distance(transform.position, Target.transform.position) > enemy.enemyData.DetectRange;
        Func<bool> InMeleeAttack() => () => Vector3.Distance(transform.position, Target.transform.position) <= enemy.enemyData.AttackRange && AttackPreference == "melee";
        Func<bool> InRangedAttack() => () => Vector3.Distance(transform.position, Target.transform.position) <= enemy.enemyData.AttackRange && AttackPreference == "ranged";
        Func<bool> OutOfAttackRange() => () => Vector3.Distance(transform.position, Target.transform.position) > enemy.enemyData.AttackRange;
        Func<bool> IsDead() => () => CurrentHealth <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = enemy.enemyData.MaxHealth;
        EnemyHealthChanged?.Invoke(CurrentHealth, enemy.enemyData.MaxHealth, this);

        gameObject.GetComponent<NavMeshAgent>().speed = enemy.enemyData.Speed;

        impulse = Vector3.zero;
        mass = gameObject.GetComponent<Rigidbody>().mass;

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Attack.gargoyleSpecialAttack += GargoyleSpecialAttack;
        Attack.berserkerSpecialAttack += BerserkerSpecialAttack;
    }

    private void OnDisable()
    {
        Attack.gargoyleSpecialAttack -= GargoyleSpecialAttack;
        Attack.berserkerSpecialAttack += BerserkerSpecialAttack;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();

        //apply impulse
        if(impulse.magnitude > 0.2f)
        {
            transform.Translate(impulse * Time.deltaTime, Space.World);
        }

        impulse = Vector3.Lerp(impulse, Vector3.zero, pushTime * Time.deltaTime);

        //Animations
        var state = this.stateMachine.CurrentState.ToString();
        switch (state)
        {
            case "Patrol":
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);

                break;
            case "Chase":
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);

                break;
            case "MeleeAttack":
                animator.SetBool("isAttacking", true);
                animator.SetBool("isRunning", false);
                break;
            case "ranged_attack":
                animator.SetBool("isAttacking", true);
                animator.SetBool("isRunning", false);

                break;
            case "die":
                animator.SetBool("isDying", true);
                break;

        }
    }

    private string ObtainAttackPreference()
    {
        float val = Random.Range(0, 100);
        if (val >= RangedAttackProbability)
            return "melee";
        else
            return "ranged";
    }

    public ElementEnemyData GetEnemyData()
    {
        return enemy;
    }

    public void Die()
    {
        PlayDeathSound();
        switch (enemy.enemyData.enemyType)
        {
            case "Imp":
                EnemyKilled?.Invoke(enemy.Element, 1);
                break;
            case "Gargoyle":
                EnemyKilled?.Invoke(enemy.Element, 3);
                break;
            case "Berserker":
                EnemyKilled?.Invoke(enemy.Element, 5);
                break;
            case "Ethereal":
                EnemyKilled?.Invoke(enemy.Element, 10);
                EtherealKilled?.Invoke(enemy.Element);
                break;
        }
    
        
        Destroy(gameObject);
    }

    public void ReceiveDamage(float Damage)
    {
        if (Damage > 0)
        {
            PlayDamageSound();
        }
        CurrentHealth -= Damage;
        if (CurrentHealth > enemy.enemyData.MaxHealth) {
            CurrentHealth = enemy.enemyData.MaxHealth;
        }
        EnemyHealthChanged?.Invoke(CurrentHealth, enemy.enemyData.MaxHealth, this);
    }

    public void PlayDamageSound()
    {
        if (Target.GetComponent<Player>().isDead)
            return;

        switch (enemy.enemyData.enemyType)
        {
            case "Imp":
                AudioManager.PlaySound(AudioManager.Sound.ImpDamaged, transform.position);
                break;
            case "Gargoyle":
                AudioManager.PlaySound(AudioManager.Sound.GargoyleDamaged, transform.position);
                break;
            case "Berserker":
                AudioManager.PlaySound(AudioManager.Sound.BerserkerDamaged, transform.position);
                break;
            case "Ethereal":
                AudioManager.PlaySound(AudioManager.Sound.EtherealDamaged, transform.position);
                break;
        }
    }

    public void PlayDeathSound()
    {
        if (Target.GetComponent<Player>().isDead)
            return;

        switch (enemy.enemyData.enemyType)
        {
            case "Imp":
                AudioManager.PlaySound(AudioManager.Sound.ImpDeath, transform.position);
                break;
            case "Gargoyle":
                AudioManager.PlaySound(AudioManager.Sound.GargoyleDeath, transform.position);
                break;
            case "Berserker":
                AudioManager.PlaySound(AudioManager.Sound.BerserkerDeath, transform.position);
                break;
            case "Ethereal":
                AudioManager.PlaySound(AudioManager.Sound.EtherealDeath, transform.position);
                break;
        }
    }

    public void PlayEffectSound()
    {
        if (Target.GetComponent<Player>().isDead)
            return;

        switch (enemy.enemyData.enemyType)
        {
            case "Imp":
                AudioManager.PlaySound(AudioManager.Sound.ImpEffect, transform.position);
                break;
            case "Gargoyle":
                AudioManager.PlaySound(AudioManager.Sound.GargoyleEffect, transform.position);
                break;
            case "Berserker":
                AudioManager.PlaySound(AudioManager.Sound.BerserkerEffect, transform.position);
                break;
            case "Ethereal":
                AudioManager.PlaySound(AudioManager.Sound.EtherealEffect, transform.position);
                break;
        }
    }
    public void GargoyleSpecialAttack(string type)
    {

        if (throwable != null)
        {
            var state = this.stateMachine.CurrentState.ToString();
            if (state == "MeleeAttack" || state == "RangedAttack")
            {
                if (type == "Fire")
                {
                    Vector3 pos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
                    Instantiate(throwable ,pos , Quaternion.identity);
                    AudioManager.PlaySound(AudioManager.Sound.Fire, transform.position);
                }
            }

        } else 
        {
            if (type == "Water")
            {
                WaterGargoyleSp?.Invoke();
                //Efectos de sonido de este ataque
            }
        }

        if(type == "WaterEthereal")
        {
            WaterGargoyleSp?.Invoke();
            //Efectos de sonido de este ataque
        }
    }

    public void BerserkerSpecialAttack(string type)
    {
        if (this == null)
            return;
        if (throwable != null && throwableSec == null)
        {
            var state = this.stateMachine.CurrentState.ToString();
            if (state == "MeleeAttack" || state == "RangedAttack")
            {
                if (type == "Fire")
                {
                    Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 5, gameObject.transform.position.z);
                    Instantiate(throwable, pos, Quaternion.identity);
                }
                else if (type == "Earth")
                {
                    Instantiate(throwable, gameObject.transform.position, Quaternion.identity);
                    //Efectos de sonido de agua
                }
                else if (type == "Water")
                {
                    Instantiate(throwable, gameObject.transform.position, Quaternion.identity);
                    //Efectos de sonido de tierra
                }

            }

        } else if (throwableSec != null)
        {
            var state = this.stateMachine.CurrentState.ToString();
            if (state == "MeleeAttack" || state == "RangedAttack")
            {
                if (type == "Fire")
                {
                    Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 5, gameObject.transform.position.z);
                    Instantiate(throwableSec, pos, Quaternion.identity);
                }
                else if (type == "Earth")
                {
                    Instantiate(throwableSec, gameObject.transform.position, Quaternion.identity);
                    //Efectos de sonido de agua
                }
                else if (type == "Water")
                {
                    Instantiate(throwableSec, gameObject.transform.position, Quaternion.identity);
                    //Efectos de sonido de tierra
                }

            }
        }
        else
        {
            if (type == "Air")
            {
                StartCoroutine(SpeedUpEnemy());
            }
        }
    }

    IEnumerator SpeedUpEnemy()
    {
        float originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed *= 2;
        yield return new WaitForSeconds(enemySpeedUpTime);
        navMeshAgent.speed = originalSpeed;
    }

    public void AddImpact(Vector3 force)
    {
        impulse += force / mass;
    }
}