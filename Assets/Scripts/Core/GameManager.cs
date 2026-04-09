using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;
    GameObject[] projectilePlayer;

    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        
        if (uiManager == null)
        {
            Debug.LogWarning("GameManager: UIManager not found in scene!");
        }
    }

    // Appelé quand le joueur meurt
    public void TriggerGameOver()
    {
        if (isGameOver) return;  // Éviter les appels multiples

        isGameOver = true;

        // Afficher l'écran de défaite
        if (uiManager != null)
        {
            uiManager.ShowEndRunScreen(false);  // false = défaite
        }
    }

    // ?? Appelé quand le joueur termine la run avec succès
    public void TriggerVictory()
    {
        if (isGameOver) return;  // Éviter les appels multiples

        isGameOver = true;

        // Afficher l'écran de victoire
        if (uiManager != null)
        {
            uiManager.ShowEndRunScreen(true);  // true = victoire
        }
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
