using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    void Start()
    {
        Camera TargetCamera = GetComponent<Camera>();
        TargetCamera.SetReplacementShader(Shader.Find("Unlit/Color"),"RenderType");
    }

    void Update()
    {
        
    }
}
