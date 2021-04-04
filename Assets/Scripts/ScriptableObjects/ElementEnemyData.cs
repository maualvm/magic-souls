using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Element Enemy")]
public class ElementEnemyData : ScriptableObject
{

    public EnemyData enemyData;
    public string element;

    public GameObject Model;
}
