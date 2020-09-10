﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] float translationTime = 0f;
    [SerializeField] float rotationTime = 0f;
    [SerializeField] string gameSceneName = null;

    [SerializeField] GameObject shop, panel, leaderboard, logo;

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
        GameStarter.instance.StartRunFromMainMenu();
    }

    public void Quit() {
        Application.Quit();
    }

    public void OpenShop() {
        panel.SetActive(true);
        shop.SetActive(true);
        logo.SetActive(false);
        leaderboard.SetActive(false);
    }

    public void OpenLeaderboard() {
        panel.SetActive(true);
        shop.SetActive(false);
        logo.SetActive(false);
        leaderboard.SetActive(true);
    }

    public void ClosePanel(){
        panel.SetActive(false);
        logo.SetActive(true);
    }
}