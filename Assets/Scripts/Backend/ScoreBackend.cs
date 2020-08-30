using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using TMPro;
public class ScoreBackend : MonoBehaviour {
    private FirebaseDatabase database;
    private DatabaseReference reference;
    private FirebaseUser user;
    [SerializeField]
    private Scoreboard scoreboard;
    public string userId;

    void Start() {
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        userId = user.UserId;
        GetHighestScore();
        StartCoroutine(SaveScore());
    }

    private IEnumerator SaveScore() {

        reference.Child("users").Child(user.UserId).Child("last-score").SetValueAsync(scoreboard.instance.Score);
        yield return new WaitForSeconds(10);
        StartCoroutine(SaveScore());
    }

    public void SaveScoreOnDeath() {
        StopAllCoroutines();
        scoreboard.StopScore();
        reference.Child("users").Child(user.UserId).Child("last-score").SetValueAsync(scoreboard.instance.Score);
    }

    public void GetHighestScore() {
        reference.Child("users").Child(userId).Child("highest-score")
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task.Exception);
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.GetRawJsonValue());
                scoreboard.instance.highestScore = float.Parse(snapshot.GetRawJsonValue());
            }
        });
    }
}