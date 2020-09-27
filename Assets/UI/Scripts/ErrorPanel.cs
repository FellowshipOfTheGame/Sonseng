using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ErrorPanel : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private GameObject holder;
    public static ErrorPanel instance;

    public bool isUpdated = true;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this)
            Destroy(this.gameObject);
    }

    public void SetErrorText(string text){
        holder.SetActive(true);
        errorText.text = text;
        
    }

    public void ClosePanel(){
        holder.SetActive(false);
    }
}