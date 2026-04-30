using System;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private CharacterBlackboard player;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        player.OnDeath += GameOverTrigger;
        restartButton.onClick.AddListener(RestartGame);
    }
    private void OnDisable()
    {
        player.OnDeath -= GameOverTrigger;
        restartButton.onClick.RemoveListener(RestartGame);
    }
    private void GameOverTrigger()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        // Obtiene el índice de la escena actual y la vuelve a cargar
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
