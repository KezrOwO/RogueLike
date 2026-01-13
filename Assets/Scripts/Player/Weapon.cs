using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private float damagePlayer = 2f;
    [SerializeField] private float damageEnemy = 1f;

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Récupère le tag du projectile
        // Permet de différencier les projectiles ennemis et joueurs

        // Si le projectile est joueur et touche un ennemi
        if (collision.CompareTag("Enemy") && gameObject.CompareTag("Player")) 
        {
            collision.GetComponent<Enemy>().TakeDamage(damagePlayer);
            Destroy(gameObject);
        }

        // Si le projectile est ennemi et touche le joueur
        if (collision.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damageEnemy);
            Destroy(gameObject);
        }
    }
}
