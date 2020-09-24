using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(ScenarySpawner), editorForChildClasses: true)]
public class ScenarySpawnerEditor : Editor
{
    SerializedProperty _percentageProp;
    SerializedProperty _stageNumberProp;
    SerializedProperty _currentStageProp;
    SerializedProperty _prefabArraysProp;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        _percentageProp = serializedObject.FindProperty("StageChangePercentages");
        _stageNumberProp = serializedObject.FindProperty("StageNumber");
        _currentStageProp = serializedObject.FindProperty("CurrentStage");
        _prefabArraysProp = serializedObject.FindProperty("StagePrefabsArray");
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        // Header
        Header("Prefab Lists");

        for(int i = 0; i < _prefabArraysProp.arraySize; i++)
        {
            EditorGUILayout.PropertyField(_prefabArraysProp.GetArrayElementAtIndex(i).FindPropertyRelative("Array"), new GUIContent($"Stage {i+1} Prefabs", $"Prefabs Spawnaveis no Estagio {i+1}"), true);
        }


        // Header
        Header("Speed Stages Configurations");

        // Show Current Stage Without being able to Edit
        EditorGUILayout.LabelField($"Current Stage {_currentStageProp.intValue}", EditorStyles.boldLabel);

        // Edit Number of Stages
        EditorGUILayout.PropertyField(_stageNumberProp, new GUIContent("Speed Stages", "Numero de dificuldades do jogo relativas a velocidade"), false);

        // Minimun of 1 stage
        if (_stageNumberProp.intValue <= 0) _stageNumberProp.intValue = 1;

        // Resize Percentages Array
        if (_percentageProp.arraySize != _stageNumberProp.intValue - 1)
        {
            for (int i = 0; i < _stageNumberProp.intValue - 1; i++)
            {
                if (i >= _percentageProp.arraySize)
                    _percentageProp.InsertArrayElementAtIndex(i);
                _percentageProp.GetArrayElementAtIndex(i).floatValue = (1f / (float)_stageNumberProp.intValue) * (i + 1);
            }

            for (int i = _stageNumberProp.intValue - 1; i < _percentageProp.arraySize; i++)
                _percentageProp.DeleteArrayElementAtIndex(i);
        }

        // Show the custom GUI controls.
        for (int i = 0; i < _percentageProp.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_percentageProp.GetArrayElementAtIndex(i), new GUIContent($"Stage {i + 1}"));
            EditorGUILayout.LabelField(" %", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }


        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

    private static void Header(string text)
    {
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
    }
}