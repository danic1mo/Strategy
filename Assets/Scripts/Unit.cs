using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectebleObject
{
    public NavMeshAgent NavMeshAgent;
    public int Price;
    public int Health;
    public int _maxHealth;

    public GameObject HealthBar;
    HealthBar _healthBar;


    public GameObject NavigationIndicator;
    public override void Start()
    {
        base.Start();
        _healthBar = HealthBar.GetComponent<HealthBar>();
        _maxHealth = Health;

        NavigationIndicator.SetActive(false);
        NavigationIndicator.transform.parent = null;
    }
    private void LateUpdate()
    {
        if(IsTargetPointReached())
        {
            ResetTargetPoint();
        }
    }
    public void ResetTargetPoint()
    {
        NavigationIndicator.SetActive(false);
    }
    protected private bool IsTargetPointReached()
    {
        if(NavMeshAgent.pathPending || NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance)
        {
            return false;
        }

        return !NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f;
    }
    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        NavMeshAgent.SetDestination(point);

        NavigationIndicator.SetActive(true);
        NavigationIndicator.transform.position = new Vector3(point.x, NavigationIndicator.transform.position.y, point.z);
    }
    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if(Health <= 0)
        {
            //Die
            unitAnimator.Death();
        }
    }
    private void OnDestroy()
    {
        Managment managment = FindObjectOfType<Managment>();
        if (managment)
            managment.Unselect(this);
        Destroy(NavigationIndicator);
    }
}
