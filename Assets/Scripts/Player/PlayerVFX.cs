using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public ParticleSystem explosionContainer;
    public ParticleSystem smokeContainer;
    public void PlayExplosion()
    {
      
        explosionContainer.Play(true);
    }

    public void PlaySmoke()
    {
        smokeContainer.Play(true);
    }
}
