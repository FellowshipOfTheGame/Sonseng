using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {
    [SerializeField] CollisionDetector collisionDetector;
    [SerializeField] GameObject endGameMenu;
    [SerializeField] GameObject pauseMenu, header;
    [SerializeField] AudioClip songIntro, songLoop;
    [SerializeField] AudioClip deathJingle;

    [SerializeField] AudioSource _audioIntro, _audioLoop;

    [SerializeField] TMPro.TextMeshProUGUI pauseScoreText;

    [SerializeField] Scoreboard scoreboard;

    private void Start() {
        endGameMenu.SetActive(false);
        pauseMenu.SetActive(false);

        _audioIntro.clip = songIntro;
        _audioLoop.clip = songLoop;
        _audioIntro.Play();
        _audioLoop.PlayDelayed(songIntro.length);

    }

    void OnEnable() {
        collisionDetector.OnDeath += ShowEndGameMenu;
    }

    private void OnDisable() {
        collisionDetector.OnDeath -= ShowEndGameMenu;
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
                header.SetActive(false);
            } else {
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
                header.SetActive(true);
            }
        }
    }

    private void ShowEndGameMenu() {
        endGameMenu.SetActive(true);
        header.SetActive(false);
        _audioLoop.Stop();
        _audioIntro.Stop();
        _audioIntro.clip = deathJingle;
        _audioIntro.Play();
        collisionDetector.OnDeath -= ShowEndGameMenu;
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