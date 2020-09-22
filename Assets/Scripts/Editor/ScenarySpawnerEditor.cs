using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScenarySpawner), true)]
public class ScenarySpawnerEditor : Editor
{
    ScenarySpawner Spawner = null;
    List<bool> foldout = new List<bool>();
    List<int> size = new List<int>();

    // TODO: Update dos arrays so quando da enter, salvar os arrays aqui no editor

    private void OnEnable()
    {
        Spawner = target as ScenarySpawner;
        foldout = new List<bool>();
        foreach (var obj in Spawner.StagePrefabsArray)
        {
            size.Add(obj.Length);
            foldout.Add(false);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        // Stage Prefabs Length Field
        int size = EditorGUILayout.IntField("Number Of Stages", Spawner.StagePrefabsArray.Length);
        // If size changed, allocate new memory for array without loosing previous objects
        if (size != Spawner.StagePrefabsArray.Length && size >= 0)
        {
            GameObject[][] new_list = new GameObject[size][];
            foldout = new List<bool>();
            for (int i = 0; i < size; i++)
            {
                if (i >= Spawner.StagePrefabsArray.Length)
                    new_list[i] = new GameObject[0];
                else
                    new_list[i] = Spawner.StagePrefabsArray[i];
                foldout.Add(false);
            }
            Spawner.StagePrefabsArray = new_list;
            Debug.Log(Spawner.StagePrefabsArray);
        }

        // Check foldout
        if(foldout == null || foldout.Count <= 0)
        {
            foldout = new List<bool>(Spawner.StagePrefabsArray.Length);
            for (int i = 0; i < foldout.Count; i++) foldout[i] = false;
        }

        // Show Foldouts for each prefab array
        for (int i = 0; i < Spawner.StagePrefabsArray.Length; i++)
        {
            foldout[i] = EditorGUILayout.Foldout(foldout[i], $"Stage {i} Prefabs", true, EditorStyles.foldout);
            if (foldout[i])
                DrawGameObjectArray(ref Spawner.StagePrefabsArray[i]);
        }
        
    }

    public void DrawGameObjectArray(ref GameObject[] array)
    {
        // Size Field
        int size = EditorGUILayout.IntField("Size", array.Length);
        // If size changed, allocate new memory for array without loosing previous objects
        if (size != array.Length && size >= 0)
        {
            GameObject[] new_list = new GameObject[size];
            for(int i = 0; i < size; i++)
            {
                if (i >= array.Length)
                    new_list[i] = null;
                else
                    new_list[i] = array[i];
            }
            array = new_list;
        }

        // Elements Field
        for(int i = 0; i < array.Length; i++)
        {
            array[i] = EditorGUILayout.ObjectField
                (
                    new GUIContent { text = $"Element {i}", tooltip = "Colocar um Prefab para que ele possa ser spawnado"},
                    array[i], 
                    typeof(GameObject), 
                    false
                ) as GameObject;
        }
    }
}
