using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
public class Login : MonoBehaviour {

    [SerializeField]
    private string webClientId;

    private GoogleSignInConfiguration configuration;
    private FirebaseAuth auth;
    private FirebaseApp app;

    public GameObject playButton, loginButton, bottomButtons;
    // public TMPro.TextMeshProUGUI title;

    private bool isLogged, hasLogged;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
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

    private void SetToken(){
        auth.CurrentUser.TokenAsync(true).ContinueWith(task=>{
            if(task.IsCompleted){
                string token = task.Result;
                RequestManager.token = token;
            }
        });
    }
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

}