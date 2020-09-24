using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [Header("Tira a Tampa")]
    [SerializeField] AudioSource _audioSFX;
    [SerializeField] AudioSource _audioLoops;
    [SerializeField] AudioClip[] changeLaneStart, changeLaneEnd;
    [SerializeField] AudioClip jumpStart, wheelSpinLoop, jumpEnd;
    [SerializeField] AudioClip duck;
    [SerializeField] AudioClip runLoop;
    [SerializeField] AudioClip crash, lidOpen, electricalFailure, fireLoop;

    [Header("Coins and PowerUps")]
    [SerializeField] AudioSource _audioCoins;
    [SerializeField] AudioSource _audioPowerUpSFX;
    [SerializeField] AudioSource _audioPowerUpLoop;
    [SerializeField] AudioClip coin, powerUp, powerUpEnd;
    [SerializeField] float pitchAddedPerCoinChain;
    [SerializeField] float timeToResetCoinChain;
    

    private bool isDead;

    private void OnEnable()
    {
        PowerUps.instance.OnPowerPicked += PickUpPowerUp;
    }

    public void StartRunning()
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

    public void PickUpCoin()
    {
        CancelInvoke(nameof(ResetPitch));
        _audioCoins.clip = coin;
        _audioCoins.Play();
        _audioCoins.pitch += pitchAddedPerCoinChain/100;
        Invoke(nameof(ResetPitch), timeToResetCoinChain);
    }

    private void ResetPitch()
    {
        _audioCoins.pitch = 1f;
    }

    public void PickUpPowerUp()
    {
        _audioPowerUpSFX.PlayOneShot(powerUp);
        if(!_audioPowerUpLoop.isPlaying)
            _audioPowerUpLoop.Play();
    }

    public void PowerUpEnd()
    {
        _audioPowerUpSFX.PlayOneShot(powerUpEnd);
        _audioPowerUpLoop.Stop();
    }

    private void OnDisable()
    {
        PowerUps.instance.OnPowerPicked -= PickUpPowerUp;
    }
}
