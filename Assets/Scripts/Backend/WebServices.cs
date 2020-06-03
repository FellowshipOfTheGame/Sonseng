using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WebServices : MonoBehaviour {
    static string baseURL = "https://localhost:5000";
    static private CertificateHandler certHandler = new CustomCertificateHandler();
    public static string CookieString {
        get {
            return PlayerPrefs.GetString("cookie");
        }
        set {
            PlayerPrefs.SetString("cookie", value);
        }
    }

    public static string BuildUrl(string path) {
        return Path.Combine(baseURL, path).Replace(Path.DirectorySeparatorChar, '/');
    }

    public static UnityWebRequest Get(string path) {
        var request = new UnityWebRequest(BuildUrl(path), "GET");
        if (!string.IsNullOrEmpty(CookieString)) {
            request.SetRequestHeader("cookie", CookieString);
        }
        request.certificateHandler = certHandler;
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }

    public static UnityWebRequest Post(string path, string jsonString) {
        var request = new UnityWebRequest(BuildUrl(path), "POST");
        if (!string.IsNullOrEmpty(CookieString)) {
            request.SetRequestHeader("cookie", CookieString);
        }
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonString);
        request.certificateHandler = certHandler;
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }
}