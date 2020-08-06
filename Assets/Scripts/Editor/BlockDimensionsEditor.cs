using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockDimensions))]
public class BlockDimensionsEditor : Editor
{
    int oldW = 0, oldH = 0;
    public override void OnInspectorGUI()
    {
        BlockDimensions blockDim = (BlockDimensions) target;
         
        oldH = blockDim.height;
        oldW = blockDim.width;

        EditorGUILayout.LabelField("Matrix Dimension");
        blockDim.width = EditorGUILayout.IntField("Width", blockDim.width);
        blockDim.height = EditorGUILayout.IntField("Height", blockDim.height);

        if (oldW != blockDim.width || oldH != blockDim.height)
        {
            blockDim.matrix = new byte[blockDim.width * blockDim.height];
            oldH = blockDim.height; 
            oldW = blockDim.width;
        } 

        EditorGUILayout.LabelField("Matrix");
        for (int i = 0; i < blockDim.height; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < blockDim.width; j++)
            {
                int index = i * blockDim.width + j;
                ObjectGroup result = (ObjectGroup) EditorGUILayout.EnumPopup((ObjectGroup)blockDim.matrix[index]);
                blockDim.matrix[index] = (result == ObjectGroup.None) ? (byte) 0 : (byte) 1;
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}
