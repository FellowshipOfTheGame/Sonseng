using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardBackend : MonoBehaviour {

    [SerializeField]
    private GameObject rankHolder, container;

    [Serializable]
    public struct Leader {
        public string name;
        public int highestScore;
    }

    [Serializable]
    public struct LRoot {
        public Leader[] leaders;
    }

    void Start() {
        LoadingCircle.instance.EnableOrDisable(true);
        StartCoroutine(RequestManager.GetRequest<LRoot>("/leaderboard/getLeaders", FinishGetLeaders, LoadError));
    }

    public void LoadError(string message) {
        Debug.Log(message);

    }
    public void FinishGetLeaders(LRoot root) {
        var rec = container.GetComponent<RectTransform>();

        List<Leader> lRec = new List<Leader>(root.leaders);
        lRec.Sort((a, b) => b.highestScore.CompareTo(a.highestScore));
        int rank = 1;
        Debug.Log(lRec.Count);
        foreach (Leader l in lRec) {
            Debug.Log(l.highestScore);
            var newRank = Instantiate(rankHolder, rec.position, rec.rotation, rec);
            newRank.GetComponent<RankHolder>().playerName = l.name;
            newRank.GetComponent<RankHolder>().rank = rank.ToString();
            newRank.GetComponent<RankHolder>().score = l.highestScore.ToString();
            rank++;
        }
        rec.sizeDelta = new Vector2(0, lRec.Count * rankHolder.GetComponent<RectTransform>().rect.height);
        LoadingCircle.instance.EnableOrDisable(false);
        container.SetActive(true);
    }
}