using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mute : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;
    private bool muted;
    private bool Muted {
        get => muted;
        set {
            muted = value;
            if (muted)
                text.text = "\uf6a9";
            else
                text.text = "\uf028";
        }
    }

    private void OnEnable()
    {
        Muted = AudioListener.volume == 0f;
    }

    public void MuteGame()
    {
        Muted = !Muted;
        AudioListener.volume = Muted ? 0f : 1f;
    }
}