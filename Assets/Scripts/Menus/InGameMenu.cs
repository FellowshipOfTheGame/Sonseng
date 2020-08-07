using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {
    [SerializeField] TakeDamageOnContact collisionDetector;
    [SerializeField] GameObject endGameMenu;
    [SerializeField] GameObject pauseMenu;

    private void Start() {
        endGameMenu.SetActive(false);
        pauseMenu.SetActive(false);

        collisionDetector.OnDeath += ShowEndGameMenu;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseOrResume();
        }
    }

    /// <summary>
    /// Pauses the game if possible, and resume if it is already paused
    /// </summary>
    public void PauseOrResume() {
        // If not in another menu
        if (!endGameMenu.activeInHierarchy) {
            // If not paused, pause. Else, resume
            if (!pauseMenu.activeInHierarchy) {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
            } else {
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
            }
        }
    }

    private void ShowEndGameMenu() {
        endGameMenu.SetActive(true); 
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoBackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}