using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;
    GameObject[] projectilePlayer; 


    public void TriggerGameOver()
    {
        isGameOver = true;
    }

    public void DestroyProjectil()
    {
        projectilePlayer = GameObject.FindGameObjectsWithTag("PlayerProjectile");
        foreach (GameObject projectile in projectilePlayer)
        {
            Destroy(projectile);
        }
    }
}
