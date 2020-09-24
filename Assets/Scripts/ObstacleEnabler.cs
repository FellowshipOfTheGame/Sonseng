using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEnabler : MonoBehaviour
{
    [SerializeField] Obstacle[] obstacles;

    private void OnEnable()
    {
        foreach (var obj in obstacles)
            obj.gameObject.SetActive(true);
    }

    [ContextMenu("Get Obstacle Components in Children")]
    public void GetObstacleCompInChildren()
    {
        obstacles = GetComponentsInChildren<Obstacle>();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }
}
