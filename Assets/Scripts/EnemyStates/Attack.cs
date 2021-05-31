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
    public static event Action gargoyleSpecialAttack;
    public static event Action berserkerSpecialAttack;
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
        Debug.Log("The Fire Imp set you on fire!");
        Target.GetComponent<Player>().SetOnFire();
    }

    protected void WaterImpAbility()
    {
        Debug.Log("The Water Imp slowed you down!");
        Target.GetComponent<Player>().SetExhausted();
    }

    protected void EarthImpAbility()
    {
        Debug.Log("The Earth Imp stunned you!");
        Target.GetComponent<Player>().ApplyBleed();
    }

    protected void AirImpAbility()
    {
        Debug.Log("The Air Imp pushed you!");
    }

    //Gargoyles
    protected void FireGargoyleAbility()
    {
        Debug.Log("The Fire Gargoyle set the area on fire!");
        gargoyleSpecialAttack?.Invoke();

    }

    protected void WaterGargoyleAbility()
    {

    }

    protected void EarthGargoyleAbility()
    {

    }

    protected void AirGargoyleAbility()
    {

    }

    //Berserkers
    protected void FireBerserkerAbility()
    {
        Debug.Log("The Fire Berserker threw a fire ball!");
        berserkerSpecialAttack?.Invoke();

    }

    protected void WaterBerserkerAbility()
    {

    }

    protected void EarthBerserkerAbility()
    {

    }

    protected void AirBerserkerAbility()
    {

    }
}
