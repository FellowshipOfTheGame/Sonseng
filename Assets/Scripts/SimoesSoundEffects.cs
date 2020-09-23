using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimoesSoundEffects : MonoBehaviour
{
    [SerializeField] AudioClip[] idleVoiceLines;
    [SerializeField] float minVoiceLineInterval, maxVoidLineInterval;
    [SerializeField] AudioClip removeTheLidVoiceLine;
    
    AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        Invoke(nameof(RandomVoiceLine), Random.Range(minVoiceLineInterval, maxVoidLineInterval));
    }

    public void TiraATampa()
    {
        CancelInvoke(nameof(RandomVoiceLine));
        _audio.Stop();
        _audio.clip = removeTheLidVoiceLine;
        _audio.Play();
    }

    private void RandomVoiceLine()
    {
        AudioClip currentClip;

        do
            currentClip = idleVoiceLines[Random.Range(0, idleVoiceLines.Length)];
        while(currentClip == _audio.clip);

        _audio.clip = currentClip;
        _audio.Play();

        Invoke(nameof(RandomVoiceLine), Random.Range(minVoiceLineInterval, maxVoidLineInterval));
    }
}
