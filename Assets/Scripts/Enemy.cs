using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack

}
public class Enemy : MonoBehaviour
{
    public EnemyState CurrentEnemyState;

    //public int Health;
    public Building TargetBuilding;
    public Unit TargetUnit;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;
    public NavMeshAgent NavMeshAgent;


    public float AttackPeriod = 1f;
    public int Damage = 1;
    float _timer;

    //int _maxHealth;
    //public GameObject HealthBar;
    //HealthBar _healthBar;

    void Start()
    {
        SetState(EnemyState.WalkToBuilding);
        //_maxHealth = Health;
        //_healthBar = HealthBar.GetComponent<HealthBar>();
    }

    void Update()
    {
        if(CurrentEnemyState == EnemyState.Idle)
        {
            FindClosestBuilding();
            if (TargetBuilding)
            {
                SetState(EnemyState.WalkToBuilding);
            }
            FindClosestUnit();
        }
        else if(CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestUnit();
            if(TargetBuilding == null)
            {
                SetState(EnemyState.Idle);
            }
            float distance = Vector3.Distance(transform.position,
                                            TargetBuilding.transform.position);
            if(distance < DistanceToAttack)
            {
                SetState(EnemyState.Attack);
            }
        }
        else if(CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (TargetUnit)
            {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(EnemyState.WalkToBuilding);
                }
                if (distance < DistanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
        else if(CurrentEnemyState == EnemyState.Attack)
        {
            if (TargetUnit)
            {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    TargetUnit.TakeDamage(Damage);
                }
            }
            else if (TargetBuilding)
            {
                FindClosestUnit();
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    TargetBuilding.TakeDamage(Damage);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    private void FindClosestUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }
        if(minDistance < DistanceToFollow)
        {
            TargetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
        
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;

        if (CurrentEnemyState == EnemyState.Idle)
        {

        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestBuilding();
            if(TargetBuilding)
            {
                NavMeshAgent.SetDestination(TargetBuilding.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {

        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            _timer = 0;
        }
    }
    public void FindClosestBuilding()
    {
        Building[] allBuildings = FindObjectsOfType<Building>();
        float minDistance = Mathf.Infinity;
        Building closestBuildsng = null;

        for (int i = 0; i < allBuildings.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if(distance < minDistance && allBuildings[i].CurrentBuildingState == BuildingState.Placed)
            {
                minDistance = distance;
                closestBuildsng = allBuildings[i];
            }
        }
        TargetBuilding = closestBuildsng;
    }
    //public void TakeDamage(int damageValue)
    //{
    //    Health -= damageValue;
    //    _healthBar.SetHealth(Health, _maxHealth);
    //    if (Health <= 0)
    //    {
    //        //Die
    //        Destroy(gameObject);

    //    }
    //}
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
#endif
}
