using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform ScaleTransform;
    Transform _cameraTransform;


    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.rotation = _cameraTransform.rotation;
    }

    public void SetHealth(int health, int maxHealth)
    {
        float xScale = (float)health / maxHealth;
        xScale = Mathf.Clamp01(xScale);
        ScaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
}
