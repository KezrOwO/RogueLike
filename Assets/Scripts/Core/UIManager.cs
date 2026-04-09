using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("End Run UI")]
    [SerializeField] private GameObject endRunPanel;           // Panel de fin de run
    [SerializeField] private Text endRunText;                  // Texte "Victoire" ou "Défaite"
    [SerializeField] private Button restartButton;             // Bouton recommencer

    private void Awake()
    {
        // Cacher le panel au démarrage
        if (endRunPanel != null)
        {
            endRunPanel.SetActive(false);
        }

        // Lier le bouton à la fonction de restart
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    public void ShowEndRunScreen(bool victory)
    {
        if (endRunPanel == null) return;

        // Afficher le panel
        endRunPanel.SetActive(true);

        // Mettre à jour le texte selon victoire/défaite
        if (endRunText != null)
        {
            endRunText.text = victory ? "VICTOIRE !" : "GAME OVER";
            endRunText.color = victory ? Color.green : Color.red;
        }

        // Arrêter le temps (optionnel, pour figer le jeu)
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        // Remettre le temps à la normale
        Time.timeScale = 1f;

        // Réinitialiser le flag de GameOver
        GameManager.isGameOver = false;

        // Recharger la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}