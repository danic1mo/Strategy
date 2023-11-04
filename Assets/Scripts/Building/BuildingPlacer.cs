using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float cellSize = 1f;
    public Camera RaycastCamera;

    Plane _plane;

    public Building CurrentBuilding;

    public Dictionary<Vector2Int, Building> BuildingDictionary = new Dictionary<Vector2Int, Building>();
    void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
        
    }
    void Update()
    {
        if (CurrentBuilding == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && CurrentBuilding != null)
        {
            Destroy(CurrentBuilding.gameObject);
            CurrentBuilding = null;
            return;
        }
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / cellSize;

        int x = Mathf.RoundToInt(point.x);
        int z  = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * cellSize;
        if (CheckAllow(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAcceptablePosition();

            if (Input.GetMouseButtonDown(0))
            {
                InitiallBuilding(x, z, CurrentBuilding);
                CurrentBuilding = null;
            }
        }
        else
        {
            CurrentBuilding.DisplayUnacceptablePosition();
        }

    }
    private bool CheckAllow(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.Xsize; x++)
        {
            for (int z = 0; z < building.Zsize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (BuildingDictionary.ContainsKey(coordinate))
                {
                    return false;
                }
            }
        }
        return true;
    }
    void InitiallBuilding(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.Xsize; x++)
        {
            for (int z = 0; z < building.Zsize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                BuildingDictionary.Add(coordinate, building);
            }
        }

        building.CurrentBuildingState = BuildingState.Placed;
        //building.Buided();
        foreach(var item in BuildingDictionary)
        {
            Debug.Log(item);
        }
    }
    public void CreateBuilding(GameObject buildingPrefab)
    {
        GameObject newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding.GetComponent<Building>();
        
    }
}
