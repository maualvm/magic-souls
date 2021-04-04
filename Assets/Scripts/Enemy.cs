using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    ElementEnemyData enemy;
 
    [SerializeField]
    float CurrentHealth;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = enemy.enemyData.MaxHealth;
        Debug.Log("This is a " + enemy.element + " " + enemy.enemyData.enemyType);
    }

    // Update is called once per frame
    void Update()
    {

    }
}