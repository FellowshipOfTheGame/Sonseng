using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
    [SerializeField] private Image progress;
    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    void Start() {
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Prototype", LoadSceneMode.Additive));
        StartCoroutine(LoadScenesAsync());
    }

    private IEnumerator LoadScenesAsync() {
        Scene original = SceneManager.GetActiveScene();
        float totalProgress = 0;
        foreach(var scene in scenesToLoad){
            while(!scene.isDone){
                totalProgress += scene.progress;
                progress.fillAmount = totalProgress/scenesToLoad.Count;
                yield return null;
            }
        }
        yield return SceneManager.UnloadSceneAsync(original);
    }

}