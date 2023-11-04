using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack

}
public class Knight : Unit
{
    public UnitState CurrentUnitState;
    public EnemyHealth TargetEnemy;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;


    public float AttackPeriod = 1f;
    public int Damage = 1;
    float _timer;

    public override void Start()
    {
        base.Start();
        SetState(UnitState.Idle);
    }
    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        SetState(UnitState.WalkToPoint);
    }
    void Update()
    {
        if (CurrentUnitState == UnitState.Idle)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {
            FindClosestEnemy();
            if(IsTargetPointReached())
            {
                SetState(UnitState.Idle);
            }
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            if (TargetEnemy)
            {
                ResetTargetPoint();
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distance > DistanceToFollow)
                {
                    SetState(UnitState.WalkToPoint);
                }
                if (distance < DistanceToAttack)
                {
                    SetState(UnitState.Attack);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            if (TargetEnemy)
            {
                ResetTargetPoint();
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distance > DistanceToAttack)
                {
                    SetState(UnitState.WalkToEnemy);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    TargetEnemy.TakeDamage(Damage);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
    }

    private void FindClosestEnemy()
    {
        EnemyHealth[] allEnemies = FindObjectsOfType<EnemyHealth>();
        float minDistance = Mathf.Infinity;
        EnemyHealth closestEnemy = null;

        for (int i = 0; i < allEnemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allEnemies[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = allEnemies[i];
            }
        }
        if (minDistance < DistanceToFollow)
        {
            TargetEnemy = closestEnemy;
            SetState(UnitState.WalkToEnemy);
        }

    }

    public void SetState(UnitState unitState)
    {
        CurrentUnitState = unitState;

        if (CurrentUnitState == UnitState.Idle)
        {
            unitAnimator.Idle();
        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {
           
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            ResetTargetPoint();
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            ResetTargetPoint();
            _timer = 0;
        }
    }
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
