using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    public MeleeAttack(string enemyType, string enemyElement, float damage, float abilityCooldown, float abilityProbability, bool bAbilityIsRanged) 
        : base(enemyType, enemyElement, damage, abilityCooldown, abilityProbability, bAbilityIsRanged)
    {

    }

    public override void Update()
    {
        base.Update();

        //Only try to use ability if it's melee
        if(!bAbilityIsRanged)
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
