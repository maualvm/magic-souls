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

    private NavMeshAgent navMeshAgent;

    private StateMachine stateMachine;

    private GameObject Target;

    public static event Action<String, int> EnemyKilled;

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
        if(navMeshAgent == null)
        {
            Debug.LogError("NavMesh Agent component missing!");
            return;
        }
        navMeshAgent.speed = enemy.enemyData.Speed;

        RangedAttackProbability = enemy.RangedAttackProbability;

        switch(enemy.AttackPreference)
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
        var melee_attack = new MeleeAttack(enemy, navMeshAgent, Target);
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
        Debug.Log("This is a " + enemy.Element + " " + enemy.enemyData.enemyType);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
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
        EnemyKilled?.Invoke(enemy.Element, 1);
        Destroy(gameObject);
    }

    public void ReceiveDamage(float Damage)
    {

        CurrentHealth -= Damage;
    }
}