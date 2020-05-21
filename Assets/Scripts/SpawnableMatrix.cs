using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectGroup { None, G1, G2, G3, G4};

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnableMatrixSO", order = 1)]
public class SpawnableMatrix : ScriptableObject
{
    public int w, h;
    public ObjectGroup[] matrix;

    public bool CheckValid()
    {
        if(matrix.Length != w * h)
        {
            Debug.LogWarning("Width and Height for SpawnableMatrix is wrong: " + name);
            return false;
        }
        return true;
    }

    public bool CheckValid(int number_of_spawn_points)
    {
        if (matrix.Length != w * h || matrix.Length != number_of_spawn_points)
        {
            Debug.LogWarning("SpawnableMatrix Not compatible with BlockGenerator: " + name);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns true if it has a sequential empty area with other
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckCompability(SpawnableMatrix other)
    {
        if(other == null)
        {
            return true;
        }

        for(int i = 0; i < w * h; i++)
        {
            if (matrix[i] == ObjectGroup.None && other.matrix[i] == matrix[i]){
                return true;
            }
        }
        return false;
    }
}
