using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine;

public class ScoreBackend : MonoBehaviour {
    private FirebaseDatabase database;
    private DatabaseReference reference;
    [SerializeField] bool debugMode;
    private FirebaseUser user;

    void Start() {
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
        GetHighestScore();
        StartCoroutine(SaveScore());
    }

    private IEnumerator SaveScore() {

        reference.Child($"users/{user.UserId}/last-score").SetValueAsync(Scoreboard.instance.Score);
        yield return new WaitForSeconds(10);
        StartCoroutine(SaveScore());
    }

    public void SaveScoreOnDeath() {
        StopAllCoroutines();
        Scoreboard.instance.StopScore();
        reference.Child($"users/{user.UserId}/last-score").SetValueAsync(Scoreboard.instance.Score);
    }

    public void GetHighestScore() {
        reference.Child($"/users/{user.UserId}/")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.LogError(task.Exception);
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists) {
                        Scoreboard.instance.highestScore = float.Parse(snapshot.Child("highest-score").GetRawJsonValue());
                    }
                }
            });
    }
}