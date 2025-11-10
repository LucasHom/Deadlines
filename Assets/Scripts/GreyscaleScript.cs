using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyscaleScript : MonoBehaviour
{
    public Shader shader;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = new Material(shader);
    }

    void Update()
    {

    }

    public void SetGreyscalePercentage(float percentage)
    {
        material.SetFloat("_Blend", percentage);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
