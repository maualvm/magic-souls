using UnityEngine;

public abstract class Attack : IState
{
    protected string EnemyType, EnemyElement;
    protected float Damage, AbilityCooldown, AbilityTimer, AbilityProbability;
    protected bool bAbilityIsRanged;
    public Attack(string enemyType, string enemyElement, float damage, float abilityCooldown, float abilityProbability, bool bAbilityIsRanged)
    {
        EnemyType = enemyType;
        EnemyElement = enemyElement;
        this.Damage = damage;
        this.AbilityCooldown = abilityCooldown;
        this.AbilityProbability = abilityProbability;
        this.bAbilityIsRanged = bAbilityIsRanged;
    }

    public virtual void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is attacking");

        //Attack
        DoDamage();

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
        //Early return if the ability wasn't triggered
        if (Random.Range(0, 100) > AbilityProbability)
            return;

        if(EnemyType == "Imp" && EnemyElement == "Fire")
        {
            FireImpAbility();
        }
        else if (EnemyType == "Imp" && EnemyElement == "Water")
        {
            WaterImpAbility();
        }
        else if (EnemyType == "Imp" && EnemyElement == "Earth")
        {
            EarthImpAbility();
        }
        else if (EnemyType == "Imp" && EnemyElement == "Air")
        {
            AirImpAbility();
        }
    }

    protected void FireImpAbility()
    {
        Debug.Log("The Fire Imp set you on fire!");
    }

    protected void WaterImpAbility()
    {
        Debug.Log("The Water Imp slowed you down!");
    }

    protected void EarthImpAbility()
    {
        Debug.Log("The Earth Imp stunned you!");
    }

    protected void AirImpAbility()
    {
        Debug.Log("The Air Imp pushed you!");
    }
}
