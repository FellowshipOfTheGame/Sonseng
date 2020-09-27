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
    [SerializeField] AudioClip crash, lidOpen, electricalFailure, electricalPop, fireLoop;
    [SerializeField] float startElectricalFailureVolumeScale = 0.5f;

    [Header("Coins and PowerUps")]
    [SerializeField] AudioSource _audioCoins;
    [SerializeField] AudioSource _audioPowerUpSFX;
    [SerializeField] AudioSource _audioPowerUpLoop;
    [SerializeField] AudioClip coin, powerUp, powerUpLoop, starLoop, debuff, powerUpEnd;
    [SerializeField] float pitchAddedPerCoinChain;
    [SerializeField] float timeToResetCoinChain;
    

    private bool isDead;

    public void StartRunning()
    {
        _audioLoops.clip = runLoop;
        _audioLoops.Play();
        ElectricalFailure();
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
        _audioSFX.PlayOneShot(electricalFailure);
        _audioLoops.clip = fireLoop;
        _audioLoops.Play();
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

    public void PickUpPowerUp(bool hasDuration = true, bool isDebuff = false)
    {
        if(!isDebuff)
            _audioPowerUpSFX.PlayOneShot(powerUp);
        else
            _audioPowerUpSFX.PlayOneShot(debuff);


        if(!_audioPowerUpLoop.isPlaying && hasDuration)
        {
            _audioPowerUpLoop.clip = powerUpLoop;
            _audioPowerUpLoop.Play();
        }
    }

    public void PickUpStar()
    {
        _audioPowerUpSFX.PlayOneShot(powerUp);
        _audioPowerUpLoop.clip = starLoop;
        _audioPowerUpLoop.Play();
    }

    public void PowerUpEnd()
    {
        _audioPowerUpSFX.PlayOneShot(powerUpEnd);
        _audioPowerUpLoop.Stop();
    }

    public void ElectricalFailure()
    {
        _audioSFX.PlayOneShot(electricalFailure, startElectricalFailureVolumeScale);
        _audioSFX.PlayOneShot(electricalPop, startElectricalFailureVolumeScale);
    }
}
