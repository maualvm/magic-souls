using UnityEngine;
using UnityEngine.AI;

public class Chase : IState
{
    private string EnemyType, EnemyElement;
    private Transform transform, Target;
    private float StoppingDistance;
    private NavMeshAgent navMeshAgent;
    public Chase(ElementEnemyData elementEnemyData, Transform transform, Transform target, NavMeshAgent navMeshAgent)
    {
        EnemyType = elementEnemyData.enemyData.enemyType;
        EnemyElement = elementEnemyData.Element;
        StoppingDistance = elementEnemyData.enemyData.AttackRange;
        this.transform = transform;
        this.Target = target;
        this.navMeshAgent = navMeshAgent;
        
    }

    public void Update()
    {
        navMeshAgent.SetDestination(Target.position);
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is chasing");
        transform.LookAt(Target);
    }

    public void OnEnter()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is starting to chase");
        navMeshAgent.stoppingDistance = StoppingDistance;
    }

    public void OnExit()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is leaving chase");
    }
}
