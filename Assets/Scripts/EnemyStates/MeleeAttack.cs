using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : Attack
{
    public MeleeAttack(ElementEnemyData elementEnemyData, NavMeshAgent navMeshAgent, GameObject target) 
        : base(elementEnemyData, navMeshAgent, target)
    {
        StoppingDistance = elementEnemyData.MeleeDistance;
    }

    public override void Update()
    {
        base.Update();
        navMeshAgent.SetDestination(Target.transform.position);

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
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting the melee atack");
        navMeshAgent.stoppingDistance = StoppingDistance;
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is stopping the melee attack");
    }

    public override void DoDamage() 
    {
        Debug.Log("The enemy meleed the player");
    }
}
