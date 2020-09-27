using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour {
    [SerializeField] float translationTime = 0f;
    [SerializeField] float rotationTime = 0f;
    [SerializeField] string gameSceneName = null;

    [SerializeField] GameObject shop, panel, leaderboard, logo, play, options, errorPanel;

    private void Awake() {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    private void Start() {
        if (!SceneUtility.IsSceneLoaded(gameSceneName)) {
            var gameScene = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
            gameScene.completed += GameSceneCompleted;
        }
    }

    private void GameSceneCompleted(AsyncOperation obj) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameSceneName));
        GameStarter.instance.InitializeSpawners();
    }

    public void Play() {
        if(ErrorPanel.instance.isUpdated)
            GameStarter.instance.StartRunFromMainMenu();
        else
            ErrorPanel.instance.SetErrorText("Atualize seu jogo para continuar jogando!");
    }

    public void Quit() {
        Application.Quit();
    }

    public void OpenShop() {
        panel.SetActive(true);
        shop.SetActive(true);
        play.GetComponent<Button>().enabled = false;
        logo.SetActive(false);
        leaderboard.SetActive(false);
    }

    public void OpenLeaderboard() {
        panel.SetActive(true);
        shop.SetActive(false);
        play.GetComponent<Button>().enabled = false;
        logo.SetActive(false);
        leaderboard.SetActive(true);
        errorPanel.SetActive(false);
    }

    public void ClosePanel() {
        panel.SetActive(false);
        logo.SetActive(true);
        LoadingCircle.instance.EnableOrDisable(false);
        play.GetComponent<Button>().enabled = true;

    }

    public void OpenOptions() {
        panel.SetActive(false);
        play.SetActive(false);
        options.SetActive(true);
    }

    public void CloseOptions(){
        play.SetActive(true);
        options.SetActive(false);
    }
    public void CloseErrorPanel(){
        errorPanel.SetActive(false);
    }
}