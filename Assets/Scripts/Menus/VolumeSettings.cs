using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider, sfxSlider;

    private float sfxVolume, musicVolume;

    public float SfxVolume
    {
        get => sfxVolume;
        set
        {
            PlayerPrefs.SetFloat("sfxVolume", value);
            float dB = LinearToDecibel(value);
            sfxVolume = dB;
            mixer.SetFloat("sfxVolume", dB);
        }
    }

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            PlayerPrefs.SetFloat("musicVolume", value);
            float dB = LinearToDecibel(value);
            musicVolume = dB;
            mixer.SetFloat("musicVolume", dB);
        }
    }

    private void Awake()
    {
        if(PlayerPrefs.HasKey("sfxVolume"))
            SfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        else
            mixer.GetFloat("sfxVolume", out sfxVolume);

        if(PlayerPrefs.HasKey("musicVolume"))
            MusicVolume = PlayerPrefs.GetFloat("musicVolume");
        else
            mixer.GetFloat("musicVolume", out musicVolume);

        sfxSlider.value = DecibelToLinear(sfxVolume);
        musicSlider.value = DecibelToLinear(musicVolume);
    }

    private float LinearToDecibel(float linear)
     {
         float dB;
         
         if (linear != 0)
             dB = 20.0f * Mathf.Log10(linear);
         else
             dB = -144.0f;
         
         return dB;
     }

     private float DecibelToLinear(float dB)
     {
         float linear = Mathf.Pow(10.0f, dB/20.0f);
 
         return linear;
     }
}
