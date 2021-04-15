using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private ElementEnemyData enemy;
 
    [SerializeField]
    private float CurrentHealth;

    private float RangedAttackProbability;

    private string AttackPreference;

    private StateMachine stateMachine;

    public GameObject Target;

    private void Awake()
    {
        //Get all necessary components
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
        var patrol = new Patrol(enemy.enemyData.enemyType, enemy.Element, enemy.enemyData.MinWanderTime, enemy.enemyData.MaxWanderTime, transform, enemy.enemyData.Speed);
        var chase = new Chase(enemy.enemyData.enemyType, enemy.Element, transform, Target.transform, enemy.enemyData.Speed);
        var melee_attack = new MeleeAttack(enemy.enemyData.enemyType, enemy.Element, enemy.MeleeAttackDamage, enemy.enemyData.AbilityCooldown, enemy.enemyData.AbilityProbability, 
            enemy.bAbilityIsRanged);
        var ranged_attack = new RangedAttack(enemy.enemyData.enemyType, enemy.Element, enemy.MeleeAttackDamage, enemy.enemyData.AbilityCooldown, enemy.enemyData.AbilityProbability, 
            enemy.bAbilityIsRanged);
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
        Destroy(gameObject);
    }
}