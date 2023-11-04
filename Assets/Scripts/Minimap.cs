using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MinimapState
{
    Hover,
    Unhover,
    Other
}
public class Minimap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Camera _mainCamera;
    private Camera _minimapCamera;

    public Camera RaycastCamera;
    private Vector3 _startPosition;
    //private Vector3 _startPositionMainCamera;
    private Vector3 _cameraStartPosition;
    private Plane _plane;

    public MinimapState CurrentMinimapState = MinimapState.Other;
    private const string tag_minimap = "MinimapCamera";
    private const string tag_ground = "Ground";
    private bool _isMoving = false;
    private GameObject _ground;
    private CameraBoundary _cameraBoundary;
    private Managment _managment;
    void Start()
    {
        _mainCamera = Camera.main;
        _minimapCamera = GameObject.FindWithTag(tag_minimap).GetComponent<Camera>();
        //_plane = new Plane(Vector3.up, Vector3.zero);
        _ground = GameObject.FindGameObjectWithTag(tag_ground);
        _cameraBoundary = _mainCamera.GetComponentInChildren<CameraBoundary>();
        _managment = GameObject.FindGameObjectWithTag("Managment").GetComponent<Managment>();
    }
    void Update()
    {
        if(CurrentMinimapState == MinimapState.Hover)
        {
            MovingMinimap();
            ScalingMinimap();
            ClickMinimap();
        }
    }

    private void ClickMinimap()
    { 
        if (Input.GetMouseButton(0))
        {
            Rect minimapRect = GetComponent<RectTransform>().rect;

            Vector3 mousePos = Input.mousePosition;
            mousePos.y -= transform.position.y;
            mousePos.x -= transform.position.x;
            _mainCamera.transform.position = new Vector3(
                mousePos.x * (_minimapCamera.orthographicSize * 2 / minimapRect.width) + 
                _minimapCamera.transform.position.x,
                _mainCamera.transform.position.y,
                mousePos.y * (_minimapCamera.orthographicSize * 2 / minimapRect.height) - 
                _cameraBoundary.offsetPositionZ + _minimapCamera.transform.position.z);

        }
    }

    private void ScalingMinimap()
    {
        _minimapCamera.orthographicSize -= Input.mouseScrollDelta.y;
        _minimapCamera.orthographicSize = Mathf.Clamp(_minimapCamera.orthographicSize, 10, 50);

        RaycastCamera.orthographicSize -= Input.mouseScrollDelta.y;
        RaycastCamera.orthographicSize = Mathf.Clamp(RaycastCamera.orthographicSize, 10, 50);
    }

    private void MovingMinimap()
    {
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        float distance;

        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        if (Input.GetMouseButtonDown(2))
        {
            _startPosition = point;
            _cameraStartPosition = _minimapCamera.transform.position;
            //_startPositionMainCamera = _mainCamera.transform.position;
            _isMoving = true;
        }
        if (Input.GetMouseButton(2) && _isMoving)
        {
            Vector3 offset = point - _startPosition;
            _minimapCamera.transform.position = _cameraStartPosition - offset;
            //_mainCamera.transform.position = _startPositionMainCamera + offset;
        }
        if (Input.GetMouseButtonUp(2))
        {
            _isMoving = false;
        }

        float groundSizeX = _ground.transform.localScale.x;
        float limitPositionX = groundSizeX/2 - _minimapCamera.orthographicSize;

        float groundSizeZ = _ground.transform.localScale.z;
        float limitPositionZ = groundSizeZ / 2 - _minimapCamera.orthographicSize;

        _minimapCamera.transform.position = new Vector3(
            Mathf.Clamp(_minimapCamera.transform.position.x, -limitPositionX, limitPositionX),
            _minimapCamera.transform.position.y,
            Mathf.Clamp(_minimapCamera.transform.position.z, -limitPositionZ, limitPositionZ));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mainCamera.GetComponent<CameraMove>().enabled = false;
        CurrentMinimapState = MinimapState.Hover;
        _mainCamera.GetComponent<CameraMove>().IsMoving = false;
        _managment.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mainCamera.GetComponent<CameraMove>().enabled = true;
        CurrentMinimapState = MinimapState.Unhover;
        _isMoving = false;
        _managment.enabled = true;
    }
}
