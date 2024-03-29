﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttack : Attack
{
    private Transform transform;
    private float RangedTimer, RangedCooldown, Damage;
    public RangedAttack(ElementEnemyData elementEnemyData, NavMeshAgent navMeshAgent, GameObject target, Transform transform)
        : base(elementEnemyData, navMeshAgent, target, transform)
    {
        StoppingDistance = elementEnemyData.RangedDistance;
        RangedCooldown = elementEnemyData.RangedCooldown;
        Damage = elementEnemyData.RangedAttackDamage;
        this.transform = transform;
    }

    public override void Update()
    {
        base.Update();
        //If enemy is closer than stopping distance, move away from the player
        float Distance = Vector3.Distance(transform.position, Target.transform.position);
        if (Distance + 0.2f < StoppingDistance)
        {
            Vector3 MovePosition = transform.position - Target.transform.position;
            transform.LookAt(MovePosition);
            navMeshAgent.SetDestination(MovePosition);
        }
        else
        {
            transform.LookAt(Target.transform);
            navMeshAgent.SetDestination(Target.transform.position);

            if (RangedTimer > 0)
            {
                RangedTimer -= Time.deltaTime;
            }
            else
            {
                DoDamage();
                RangedTimer = RangedCooldown;
            }

            //Only try to use ability if it's ranged
            if (bAbilityIsRanged)
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
    }

    public override void OnEnter()
    {
        base.OnEnter();
        navMeshAgent.stoppingDistance = StoppingDistance;
        RangedTimer = RangedCooldown;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void DoDamage()
    {

    }
}
