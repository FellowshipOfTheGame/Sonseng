using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public FloatSharedVariable LifeSpan;

    private void OnEnable()
    {
        Invoke("Deactivate", LifeSpan.Value);
    }

    private void OnDisable()
    {
        CancelInvoke("Deactivate");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
