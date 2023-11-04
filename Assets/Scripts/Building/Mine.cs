using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    private Resources _resources;
    public int MoneyCount = 1;
    public float PeriodOfMine = 1;

    public override void Start()
    {
        base.Start();
        _resources = FindObjectOfType<Resources>();
    }
    private void AddMoney()
    {
        _resources.Money += MoneyCount;
    }
    public override void Buided()
    {
        base.Buided();
        InvokeRepeating(nameof(AddMoney), PeriodOfMine, PeriodOfMine);
    }
}
