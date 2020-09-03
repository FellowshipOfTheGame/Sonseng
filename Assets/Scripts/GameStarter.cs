using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameStarter : MonoBehaviour
{


    public static GameStarter instance = null;
    [SerializeField] private string mainMenuSceneName = null;
    [SerializeField] private GameObject inGameUI = null;
    [SerializeField] private GameObject playerCamera = null;
    [SerializeField] private Transform mainMenuCameraPosition = null;
    [SerializeField] float translationTime  =  0f;
    [SerializeField] float rotationTime  =  0f;
    [HideInInspector] public bool gameHasStarted;   

    private Vector3 bufferPosition;
    private Vector3 bufferRotation;

    // Start is called before the first frame update
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start() {
        if(isSceneLoaded(mainMenuSceneName)){
            GameManager.instance.StopGame();

            bufferPosition = playerCamera.transform.position;
            bufferRotation = playerCamera.transform.rotation.eulerAngles;
            
            playerCamera.transform.position = mainMenuCameraPosition.position; 
            playerCamera.transform.rotation = mainMenuCameraPosition.rotation;
        }
        else
        {
           StartRun();
        }
    }

    public void StartRunFromMainMenu()
    {
        // gameHasStarted = true;
        playerCamera.transform.DOMove(bufferPosition, translationTime);
        playerCamera.transform.DORotate(bufferRotation, rotationTime);
        GameManager.instance.StartNewGame();
        inGameUI.SetActive(true);
    }

    public void StartRun()
    {
        inGameUI.SetActive(true);
        GameManager.instance.StartNewGame();
    }
    
    //Checks if a scene with a name is loaded
    private bool isSceneLoaded(string sceneName)
    {
        Scene[] loadedScenes = GetOpenScenes();
        foreach (Scene scene in loadedScenes)
        {
            if(scene.name == sceneName)
            {
                return true;
            }
        }
        Debug.Log("Menu não está carregado");
        return false;
    }

    //Get all loaded scenes
    private Scene[] GetOpenScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];
 
        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }

        return loadedScenes;
    }
}
