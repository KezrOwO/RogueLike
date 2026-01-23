using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStat : ScriptableObject
{
    public float maxHealth = 10f;
    public float speed = 1.5f;
    public float fireForce = 5f;
    public float attackCooldown = 1f;
    public float flashDuration = 0.1f;

}
