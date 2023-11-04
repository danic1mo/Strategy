using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public GameObject unitPrefab;
    public Text PriceText;
    public Barack Barack;

    private void Start()
    {
        PriceText.text = unitPrefab.GetComponent<Unit>().Price.ToString();
    }
    public void TryBuy()
    {
        int price = unitPrefab.GetComponent<Unit>().Price;
        if (FindObjectOfType<Resources>().Money >= price)
        {
            FindObjectOfType<Resources>().Money -= price;
            Barack.CreateUnit(unitPrefab);
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
    }

}
