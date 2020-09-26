using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour
{
    //Checks if a scene with a name is loaded
    public static bool IsSceneLoaded(string sceneName)
    {
        Scene[] loadedScenes = GetOpenScenes();
        foreach (Scene scene in loadedScenes)
        {
            if(scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    //Get all loaded scenes
    public static Scene[] GetOpenScenes()
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
