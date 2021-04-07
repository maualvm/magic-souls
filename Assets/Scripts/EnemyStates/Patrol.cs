using UnityEngine;

public class Patrol : IState
{
    private string EnemyType, EnemyElement;
    public Patrol(string enemyType, string enemyElement)
    {
        EnemyType = enemyType;
        EnemyElement = enemyElement;
    }

    public void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is patroling");
    }

    public void OnEnter()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting to patrol");
    }

    public void OnExit()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is leaving patrol");
    }
}
