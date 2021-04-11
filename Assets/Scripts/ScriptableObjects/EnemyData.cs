using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Enemies/Enemy Type")]
public class EnemyData : ScriptableObject
{

    public string enemyType;

    public float MaxHealth;
    public float Speed;
    public float DetectRange;
    public float AttackRange;
    public float MinWanderTime;
    public float MaxWanderTime;

    public float AttackDamage;
    public float AbilityCooldown;    
}