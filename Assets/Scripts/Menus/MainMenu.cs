using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class MainMenu : MonoBehaviour
{
    [SerializeField] float translationTime  =  0f;
    [SerializeField] float rotationTime  =  0f;
    private Camera cam = null;


 
    private void Awake() {
        cam = Camera.main;
    }
    private void Start() {
       
    }
    public void Play() {
        GameStarter.instance.StartRunFromMainMenu();
        SceneManager.UnloadSceneAsync("Menu 1");
    }

    public void Quit() {
        Application.Quit();
    }
}