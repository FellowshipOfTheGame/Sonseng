﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameStarter : MonoBehaviour
{


    public static GameStarter instance = null;
    [SerializeField] private string mainMenuSceneName = null;
    [SerializeField] private GameObject inGameUI = null;
    [SerializeField] private GameObject playerCamera = null;
    [SerializeField] private GameObject tiraTampa = null;
    [SerializeField] private GameObject simoes = null;
    [SerializeField] private Transform mainMenuCameraPosition = null;
    [SerializeField] private float waitToMoveTime = 0f;
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
            
            TimeToSpeedManager.instance.StopGame();
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
        InitializeSpawners();
        playerCamera.transform.DOMove(playerCamera.transform.position, waitToMoveTime).OnComplete(()=>{
            playerCamera.transform.DOMove(bufferPosition, translationTime);
            playerCamera.transform.DORotate(bufferRotation, rotationTime);
            StartRun();
        });
    }

    public void StartRun()
    {
        tiraTampa.GetComponent<PlayerVFX>().PlayExplosion();
        simoes.GetComponent<Animator>().SetTrigger("Fall");
        tiraTampa.GetComponentInChildren<TextureAnimation>().StartAnimation();
        tiraTampa.GetComponent<PlayerSoundEffects>().StartRunning();
        InitializeSpawners();
        inGameUI.SetActive(true);
        playerMovement.isInMainMenu = false;
        UserBackend.instance.GetBoughtUpgrades();
        TimeToSpeedManager.instance.StartNewGame();
    }
    
    public void InitializeSpawners()
    {
        var spawners = FindObjectsOfType<ScenarySpawner>();
        foreach (var s in spawners)
            s.Initialize();
    }



  
}
