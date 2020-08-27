using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnableMatrix))]
public class SpawnableMatrixEditor : Editor
{
    int size = 0;
    SpawnableMatrix spawnableMatrix;

    public override void OnInspectorGUI()
    {
        spawnableMatrix = (SpawnableMatrix) target;
        size = EditorGUILayout.IntField("Size", spawnableMatrix.w);
        if (size != spawnableMatrix.w && size > 0)
        {
            spawnableMatrix.matrix = new ObjectGroup[size * size];
            spawnableMatrix.w = spawnableMatrix.h = size;
        }

        EditorGUILayout.LabelField("Matrix");
        for (int i = 0; i < spawnableMatrix.h; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for(int j = 0; j < spawnableMatrix.w; j++)
            {
                int index = i * spawnableMatrix.w + j;
                spawnableMatrix.matrix[index] = (ObjectGroup) EditorGUILayout.EnumPopup(spawnableMatrix.matrix[index]);
            }
            EditorGUILayout.EndHorizontal();
        }

        if(GUILayout.Button("Calculate Sub Matrices"))
        {
            spawnableMatrix.CalculateSubMatrices();   
        }
        if(spawnableMatrix.subMatrices != null)
        {
            for(int k = 0; k < spawnableMatrix.subMatrices.Count; k++)
            {
                EditorGUILayout.LabelField($"Sub matrix {k}");
                var sub = spawnableMatrix.subMatrices[k];
                for (int i = 0; i < sub.h; i++)
                {
                    //EditorGUILayout.BeginHorizontal();
                    string sequence = "";
                    for (int j = 0; j < sub.w; j++)
                    {
                        int index = i * sub.w + j;
                        sequence = sequence + sub.matrix[index] + "   ";
                        //EditorGUILayout.LabelField(sub.matrix[index].ToString());
                    }
                    EditorGUILayout.LabelField(sequence);
                    //EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}
