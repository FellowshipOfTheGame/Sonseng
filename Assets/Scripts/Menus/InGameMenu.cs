using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {
    [SerializeField] CollisionDetector collisionDetector;
    [SerializeField] GameObject endGameMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioClip songIntro, songLoop;
    [SerializeField] AudioClip deathJingle;

    [SerializeField] AudioSource _audioIntro, _audioLoop;

    private void Start() {
        endGameMenu.SetActive(false);
        pauseMenu.SetActive(false);

        collisionDetector.OnDeath += ShowEndGameMenu;

        _audioIntro.clip = songIntro;
        _audioLoop.clip = songLoop;
        _audioIntro.Play();
        _audioLoop.PlayDelayed(songIntro.length);
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
        _audioLoop.Stop();
        _audioIntro.Stop();
        _audioIntro.clip = deathJingle;
        _audioIntro.Play();
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