using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private float vignetteAmount = 0;
    private Color vignetteColor = Color.white;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_VRadius", vignetteAmount);
        material.SetColor("_Color", vignetteColor);
        Graphics.Blit(source, destination, material);
    }

    public void ChangeVignette(float pAmount)
    {
        vignetteAmount = Mathf.Lerp(1.0f, 0.5f, pAmount);
        vignetteColor = Color.Lerp(Color.white, Color.red, pAmount);
    }
}
