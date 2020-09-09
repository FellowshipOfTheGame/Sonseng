using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour {
    private bool muted = false;
    [SerializeField] Toggle muteUI;

    private void OnEnable()
    {
        if (AudioListener.volume == 0f)
        {
            muteUI.isOn = false;
            Debug.Log("already muted");
        }
    }

    public void MuteGame()
    {
        muted = !muted;
        AudioListener.volume = muted ? 0f : 1f;
    }
}