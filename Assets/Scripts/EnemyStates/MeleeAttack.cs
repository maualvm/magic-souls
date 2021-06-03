using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : Attack
{
    private float MeleeTimer, MeleeCooldown, Damage;

    public MeleeAttack(ElementEnemyData elementEnemyData, NavMeshAgent navMeshAgent, GameObject target, Transform enemyTransform) 
        : base(elementEnemyData, navMeshAgent, target, enemyTransform)
    {
        StoppingDistance = elementEnemyData.MeleeDistance;
        MeleeCooldown = elementEnemyData.MeleeCooldown;
        Damage = elementEnemyData.MeleeAttackDamage;
    }

    public override void Update()
    {
        base.Update();
        navMeshAgent.SetDestination(Target.transform.position);

        if(MeleeTimer > 0)
        {
            MeleeTimer -= Time.deltaTime;
        }
        else
        {
            DoDamage();
            MeleeTimer = MeleeCooldown;
        }

        //Only try to use ability if it's melee
        if (!bAbilityIsRanged)
        {
            if (AbilityTimer > 0)
            {
                AbilityTimer -= Time.deltaTime;
            }
            else
            {
                UseAbility();
                AbilityTimer = AbilityCooldown;
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        navMeshAgent.stoppingDistance = StoppingDistance;
        MeleeTimer = MeleeCooldown;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void DoDamage() 
    {
        Target.GetComponent<Player>().ReceiveDamage(Damage);
    }
}
