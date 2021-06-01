using System;
using UnityEngine;
using UnityEngine.AI;


public abstract class Attack : IState
{
    protected string EnemyType, EnemyElement;
    protected float AbilityCooldown, AbilityTimer, AbilityProbability, StoppingDistance;
    protected bool bAbilityIsRanged;
    protected NavMeshAgent navMeshAgent;
    protected GameObject Target;
    protected Transform enemyTransform;
    public static event Action<string> gargoyleSpecialAttack;
    public static event Action<string> berserkerSpecialAttack;
    public Attack(ElementEnemyData elementEnemyData, NavMeshAgent navMeshAgent, GameObject target, Transform enemyTransform)
    {
        EnemyType = elementEnemyData.enemyData.enemyType;
        EnemyElement = elementEnemyData.Element;
        this.AbilityCooldown = elementEnemyData.enemyData.AbilityCooldown;
        this.AbilityProbability = elementEnemyData.enemyData.AbilityProbability;
        this.bAbilityIsRanged = elementEnemyData.bAbilityIsRanged;
        this.navMeshAgent = navMeshAgent;
        this.Target = target;
        this.enemyTransform = enemyTransform;
    }

    public virtual void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is attacking");
    }

    public virtual void OnEnter()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting to atack");
        AbilityTimer = AbilityCooldown;
    }

    public virtual void OnExit()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is stopping the attack");
    }

    public abstract void DoDamage();

    protected void UseAbility()
    {
        enemyTransform.GetComponent<Enemy>().PlayEffectSound();
        //Early return if the ability wasn't triggered
        if (UnityEngine.Random.Range(0, 100) > AbilityProbability)
            return;

        //imps
        if(EnemyType == "Imp" && EnemyElement == "Fire")
        {
            if(Target.GetComponent<Player>().fireResistance) return;
            FireImpAbility();
        }
        else if (EnemyType == "Imp" && EnemyElement == "Water")
        {
            if(Target.GetComponent<Player>().waterResistance) return;
            WaterImpAbility();
        }
        else if (EnemyType == "Imp" && EnemyElement == "Earth")
        {
            if(Target.GetComponent<Player>().earthResistance) return;
            EarthImpAbility();
        }
        else if (EnemyType == "Imp" && EnemyElement == "Air")
        {
            if(Target.GetComponent<Player>().airResistance) return;
            AirImpAbility();
        }

        //Gargoyles
        if (EnemyType == "Gargoyle" && EnemyElement == "Fire")
        {
            FireGargoyleAbility();
        }
        else if (EnemyType == "Gargoyle" && EnemyElement == "Water")
        {
            WaterGargoyleAbility();
        }
        else if (EnemyType == "Gargoyle" && EnemyElement == "Earth")
        {
            EarthGargoyleAbility();
        }
        else if (EnemyType == "Gargoyle" && EnemyElement == "Air")
        {
            AirGargoyleAbility();
        }

        //Berserkers
        if (EnemyType == "Berserker" && EnemyElement == "Fire")
        {
            FireBerserkerAbility();
        }
        else if (EnemyType == "Berserker" && EnemyElement == "Water")
        {
            WaterBerserkerAbility();
        }
        else if (EnemyType == "Berserker" && EnemyElement == "Earth")
        {
            EarthBerserkerAbility();
        }
        else if (EnemyType == "Berserker" && EnemyElement == "Air")
        {
            AirBerserkerAbility();
        }
    }

    //imps
    protected void FireImpAbility()
    {
        Target.GetComponent<Player>().SetOnFire();
    }

    protected void WaterImpAbility()
    {
        Target.GetComponent<Player>().SetExhausted();
    }

    protected void EarthImpAbility()
    {
        Target.GetComponent<Player>().ApplyStun();
    }

    protected void AirImpAbility()
    {
        Vector3 direction = enemyTransform.position - Target.transform.position;
        direction.y = 0;
        Target.GetComponent<Player>().AddImpact(direction.normalized * 10f);
    }

    //Gargoyles
    protected void FireGargoyleAbility()
    {
        gargoyleSpecialAttack?.Invoke("Fire");

    }

    protected void WaterGargoyleAbility()
    {
        gargoyleSpecialAttack?.Invoke("Water");
    }

    protected void EarthGargoyleAbility()
    {
        Target.GetComponent<Player>().ApplyBleed();
    }

    protected void AirGargoyleAbility()
    {
        Vector3 direction = enemyTransform.position - Target.transform.position;
        direction.y = 0;
        Target.GetComponent<Player>().AddImpact(direction.normalized * 20f);
    }

    //Berserkers
    protected void FireBerserkerAbility()
    {
        berserkerSpecialAttack?.Invoke("Fire");

    }

    protected void WaterBerserkerAbility()
    {
        berserkerSpecialAttack?.Invoke("Water");
    }

    protected void EarthBerserkerAbility()
    {
        berserkerSpecialAttack?.Invoke("Earth");
    }

    protected void AirBerserkerAbility()
    {
        berserkerSpecialAttack?.Invoke("Air");
    }
}
