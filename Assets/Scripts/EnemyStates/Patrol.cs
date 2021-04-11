using UnityEngine;

public class Patrol : IState
{
    private string EnemyType, EnemyElement;
    private float MinTime, MaxTime, WanderTime, Speed;
    private Transform transform;
    public Patrol(string enemyType, string enemyElement, float minTime, float maxTime, Transform transform, float speed)
    {
        EnemyType = enemyType;
        EnemyElement = enemyElement;
        this.MinTime = minTime;
        this.MaxTime = maxTime;
        this.transform = transform;
        this.Speed = speed;
    }

    public void Update()
    {
        Debug.Log("The " + EnemyElement + " " + EnemyType + " is patroling");
        if(WanderTime > 0)
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
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

    private void Wander()
    {
        transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        WanderTime = Random.Range(MinTime, MaxTime);
    }
}
