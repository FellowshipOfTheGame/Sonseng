using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectGroup { None, G1, G2, G3, G4};

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnableMatrixSO", order = 1)]
public class SpawnableMatrix : ScriptableObject
{
    public struct SubMPos
    {
        public byte[] matrix;
        public string serial;
        public int index;
    }

    public int w, h;
    public ObjectGroup[] matrix;
    public List<SubMPos> subMatrices;

    private byte[] verified;

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

    public void CalculateSubMatrices()
    {
        subMatrices = new List<SubMPos>();
        verified = new byte[h * w];

        for (int i = 0; i < h; i++)
        {
            for(int jtnha = 0; jtnha < w; jtnha++)
            {
                int index = i * w + jtnha;
                //Se ainda nao foi verificado e faz parte de algum grupo, procura por sub matriz
                if (verified[index] == 0 && matrix[index] != ObjectGroup.None)
                {
                    int width = 1, height = 1;
                    FindSubMatrix(i, jtnha, matrix[index], ref width, ref height);
                    byte[] sub = new byte[height * width];
                    Debug.Log($"{width} e {height}");
                    for (int k = 0; k < height; k++)
                    {
                        for(int j = 0; j < width; j++)
                        {
                            if(matrix[index + (k*w+j)] == matrix[index])
                            {
                                sub[k * width + j] = 1;
                            }
                            else
                            {
                                sub[k * width + j] = 0;
                            }
                        }
                    }
                    SubMPos subStruct = new SubMPos { matrix = sub, index = index + (width - 1) + (height - 1) * h };
                    subStruct.serial = BlockDimensions.GetSerialization(width, height, sub);
                    subMatrices.Add(subStruct);
                }
            }
        }
        LogSubMatrices();
    }

    private bool FindSubMatrix(int i, int j, ObjectGroup group, ref int width, ref int height)
    {
        //Check if it is inbounds
        if(i < 0 || i >= h || j < 0 || j >= w)
        {
            return false;
        }
        int index = i * w + j;
        //Check if it is a verified node
        if(verified[index] == 1)
        {
            return false;
        }
        //Check if it belongs to same group
        if(matrix[index] == group)
        {
            //Verify passage
            verified[index] = 1;
            int nw = width + 1, nh = height;
            // Direita
            if (FindSubMatrix(i, j + 1, group, ref nw, ref nh))
            {
                width = Mathf.Max(nw, width);
                height = Mathf.Max(nh, height);
            }

            nw = width;
            nh++;
            //Baixo
            if (FindSubMatrix(i + 1, j, group, ref nw, ref nh))
            {
                width = Mathf.Max(nw, width);
                height = Mathf.Max(nh, height);
            } 
            return true;
        }
        return false;
    }

    public void LogSubMatrices()
    {
        foreach(var s in subMatrices)
        {
            Debug.Log(s.serial);
        }
    }
}
