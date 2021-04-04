using UnityEngine;

public class Attack : IState
{
    private string EnemyType, EnemyElement;
    public Attack(string enemyType, string enemyElement)
    {
        EnemyType = enemyType;
        EnemyElement = enemyElement;
    }

    public void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is attacking");
    }

    public void OnEnter()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting to atack");
    }

    public void OnExit()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is stopping the attack");
    }
}
