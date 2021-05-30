using UnityEngine;
using UnityEngine.AI;

public abstract class Attack : IState
{
    protected string EnemyType, EnemyElement;
    protected float AbilityCooldown, AbilityTimer, AbilityProbability, StoppingDistance;
    protected bool bAbilityIsRanged;
    protected NavMeshAgent navMeshAgent;
    protected GameObject Target;
    public Attack(ElementEnemyData elementEnemyData, NavMeshAgent navMeshAgent, GameObject target)
    {
        EnemyType = elementEnemyData.enemyData.enemyType;
        EnemyElement = elementEnemyData.Element;
        this.AbilityCooldown = elementEnemyData.enemyData.AbilityCooldown;
        this.AbilityProbability = elementEnemyData.enemyData.AbilityProbability;
        this.bAbilityIsRanged = elementEnemyData.bAbilityIsRanged;
        this.navMeshAgent = navMeshAgent;
        this.Target = target;
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
        //Early return if the ability wasn't triggered
        if (Random.Range(0, 100) > AbilityProbability)
            return;

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
    }

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
}
