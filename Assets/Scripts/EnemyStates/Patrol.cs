using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    private string EnemyType, EnemyElement;
    private float MinTime, MaxTime, WanderTime, Speed;
    private Transform transform;
    private NavMeshAgent navMeshAgent;
    public Patrol(ElementEnemyData elementEnemyData, Transform transform, NavMeshAgent navMeshAgent)
    {
        EnemyType = elementEnemyData.enemyData.enemyType;
        EnemyElement = elementEnemyData.Element;
        this.MinTime = elementEnemyData.enemyData.MinWanderTime;
        this.MaxTime = elementEnemyData.enemyData.MaxWanderTime;
        this.transform = transform;
        this.Speed = elementEnemyData.enemyData.Speed;
        this.navMeshAgent = navMeshAgent;
    }

    public void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is patroling");
        if(WanderTime > 0)
        {
            //transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            WanderTime -= Time.deltaTime;
        }
        else
        {
            Wander();
        }
    }

    public void OnEnter()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting to patrol");
        WanderTime = Random.Range(MinTime, MaxTime);
    }

    public void OnExit()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is leaving patrol");
        
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
        //transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        navMeshAgent.SetDestination(RandomPosition(20f));
        WanderTime = Random.Range(MinTime, MaxTime);
    }
}
