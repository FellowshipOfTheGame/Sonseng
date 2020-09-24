using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
using Firebase.Auth;
#endif
using UnityEngine;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour {
    public static RequestManager instance;
    [SerializeField] int timeout;
    public static string token;
    private void Start() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        version = Application.version;

    }

    public static string baseUrl = "https://us-central1-sonseng2020-1586957105557.cloudfunctions.net/app/";
    public static string debugUrl = "http://localhost:5000/sonseng2020-1586957105557/us-central1/app/";
    public static string version;
    public bool debug = false;
    public delegate void OnStringAnswer(string data);
    public delegate void OnObjectReturn<T>(T stats);
    public delegate void OnError(string error);

    public static IEnumerator GetRequest<T>(string uri, OnObjectReturn<T> callback, OnError errorCallback) {
        UnityWebRequest uwr = UnityWebRequest.Get(instance.debug ? debugUrl + uri : baseUrl + uri);
        uwr.timeout = RequestManager.instance.timeout;

        /*
        if (token != null)
            uwr.SetRequestHeader("authorization", token);

        uwr.SetRequestHeader("unity_token", unity_token);*/
        uwr.SetRequestHeader("version", version);

        uwr.SetRequestHeader("Authorization", "Bearer " + token);
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        uwr.SetRequestHeader("provider", FirebaseAuth.DefaultInstance.CurrentUser.ProviderId);
#endif
#if UNITY_WEBGL
        uwr.SetRequestHeader("provider", "GOOGLE_SIGN_IN_METHOD");
#endif
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
        UnityWebRequest uwr = UnityWebRequest.Post(instance.debug ? debugUrl + url : baseUrl + url, form);
        uwr.timeout = RequestManager.instance.timeout;

        /*
        if (token != null)
            uwr.SetRequestHeader("authorization", token);

        uwr.SetRequestHeader("unity_token", unity_token);*/
        uwr.SetRequestHeader("version", version);
        uwr.SetRequestHeader("Authorization", "Bearer " + token);

#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        uwr.SetRequestHeader("provider", FirebaseAuth.DefaultInstance.CurrentUser.ProviderId);
#endif
#if UNITY_WEBGL
        uwr.SetRequestHeader("provider", "EMAIL_PASSWORD_SIGN_IN_METHOD");
#endif
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