using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] AudioSource _audioSFX;
    [SerializeField] AudioSource _audioLoops;
    [SerializeField] AudioClip[] changeLaneStart, changeLaneEnd;
    [SerializeField] AudioClip jumpStart, wheelSpinLoop, jumpEnd;
    [SerializeField] AudioClip duck;
    [SerializeField] AudioClip runLoop;
    [SerializeField] AudioClip crash, lidOpen, electricalFailure, fireLoop;
    

    private bool isDead;

    private void Start()
    {
        _audioLoops.clip = runLoop;
        _audioLoops.Play();
    }

    public void ChangeLaneStart()
    {
        _audioSFX.PlayOneShot(changeLaneStart[Random.Range(0, changeLaneStart.Length)]);
        _audioLoops.Stop();
        _audioLoops.clip = wheelSpinLoop;
        _audioLoops.Play();
    }

    public void ChangeLaneEnd()
    {
        if(!isDead)
        {
            _audioSFX.PlayOneShot(changeLaneEnd[Random.Range(0, changeLaneEnd.Length)]);
            _audioLoops.Stop();
            _audioLoops.clip = runLoop;
            _audioLoops.Play();
        }
    }

    public void Crash()
    {
        isDead = true;
        _audioLoops.Stop();
        _audioSFX.PlayOneShot(crash);
        _audioSFX.PlayOneShot(lidOpen);
    }

    public void JumpStart()
    {
        _audioSFX.PlayOneShot(jumpStart);
        _audioLoops.Stop();
        _audioLoops.clip = wheelSpinLoop;
        _audioLoops.Play();
    }

    public void JumpEnd()
    {
        if(!isDead)
        {
            _audioSFX.PlayOneShot(jumpEnd);
            _audioLoops.Stop();
            _audioLoops.clip = runLoop;
            _audioLoops.Play();
        }
    }

    public void Duck()
    {
        _audioSFX.PlayOneShot(duck);
    }
}
