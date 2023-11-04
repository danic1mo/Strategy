using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum BuildingState
{
    Placed,
    Purchased
}
public class Building : SelectebleObject
{
    public int Price;
    public int Xsize = 3;
    public int Zsize = 3;

    private Color _startColor;
    public Material OpaqueMaterial;
    public Material TransparentMaterial;
    public Renderer Renderer;

    public GameObject MenuObject;

    private BuildingState _currentBuildingState = BuildingState.Purchased;

    public BuildingState CurrentBuildingState
    {
        get => _currentBuildingState;

        set
        {
            _currentBuildingState = value;
            if (_currentBuildingState == BuildingState.Placed)
            {
                Buided();
            }
        }
    }
    private NavMeshObstacle _navMeshObstacle;

    public int Health;
    private int _maxHealth;

    public GameObject HealthBar;
    HealthBar _healthBar;


    private void Awake()
    {
        _startColor = Renderer.material.color;
    }
    public override void Start()
    {
        base.Start();
        UnSelect();
        _navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
        _navMeshObstacle.enabled = false;

        _maxHealth = Health;
        _healthBar = HealthBar.GetComponent<HealthBar>();
    }
    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        float cellSize = FindAnyObjectByType<BuildingPlacer>().cellSize;
        for (int x = 0; x < Xsize; x++)
        {
            for (int z = 0; z < Zsize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1, 0, 1) * cellSize);
            }
        }
    }
    public override void Select()
    {
        base.Select();
        MenuObject.SetActive(true);
    }
    public override void UnSelect()
    {
        base.UnSelect();
        MenuObject.SetActive(false);
    }
    public void DisplayUnacceptablePosition()
    {
        Renderer.material = TransparentMaterial;
        Renderer.material.color = new Color(_startColor.r,0f,0f, 0.3f);
        Debug.Log("No");
    }
    public void DisplayAcceptablePosition()
    {
        Renderer.material = TransparentMaterial;
        Renderer.material.color = new Color(0f, _startColor.g, 0f, 0.3f);
        Debug.Log("Yes");
    }
    public virtual void Buided()
    {
        FindObjectOfType<Resources>().Money -= Price;
        Renderer.material.color = _startColor;
        Renderer.material = OpaqueMaterial;
        _navMeshObstacle.enabled = true;
    }
}
