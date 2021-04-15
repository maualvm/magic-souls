using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Element Enemy")]
public class ElementEnemyData : ScriptableObject
{

    public EnemyData enemyData;
    public string Element;
    public AttackTypes AttackPreference;
    [Tooltip("Probability of doing a ranged attack (Only applies when AttackPreference is set to Both)")]
    [Range(0, 100)]
    public float RangedAttackProbability;
    public bool bAbilityIsRanged;
    public float MeleeAttackDamage;
    public float MeleeDistance;
    public float MeleeCooldown;
    public float RangedAttackDamage;
    public float RangedDistance;
    public float RangedCooldown;

}
public enum AttackTypes
{
    Melee,
    Ranged,
    Both
}