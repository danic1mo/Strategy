using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    private Camera _minimapCamera;
    private Camera _mainCamera;
    public float offsetPositionZ;
    void Start()
    {
        GameObject minimapCameraObject = GameObject.FindGameObjectWithTag("MinimapCamera");
        _minimapCamera = minimapCameraObject.GetComponent<Camera>();
        _mainCamera = Camera.main;
        offsetPositionZ = transform.position.z - _mainCamera.transform.position.z;
    }
    void Update()
    {
        float scale = _minimapCamera.orthographicSize / 25;
        transform.localScale = new Vector3(scale,scale,scale);
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
