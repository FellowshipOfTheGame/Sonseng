using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour {
    private RectTransform rect;
    [HideInInspector]
    public static LoadingCircle instance;
    public float rotateSpeed = 200f;
    public bool isEnabled = false;
    private float t = 0f;
    public Image circleImage, fakePanel;
    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        rect = circleImage.GetComponent<RectTransform>();
    }

    public void EnableOrDisable(bool enabled){
        circleImage.enabled = enabled;
        fakePanel.enabled = enabled;
        isEnabled = enabled;
    }

    // Update is called once per frame
    void Update() {
        if (isEnabled) {

            t += Time.deltaTime;
            rect.Rotate(Vector3.forward * t * rotateSpeed);
        }
    }
}