using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
    public Texture[] tTexture;
    SkinnedMeshRenderer t_Renderer;
    public float changeInterval = 0.33F;
    public bool animate = false;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        t_Renderer = GetComponent<SkinnedMeshRenderer> ();
        t_Renderer.material.EnableKeyword ("_NORMALMAP");
        t_Renderer.material.EnableKeyword ("_METALLICGLOSSMAP");
    }

    // Update is called once per frame
    void Update()
    {
        if(animate) {
            index++; //Mathf.FloorToInt(Time.time / changeInterval);
            //Debug.Log(t_Renderer.material);
            index = index % tTexture.Length;
            t_Renderer.material.SetTexture("Texture2D_A9881C30", tTexture[index]);
        }
    }

    public void StartAnimation(){
        animate = true;
    }
}
