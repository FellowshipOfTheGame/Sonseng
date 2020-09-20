using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
    [SerializeField] private Image progress;

    void Start() {
        StartCoroutine(LoadScenesAsync("Menu"));
    }

    private IEnumerator LoadScenesAsync(string namesOfSceneToLoad) {
        Scene originalScene = SceneManager.GetActiveScene();

        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(namesOfSceneToLoad, LoadSceneMode.Additive);
        while(sceneLoading.progress < 0.9f){
            progress.fillAmount = Mathf.Clamp(sceneLoading.progress /0.9f, 0f, 0.5f);
            
            yield return null;
        }
        while(!sceneLoading.isDone) yield return null;

        AsyncOperation sceneUnloading = SceneManager.UnloadSceneAsync(originalScene);
        while (!sceneUnloading.isDone) {
            progress.fillAmount = Mathf.Clamp(sceneUnloading.progress / .9f, 0.5f, 1f);
            yield return null;
        }
    }

}