using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutline : MonoBehaviour
{
    [SerializeField] private Shader drawSimple;
    [SerializeField] private Shader outlineShader;

    private Camera attachedCamera;
    private Camera tempCamera;

    private void Start()
    {
        attachedCamera = GetComponent<Camera>();
        tempCamera = new GameObject().AddComponent<Camera>();
        tempCamera.enabled = false;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Set up the temporary camera
        tempCamera.CopyFrom(attachedCamera);
        tempCamera.clearFlags = CameraClearFlags.Color;
        tempCamera.backgroundColor = Color.black;

        // Cull everything except the outline
        tempCamera.cullingMask = 1 << LayerMask.NameToLayer("Outline");

        // Make a temporary texture
        RenderTexture rt = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);
        rt.Create();
        tempCamera.targetTexture = rt;

        // Render it
        tempCamera.RenderWithShader(drawSimple, "");
        Graphics.Blit(rt, destination);
        rt.Release();
    }
}
