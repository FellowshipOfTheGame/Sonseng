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
    [SerializeField] PlayerMovement playerMovement;
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
        if(SceneUtility.IsSceneLoaded(mainMenuSceneName)){
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
        SceneManager.UnloadSceneAsync(mainMenuSceneName);
        playerCamera.transform.DOMove(bufferPosition, translationTime);
        playerCamera.transform.DORotate(bufferRotation, rotationTime);
        StartRun();
    }

    public void StartRun()
    {
        InitializeSpawners();
        inGameUI.SetActive(true);
        playerMovement.isInMainMenu = false;
        GameManager.instance.StartNewGame();
    }
    
    public void InitializeSpawners()
    {
        var spawners = FindObjectsOfType<ScenarySpawner>();
        foreach (var s in spawners)
            s.Initialize();
    }
  
}
