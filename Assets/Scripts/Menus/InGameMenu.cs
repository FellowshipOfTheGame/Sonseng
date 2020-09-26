using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class InGameMenu : MonoBehaviour {
    [SerializeField] CollisionDetector collisionDetector;
    [SerializeField] GameObject endGameMenu;
    [SerializeField] GameObject pauseMenu, header, pauseButton;
    [SerializeField] AudioClip songIntro, songLoop;
    [SerializeField] AudioClip deathJingle;

    [SerializeField] AudioSource _audioIntro, _audioLoop;
    [SerializeField] AudioMixer mixer;
    [SerializeField] float pauseVolumeReduction;

    [SerializeField] TMPro.TextMeshProUGUI pauseScoreText;

    [SerializeField] Scoreboard scoreboard;
    [SerializeField] GameObject translucid;
    
    private void Awake()
    {
        mixer.SetFloat("masterVolume", 0f);
    }

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
                TimeToSpeedManager.instance.StopGame();
                pauseMenu.SetActive(true);
                header.SetActive(false);
                mixer.SetFloat("masterVolume", -pauseVolumeReduction);
                translucid.SetActive(true);
            } else {
                translucid.SetActive(false);
                TimeToSpeedManager.instance.ResumeGame();
                pauseMenu.SetActive(false);
                header.SetActive(true);
                mixer.SetFloat("masterVolume", 0f);
            }
        }
    }

    private void ShowEndGameMenu() {
        mixer.SetFloat("masterVolume", 0f);
        _audioLoop.Stop();
        _audioIntro.Stop();
        _audioIntro.clip = deathJingle;
        _audioIntro.Play();
        pauseButton.SetActive(false);
        TimeToSpeedManager.instance.StopGame();
        StartCoroutine(WaitForJingleToEnd(3.3f));
        collisionDetector.OnDeath -= ShowEndGameMenu;
    }

    private IEnumerator WaitForJingleToEnd(float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds) {
            yield return null;
        }
        header.SetActive(false);
        endGameMenu.SetActive(true);
    }
    public void Restart() {
        Time.timeScale = 1f;
        mixer.SetFloat("masterVolume", 0f);
        TimeToSpeedManager.instance.StartNewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoBackToMenu() {
        Time.timeScale = 1f;
        mixer.SetFloat("masterVolume", 0f);
        SceneManager.LoadScene("Menu");
    }

}