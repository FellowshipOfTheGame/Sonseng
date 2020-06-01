using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
public class ApiRequest : MonoBehaviour {

    public Text statusText;

    public string webClientId;

    private GoogleSignInConfiguration configuration;
    private FirebaseAuth auth;
    private FirebaseApp app;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
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

    public void OnSignIn() {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestAuthCode = true;
        AddStatusText("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished);
    }

    public void OnSignOut() {
        AddStatusText("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
        auth.SignOut();
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
            AddStatusText("Email = " + task.Result.Email);
            AddStatusText("Auth code = " + task.Result.AuthCode);
            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, task.Result.AuthCode);
            AddStatusText("Credentials set!");
            FirebaseAuth(credential);
        }
    }

    private FirebaseUser newUser;
    private void FirebaseAuth(Credential cred) {
        AddStatusText("Auth Started!");
        auth.SignInWithCredentialAsync(cred).ContinueWith(task => {
            if (task.IsCanceled) {
                AddStatusText("LinkWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                AddStatusText("LinkWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            AddStatusText("Login sucessfully");
        });
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
        statusText.text = "\n" + txt;
    }

    
}