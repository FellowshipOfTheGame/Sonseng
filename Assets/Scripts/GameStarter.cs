using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{


    public static GameStarter instance = null;
    [SerializeField] private GameObject scoreUI = null;
    [HideInInspector] public bool gameHasStarted;
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

    public void StartRun()
    {
        gameHasStarted = true;
        BlocksGenerator.Instance.StartSpawn();
        ScenarySpawner.Instance.StartSpawn();
        scoreUI.SetActive(true);
    }

}
