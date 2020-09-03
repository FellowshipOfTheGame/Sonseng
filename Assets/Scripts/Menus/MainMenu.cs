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
        GameStarter.instance.StartRun();
        // SceneManager.LoadScene(1);
        cam.transform.DOMove(new Vector3(0, 15.57f, -11.16f), translationTime).OnComplete(()=>{
            // if(rotationTime > translationTime)
            // {
            //     GameStarter.instance.StartRun();
            // }
        });
        cam.transform.DORotate(new Vector3(36.5f, 0f, 0f), rotationTime).OnComplete(()=>{
            // if(rotationTime <= translationTime)
            // {
            //     GameStarter.instance.StartRun();
            // }
        });
        SceneManager.UnloadSceneAsync("Menu 1");
    }

    public void Quit() {
        Application.Quit();
    }
}