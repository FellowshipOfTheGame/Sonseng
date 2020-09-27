using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public ParticleSystem explosionContainer;
    public ParticleSystem smokeContainer;
    public ParticleSystem batteryContainer;
    public GameObject shieldContainer;
    public void PlayExplosion()
    {
        explosionContainer.Play(true);
    }

    public void PlaySmoke()
    {
        smokeContainer.Play(true);
    }

    public void PlayBattery(bool status = true)
    {
        if(status)
            batteryContainer.gameObject.SetActive(true);
        else
            batteryContainer.gameObject.SetActive(false);
    }

    public void PlayShield(bool status = true)
    {
        if(status)
            shieldContainer.gameObject.SetActive(true);
        else
            shieldContainer.gameObject.SetActive(false);
    }

}
