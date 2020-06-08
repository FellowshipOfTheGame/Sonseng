using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public float LifeSpan = 5f;

    private void OnEnable()
    {
        Invoke("Deactivate", LifeSpan);
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
