using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public BuildingPlacer buildingPlacer;
    public GameObject BuildingPrefab;
    public Text PriceText;

    private void Start()
    {
        PriceText.text = BuildingPrefab.GetComponent<Building>().Price.ToString();
    }
    public void TryBuy()
    {
        int price = BuildingPrefab.GetComponent<Building>().Price;
        if(FindObjectOfType<Resources>().Money >= price)
        {
            buildingPlacer.CreateBuilding(BuildingPrefab);
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
    }
}
