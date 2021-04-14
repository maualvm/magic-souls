using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    ElementEnemyData enemy;
 
    [SerializeField]
    float CurrentHealth;

    private StateMachine stateMachine;

    public GameObject Target;

    private void Awake()
    {
        //Get all necessary components

        stateMachine = new StateMachine();

        //States
        var patrol = new Patrol(enemy.enemyData.enemyType, enemy.Element, enemy.enemyData.MinWanderTime, enemy.enemyData.MaxWanderTime, transform, enemy.enemyData.Speed);
        var chase = new Chase(enemy.enemyData.enemyType, enemy.Element, transform, Target.transform, enemy.enemyData.Speed);
        var attack = new Attack(enemy.enemyData.enemyType, enemy.Element, enemy.enemyData.AttackDamage, enemy.enemyData.AbilityCooldown, enemy.enemyData.AbilityProbability);
        var die = new Die(this, enemy);

        //Normal transitions
        At(patrol, chase, InDetectionRange());
        At(chase, patrol, OutOfDetectionRange());
        At(chase, attack, InAttackRange());
        At(attack, chase, OutOfAttackRange());

        //Transitions that can happen at any time
        stateMachine.AddAnyTransition(die, IsDead());

        //Set initial state
        stateMachine.SetState(patrol);

        //Definitions
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        Func<bool> InDetectionRange() => () => Vector3.Distance(transform.position, Target.transform.position) <= enemy.enemyData.DetectRange;
        Func<bool> OutOfDetectionRange() => () => Vector3.Distance(transform.position, Target.transform.position) > enemy.enemyData.DetectRange;
        Func<bool> InAttackRange() => () => Vector3.Distance(transform.position, Target.transform.position) <= enemy.enemyData.AttackRange;
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

    public ElementEnemyData GetEnemyData()
    {
        return enemy;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}