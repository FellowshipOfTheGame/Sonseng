using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public float LifeSpan = 5f;
    private bool invokedFlag = false;

    private void Update()
    {
        if(!invokedFlag && ScenarySpawner.Instance.spawnActive)
        {
            Invoke("Deactivate", LifeSpan);
            invokedFlag = true;
        }
    }

    private void OnDisable()
    {
       CancelInvoke("Deactivate");
    }

    private void Deactivate()
    {
       gameObject.SetActive(false);
    }

}
