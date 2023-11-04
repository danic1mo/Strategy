using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAnimator : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Idle()
    {
        _animator.SetBool("Walk", false);
    }
    public void Walk()
    {
        _animator.SetBool("Walk", true);
    }
    public void Death()
    {
        _animator.SetTrigger("Death");

        Unit unit = GetComponentInParent<Unit>();
        Enemy enemy = GetComponentInParent<Enemy>();
        NavMeshAgent navMeshAgent = GetComponentInParent<NavMeshAgent>();

        if(unit)
        {
            Destroy(unit);
        }
        if (enemy)
        {
            Destroy(enemy);
        }
        if (navMeshAgent)
        {
            Destroy(navMeshAgent);
        }
    }
    public void DestroyEvent()
    {
        Destroy(transform.parent.gameObject);
    }


   
    void Update()
    {
        
    }
}
