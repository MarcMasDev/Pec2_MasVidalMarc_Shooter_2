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

        Cursor.lockState = CursorLockMode.None; //Desbloquea el cursor para que se mueva libremente
        Cursor.visible = true; //Hace que el cursor sea visible
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        // --- Reset del Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtiene el índice de la escena actual y la vuelve a cargar
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);


    }
}
