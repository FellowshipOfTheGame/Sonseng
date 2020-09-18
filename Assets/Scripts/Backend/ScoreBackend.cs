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

    public struct ScoreResponse{
        public int cogs;
        public float highestScore;
    }
    void Start() {
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
        GetHighestScore();
    }

    public void SaveScoreOnDeath() {
        Scoreboard.instance.StopScore();
        WWWForm form = new WWWForm();
        form.AddField("uid", user.UserId);
        form.AddField("score", (int) Scoreboard.instance.Score);
        form.AddField("cogs", Scoreboard.instance.Cogs);
        StartCoroutine(RequestManager.PostRequest<ScoreResponse>("user/saveStats", form, FinishSaveScore, LoadError));
    }

    public void FinishSaveScore(ScoreResponse res){
        Scoreboard.instance.highestScore = res.highestScore;
        Scoreboard.instance.Cogs = res.cogs;
    }

    public void LoadError(string errorMessage){
        Debug.Log(errorMessage);
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