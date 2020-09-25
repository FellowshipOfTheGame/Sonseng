using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
using Firebase;
using Firebase.Auth;
using Google;
#endif
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using UnityEngine;
public class Login : MonoBehaviour {

    [SerializeField]
    private string webClientId;

    public GameObject playButton, loginButton, bottomButtons;
    // public TMPro.TextMeshProUGUI title;

    private bool isLogged, hasLogged;

#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    private GoogleSignInConfiguration configuration;
    private Firebase.Auth.FirebaseAuth auth;
    private FirebaseApp app;
    void Start() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                isLogged = auth.CurrentUser != null;
                if (auth.CurrentUser != null) {
                    SetToken();
                    isLogged = true;
                }
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        configuration = new GoogleSignInConfiguration {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestAuthCode = true,
            RequestEmail = true
        };

    }

    private void SetToken() {
        auth.CurrentUser.TokenAsync(true).ContinueWith(task => {
            if (task.IsCompleted) {
                string token = task.Result;
                RequestManager.token = token;
                UserBackend.instance.UpdateUserReference();
            }
        });
    }

    public void OnGuestSignIn() {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                isLogged = true;
                SetToken();

            } else if (task.IsCanceled) {
                Debug.LogError("Firebase Task canceled");
                return;
            } else if (task.IsFaulted) {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
        });
    }
    public void OnSignIn() {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestAuthCode = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished);
    }

    public void OnSignOut() {
        if (auth.CurrentUser != null && !auth.CurrentUser.IsAnonymous) {
            GoogleSignIn.DefaultInstance.SignOut();
        }
        auth.SignOut();
        isLogged = false;
    }

    public void OnDisconnect() {
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
        if (task.IsFaulted) {
            using(IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    GoogleSignIn.SignInException error =
                        (GoogleSignIn.SignInException)enumerator.Current;
                }
            }
        } else if (task.IsCanceled) {
            Debug.LogError("Task canceled");
        } else {
            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, task.Result.AuthCode);
            FirebaseAuth(credential);
        }
    }
    private void FirebaseAuth(Credential cred) {

        auth.SignInWithCredentialAsync(cred).ContinueWith(task => {
            if (task.IsCompleted) {
                isLogged = true;
                SetToken();

            } else if (task.IsCanceled) {
                Debug.LogError("Firebase Task canceled");
                return;
            } else if (task.IsFaulted) {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
        });

    }
#endif

    void Update() {
        if (isLogged && !hasLogged) {
            playButton.SetActive(true);
            bottomButtons.SetActive(true);
            loginButton.SetActive(false);
            hasLogged = true;
        } else if (!isLogged && hasLogged) {
            playButton.SetActive(false);
            bottomButtons.SetActive(false);
            loginButton.SetActive(true);
            hasLogged = false;
        }
    }

    public void OnSignInWeb() {
#if UNITY_WEBGL
        SignInWithGoogle(gameObject.name, "GoogleSignInCallback", "GoogleSignInFallback");
#endif
    }


#if UNITY_WEBGL
    [DllImport("__Internal")]
    public static extern void SignInWithGoogle(string objectName, string callback, string fallback);
#endif

    
    void GoogleSignInCallback(string token) {
        RequestManager.token = token;
        UserBackend.instance.UpdateUserReference();
        isLogged = true;
    }
    void GoogleSignInFallback(string output) {
        Debug.Log(output);
    }

}