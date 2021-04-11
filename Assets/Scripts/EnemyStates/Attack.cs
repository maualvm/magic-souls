using UnityEngine;

public class Attack : IState
{
    private string EnemyType, EnemyElement;
    private float Damage, AbilityCooldown, AbilityTimer, AbilityProbability;
    public Attack(string enemyType, string enemyElement, float damage, float abilityCooldown, float abilityProbability)
    {
        EnemyType = enemyType;
        EnemyElement = enemyElement;
        this.Damage = damage;
        this.AbilityCooldown = abilityCooldown;
        this.AbilityProbability = abilityProbability;
    }

    public void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is attacking");

        //Attack


        //Ability
        if(AbilityTimer > 0)
        {
            AbilityTimer -= Time.deltaTime;
        }
        else
        {
            UseAbility();
            AbilityTimer = AbilityCooldown;
        }
    }

    public void OnEnter()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting to atack");
        AbilityTimer = AbilityCooldown;
    }

    public void OnExit()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is stopping the attack");
    }


    private void UseAbility()
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
    }

    private void FireImpAbility()
    {
        Debug.Log("The Fire Imp set you on fire!");
    }

    private void WaterImpAbility()
    {
        Debug.Log("The Water Imp slowed you down!");
    }

    private void EarthImpAbility()
    {
        Debug.Log("The Earth Imp stunned you!");
    }
}
