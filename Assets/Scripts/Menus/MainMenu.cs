using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class MainMenu : MonoBehaviour
{
    [SerializeField] float translationTime  =  0f;
    [SerializeField] float rotationTime  =  0f;
    [SerializeField] string gameSceneName = null;


 
    private void Awake()
    {

    }

    private void Start()
    {
        if(!SceneUtility.IsSceneLoaded(gameSceneName))
        {
            var gameScene = SceneManager.LoadSceneAsync(gameSceneName,LoadSceneMode.Additive);
            gameScene.completed += GameSceneCompleted;
        }
    }

    private void GameSceneCompleted(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameSceneName));
        GameStarter.instance.InitializeSpawners();
    }

    public void Play()
    {
        GameStarter.instance.StartRunFromMainMenu();
    }

    public void Quit() {
        Application.Quit();
    }
}