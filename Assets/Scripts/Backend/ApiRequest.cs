using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiRequest : MonoBehaviour {

    public void StartRequest() {
        StartCoroutine(GetRequest());
    }
    private IEnumerator GetRequest() {
        UnityWebRequest www = UnityWebRequest.Get("https://34.95.247.47:5000");
        CustomCertificateHandler certHandler = new CustomCertificateHandler();
        www.certificateHandler = certHandler;
        


        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
            Message json = JsonUtility.FromJson<Message>(www.downloadHandler.text); 

            Debug.Log(json.message);
        }
    }
}