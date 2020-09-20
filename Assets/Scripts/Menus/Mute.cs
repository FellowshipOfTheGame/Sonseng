using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Mute : MonoBehaviour {
    private bool muted = false;
    [SerializeField] Toggle muteUI;
    [SerializeField] TextMeshProUGUI icon;
    private const string unMute = "\uf028";
    private const string mute = "\uf6a9";

    private void OnEnable() {
        if (AudioListener.volume == 0f)
            muteUI.isOn = false;
    }

    public void MuteGame() {
        muted = !muted;
        icon.text = muted ? mute : unMute;
        AudioListener.volume = muted ? 0f : 1f;
    }
}