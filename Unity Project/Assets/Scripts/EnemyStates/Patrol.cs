using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    private string EnemyType, EnemyElement;
    private float MinTime, MaxTime, WanderTime;
    private NavMeshAgent navMeshAgent;
    public Patrol(ElementEnemyData elementEnemyData, NavMeshAgent navMeshAgent)
    {
        EnemyType = elementEnemyData.enemyData.enemyType;
        EnemyElement = elementEnemyData.Element;
        this.MinTime = elementEnemyData.enemyData.MinWanderTime;
        this.MaxTime = elementEnemyData.enemyData.MaxWanderTime;
        this.navMeshAgent = navMeshAgent;
    }

    public void Update()
    {
        if(WanderTime > 0)
        {
            WanderTime -= Time.deltaTime;
        }
        else
        {
            Wander();
        }
    }

    public void OnEnter()
    {
        WanderTime = Random.Range(MinTime, MaxTime);
    }

    public void OnExit()
    {
        
    }

    private Vector3 RandomPosition(float radius)
    {
        var randDirection = Random.insideUnitSphere * radius;
        randDirection += navMeshAgent.transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, radius, -1);
        return navHit.position;
    }

    private void Wander()
    {
        navMeshAgent.SetDestination(RandomPosition(20f));
        WanderTime = Random.Range(MinTime, MaxTime);
    }
}
