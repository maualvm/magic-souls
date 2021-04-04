using UnityEngine;

public class Chase : IState
{
    private string EnemyType, EnemyElement;
    public Chase(string enemyType, string enemyElement)
    {
        EnemyType = enemyType;
        EnemyElement = enemyElement;
    }

    public void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is chasing");
    }

    public void OnEnter()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting to chase");
    }

    public void OnExit()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is leaving chase");
    }
}
