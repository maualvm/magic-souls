using UnityEngine;

public class Chase : IState
{
    private string EnemyType, EnemyElement;
    private Transform transform, Target;
    private float Speed;
    private Vector3 Direction;
    public Chase(string enemyType, string enemyElement, Transform transform, Transform target, float speed)
    {
        EnemyType = enemyType;
        EnemyElement = enemyElement;
        this.transform = transform;
        this.Target = target;
        this.Speed = speed;
    }

    public void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is chasing");
        transform.LookAt(Target);

        Direction = Target.position - transform.position;
        Direction = Direction.normalized;
        transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
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
