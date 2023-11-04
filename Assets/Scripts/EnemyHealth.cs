using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int Health;
    int _maxHealth;
    public GameObject HealthBar;
    HealthBar _healthBar;
    private void Start()
    {
        _maxHealth = Health;
        _healthBar = HealthBar.GetComponent<HealthBar>();
    }
    
    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
           //Die
            Destroy(gameObject);

        }
    }

}
