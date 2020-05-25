using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ApiRequest : MonoBehaviour {

    public Text debugText;
    private const string WEB_CLIENT_ID = "321646647310-693rmelrk5re3cm29f2jc969n6t2mnpd.apps.googleusercontent.com";
    private bool _loginStarted;
    private string _token;
    private string _errorMsg, errorDebug;
    Firebase.Auth.FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    void Awake() {
        configuration = new GoogleSignInConfiguration {
            WebClientId = WEB_CLIENT_ID,
            RequestIdToken = true
        };
    }
    private void Start() {
        System.Random random = new System.Random();

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    }



    public void SignInGoogle() {
        
        try {

            GoogleSignIn.Configuration = configuration;

            Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

            TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
            signIn.ContinueWith(task => {
                if (task.IsCanceled) {
                    signInCompleted.SetCanceled();
                    errorDebug="Cancelada\n";
                    return;
                }
                if (task.IsFaulted) {
                    signInCompleted.SetException(task.Exception);
                    errorDebug="Falhou\n";
                    return;
                }
                errorDebug += "Completou : " + task.Result.Email + "\n";
                Firebase.Auth.Credential credential =
                    Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, task.Result.AuthCode);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
                    if (authTask.IsCanceled) {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        errorDebug += "SignInWithCredentialAsync was canceled.\n";
                        return;
                    } else if (authTask.IsFaulted) {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        errorDebug += "SignInWithCredentialAsync encountered an error: " + task.Exception;
                        return;
                    }

                    Firebase.Auth.FirebaseUser user = authTask.Result;
                    string name = user.DisplayName;
                    string email = user.Email;
                    string uid = user.UserId;
                    errorDebug += $"Name{name}\nEmail:{email}\nId:{uid}\n";
                    signInCompleted.SetResult(user);
                });

            });
            errorDebug += signInCompleted.Task.Result.DisplayName;
            debugText.text = errorDebug;
        } catch (System.Exception err) {
            debugText.text = err.ToString();
        }
        //StartCoroutine(SignIn());
        //Application.OpenURL("https://localhost:5000/auth/google");
    }

    private IEnumerator SignIn() {
        UnityWebRequest request = WebServices.Get("auth/google/");
        WebServices.CookieString = null;
        yield return request.SendWebRequest();
        if (request.isNetworkError) {
            Debug.LogError("ERROR!\n" + request.error);
        } else {
            Debug.Log(request.downloadHandler.text);
        }
    }

    public void SignOut() {
        auth.SignOut();
    }

    /*
    private IEnumerator GetScore() {
        User user = new User();
        UnityWebRequest request = WebServices.Post("getScore", JsonUtility.ToJson(user));
        WebServices.CookieString = null;
        yield return request.SendWebRequest();
        if (request.isNetworkError) {
            Debug.LogError("ERROR!\n" + request.error);
        } else {
            receivedText.text = "Score Recebida " + request.downloadHandler.text;
        }
    }

    private IEnumerator RegisterScore() {
        User user = new User();
        UnityWebRequest request = WebServices.Post("test", JsonUtility.ToJson(user));
        WebServices.CookieString = null;
        yield return request.SendWebRequest();
        if (request.isNetworkError) {
            Debug.LogError("ERROR!\n" + request.error);
        } else {
            Debug.Log("Received: " + request.responseCode);
        }

    }*/
}