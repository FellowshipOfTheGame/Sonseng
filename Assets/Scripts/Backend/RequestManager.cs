using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour {
    public static RequestManager instance;
    [SerializeField] int timeout;

    private void Start() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public static string baseUrl = "https://us-central1-sonseng2020-1586957105557.cloudfunctions.net/";
    // public static string baseUrl = "http://localhost:3000";
    public static string token = null;
    private static string unity_token = "66B1132A0173910B01EE3A15EF4E69583BBF2F7F1E4462C99EFBE1B9AB5BF808";
    public static string version = "1.6.2";

    public delegate void OnStringAnswer(string data);
    public delegate void OnObjectReturn<T>(T stats);
    public delegate void OnError(string error);

    public static IEnumerator GetRequest<T>(string uri, OnObjectReturn<T> callback, OnError errorCallback) {
        UnityWebRequest uwr = UnityWebRequest.Get(baseUrl + uri);
        uwr.timeout = RequestManager.instance.timeout;

        /*
        if (token != null)
            uwr.SetRequestHeader("authorization", token);

        uwr.SetRequestHeader("unity_token", unity_token);
        uwr.SetRequestHeader("version", version);*/

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log("Error While Sending: " + uwr.error);
            errorCallback(uwr.error);
        } else {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            if (uwr.responseCode == 200) // No error
                callback(JsonUtility.FromJson<T>(uwr.downloadHandler.text));
            else
                errorCallback(JsonUtility.FromJson<Message>(uwr.downloadHandler.text).message);
        }
    }

    public static IEnumerator PostRequest<T>(string url, WWWForm form, OnObjectReturn<T> callback, OnError errorCallback) {
        UnityWebRequest uwr = UnityWebRequest.Post(baseUrl + url, form);
        uwr.timeout = RequestManager.instance.timeout;

        /*
        if (token != null)
            uwr.SetRequestHeader("authorization", token);

        uwr.SetRequestHeader("unity_token", unity_token);
        uwr.SetRequestHeader("version", version);
        */
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) {
            Debug.Log("Error While Sending: " + uwr.error);
            errorCallback(uwr.error);
        } else {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            if (uwr.responseCode == 200) // No error
                callback(JsonUtility.FromJson<T>(uwr.downloadHandler.text));
            else
                errorCallback(JsonUtility.FromJson<Message>(uwr.downloadHandler.text).message);
        }
    }
}

[System.Serializable]
public class Message {
    public string message;
}