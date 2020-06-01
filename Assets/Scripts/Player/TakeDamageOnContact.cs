using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageOnContact : MonoBehaviour
{

    public LayerMask CollisionMask;

    public Action OnContact;

    private void Start()
    {
        OnContact += DieOnContact;
    }

    protected void DieOnContact()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((CollisionMask.value & LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer))) != 0)
        {
            OnContact?.Invoke();
        }
    }
}
