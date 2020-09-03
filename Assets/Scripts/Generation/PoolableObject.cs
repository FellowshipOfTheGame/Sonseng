using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    private bool invokedFlag = false;
    public FloatSharedVariable LifeSpan;

    private void Update()
    {
        if(!invokedFlag )
        {
            Invoke("Deactivate", LifeSpan.Value);
            invokedFlag = true;
        }
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
