using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ApiRequest : MonoBehaviour {

    public InputField playerText;
    public static int playerScore;
    public static string playerName;

    public Text scoreText, receivedText;
    private void Start() {
        System.Random random = new System.Random();
        playerScore = random.Next(0, 101);
        scoreText.text = "Score: " + playerScore;
    }

    public void StartRequest() {
        playerName = playerText.text;
        //StartCoroutine(GetRequest());
        StartCoroutine(PostRequest());
    }

    public void GetScoreFromServer() {
        StartCoroutine(GetScore());
    }

    private IEnumerator GetScore() {
        WWWForm form = new WWWForm();
        form.AddField("userName", playerText.text);

        UnityWebRequest uwr = UnityWebRequest.Post("https://34.95.247.47:5000/getScore", form);
        CustomCertificateHandler certHandler = new CustomCertificateHandler();
        uwr.certificateHandler = certHandler;
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError) {
            Debug.Log("Error while sending: " + uwr.error);
        } else {
            receivedText.text = "Score Recebida: " + uwr.downloadHandler.text;
        }
    }

    private IEnumerator PostRequest() {
        User user = new User();
        WWWForm form = new WWWForm();
        form.AddField("userScore", user.userScore);
        form.AddField("userName", user.userName);

        UnityWebRequest uwr = UnityWebRequest.Post("https://34.95.247.47:5000/test", form);
        CustomCertificateHandler certHandler = new CustomCertificateHandler();
        uwr.certificateHandler = certHandler;
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError) {
            Debug.Log("Error while sending: " + uwr.error);
        } else {
            Debug.Log("Received: " + uwr.responseCode);
        }

    }
}