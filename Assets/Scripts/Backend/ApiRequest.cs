using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ApiRequest : MonoBehaviour {

    public Text statusText;

    public string webClientId = "321646647310-693rmelrk5re3cm29f2jc969n6t2mnpd.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;
    private Firebase.Auth.FirebaseAuth auth;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake() {
        configuration = new GoogleSignInConfiguration {
            WebClientId = webClientId,
            RequestIdToken = true
        };
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    }

    public void OnSignIn() {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished);
    }

    public void OnSignOut() {
        AddStatusText("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect() {
        AddStatusText("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
        if (task.IsFaulted) {
            using(IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    GoogleSignIn.SignInException error =
                        (GoogleSignIn.SignInException)enumerator.Current;
                    AddStatusText("Got Error: " + error.Status + " " + error.Message);
                } else {
                    AddStatusText("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        } else if (task.IsCanceled) {
            AddStatusText("Canceled");
        } else {

            AddStatusText("Welcome: " + task.Result.DisplayName + "!");
            //Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, task.Result.AuthCode);
            AddStatusText(credential.ToString());
            auth.SignInWithCredentialAsync(credential).ContinueWith(CallFirebaseAuth);

        }
    }

    internal void CallFirebaseAuth(Task<FirebaseUser> authTask) {
        AddStatusText("Calling Firebase Auth");
        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        if (authTask.IsCanceled) {
            signInCompleted.SetCanceled();
            AddStatusText("Auth CANCELADA");
        } else if (authTask.IsFaulted) {
            signInCompleted.SetException(authTask.Exception);
            AddStatusText("Auth ERRO");
        } else {
            signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
            AddStatusText("Login " + authTask.Result.Email);
        }
    }

    public void OnSignInSilently() {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddStatusText("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently()
            .ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn() {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddStatusText("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished);
    }

    private List<string> messages = new List<string>();
    void AddStatusText(string text) {
        if (messages.Count == 25) {
            messages.RemoveAt(0);
        }
        messages.Add(text);
        string txt = "";
        foreach (string s in messages) {
            txt += "\n" + s;
        }
        statusText.text = txt;
    }

    /*private bool _loginStarted;
    private string _token;
    private string _errorMsg, errorDebug;
    Firebase.Auth.FirebaseAuth auth;
    private void Start() {
        System.Random random = new System.Random();

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    }

    public void SignInGoogle() {
        try {

            GoogleSignIn.Configuration = new GoogleSignInConfiguration {
                RequestIdToken = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "321646647310-693rmelrk5re3cm29f2jc969n6t2mnpd.apps.googleusercontent.com"
            };
            Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

            TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
            signIn.ContinueWith(task => {
                if (task.IsCanceled) {
                    signInCompleted.SetCanceled();
                    debugText.text += "CANCELADA";
                } else if (task.IsFaulted) {
                    signInCompleted.SetException(task.Exception);
                    debugText.text += "Deu ruim no começo";
                } else {

                    Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                    debugText.text += credential.ToString();
                    auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
                        if (authTask.IsCanceled) {
                            signInCompleted.SetCanceled();
                            debugText.text += "Auth CANCELADA";
                        } else if (authTask.IsFaulted) {
                            signInCompleted.SetException(authTask.Exception);
                            debugText.text += "Auth ERRO";
                        } else {
                            signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                            debugText.text += "DEU CARALHO\n";
                        }
                    });
                }
            });
            debugText.text += signInCompleted.Task.Result.Email;
            debugText.text += auth.CurrentUser.Email;
            //StartCoroutine(SignIn());
            //Application.OpenURL("https://localhost:5000/auth/google");
        } catch (GoogleSignIn.SignInException e) {
            debugText.text += e.Message;
        }
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