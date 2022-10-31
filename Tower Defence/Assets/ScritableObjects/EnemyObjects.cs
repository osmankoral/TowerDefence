using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Objects/Enemy")]
public class EnemyObjects : ScriptableObject
{
    public string className;
    public int health;
    public int attack;
    public float range;

}
