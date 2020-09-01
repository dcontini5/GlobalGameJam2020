using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassEffect : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private GameObject stand;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetVector("_PlayerPos", player.transform.position);
        material.SetVector("_StandPos", stand.transform.position);
        material.SetVector("_PlayerDir", camera.transform.forward);
        Graphics.Blit(source, destination, material);
    }
}