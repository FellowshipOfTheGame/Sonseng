using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectGenerator : MonoBehaviour
{
    //TODO: Use Object Pooling
    public static RandomObjectGenerator Instance = null;
    public float Speed { get => speed; set => SetSpeed(value); }

    //[SerializeField] private float time_to_spawn;
    [SerializeField] private GameObject[] objects_prefabs;
    [SerializeField] private float[] x_offsets = { -1f, 0f, 1f };
    [SerializeField] private float z_offset = 10f;

    private float Time_to_spawn { get => 10 / Speed;}
    private float speed = 4f;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else Instance = this;

        InvokeRepeating("SpawnRandomObject", Time_to_spawn, Time_to_spawn);
    }

    private void SpawnRandomObject()
    {
        float last_x = x_offsets[0]-1f;
        for (int i = Random.Range(0, 2); i < 2; i++)
        {
            //Get Random Values
            float x = 0f;
            do
            {
                x = x_offsets[Random.Range(0, 3)];
            } while (last_x == x);
            GameObject prefab = objects_prefabs[Random.Range(0, objects_prefabs.Length - 1)];

            //Spawn Object
            Instantiate(prefab, new Vector3(x, 0, z_offset), transform.rotation, transform);
            last_x = x;
        }
    }

    public void SetSpeed(float value)
    {
        speed = value;
        if (IsInvoking())
            CancelInvoke();
        InvokeRepeating("SpawnRandomObject", Time_to_spawn, Time_to_spawn);
    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }
}
