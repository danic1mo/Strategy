using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera RaycastCamera;
    Vector3 _startPoint;
    Vector3 _cameraStartPosition;
    Plane _plane;
    public bool IsMoving = false;
    void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        float distance;

        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        if (Input.GetMouseButtonDown(2))
        {
            _startPoint = point;
            _cameraStartPosition = transform.position;
            IsMoving = true;
        }
        if (Input.GetMouseButton(2) && IsMoving)
        {
            Vector3 offset = point - _startPoint;
            transform.position = _cameraStartPosition - offset;
        }
        if(Input.GetMouseButtonUp(2))
        {
            IsMoving = false;
        }
        //transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
        //RaycastCamera.transform.Translate(0f, 0f, Input.mouseScrollDelta.y);

        transform.position -= new Vector3(0, Input.mouseScrollDelta.y, 0);
        RaycastCamera.transform.position -= new Vector3(0, Input.mouseScrollDelta.y, 0);
        ClampValue(transform);
        ClampValue(RaycastCamera.transform);
    }

    void ClampValue(Transform transform)
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -45, 45),
            Mathf.Clamp(transform.position.y, 8, 14),
            Mathf.Clamp(transform.position.z, -51, 39)
            );
    }
}
