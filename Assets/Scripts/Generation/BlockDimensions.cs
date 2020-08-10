using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDimensions : MonoBehaviour
{
    public int width = 1;
    public int height = 1;
    public byte[] matrix = { };

    private void Awake()
    {
        gameObject.name = GetSerialization();
    }

    public string GetSerialization()
    {
        string serial = "";
        serial = serial + width;
        serial = serial + height;
        for (int i = 0; i < width * height; i++)
            serial = serial + matrix[i];
        return serial;
    }

    public static string GetSerialization(int width, int height, byte[] matrix)
    {
        string serial = "";
        serial = serial + width;
        serial = serial + height;
        for (int i = 0; i < width * height; i++)
            serial = serial + matrix[i];
        return serial;
    }
}
