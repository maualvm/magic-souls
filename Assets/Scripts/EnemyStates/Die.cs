using UnityEngine;

public class Die : IState
{
    private Enemy enemy;
    private ElementEnemyData enemyData;
    public Die(Enemy enemy, ElementEnemyData enemyData)
    {
        this.enemy = enemy;
        this.enemyData = enemyData;
    }
    public void Update()
    {

    }

    public void OnEnter()
    {
        Debug.Log("The " + enemyData.element + " " + enemyData.enemyData.enemyType + " died!");
        enemy.Die();
    }

    public void OnExit()
    {

    }
}
