using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class RegisterScore : MonoBehaviour {

    private FirebaseDatabase database;
    private DatabaseReference reference;
    private FirebaseUser user;
    [SerializeField]
    // private Scoreboard scoreboard
    public string userId;
    void Start() {
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        userId = user.UserId;
        StartCoroutine(SaveScore());
    }

    private IEnumerator SaveScore() {

        reference.Child("users").Child(user.UserId).Child("score").SetValueAsync(Scoreboard.instance.Score);
        yield return new WaitForSeconds(10);
        StartCoroutine(SaveScore());
    }

    public void SaveScoreOnDeath() {
        StopAllCoroutines();
        Scoreboard.instance.StopScore();
        reference.Child("users").Child(user.UserId).Child("score").SetValueAsync(Scoreboard.instance.Score);
    }
}