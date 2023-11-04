using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public Transform Spawn;
    public float CreationPeriod;
    public GameObject EnemyPrefab;

    float _timer;
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= CreationPeriod)
        {
            _timer = 0f;
            Instantiate(EnemyPrefab, Spawn.position, Spawn.rotation);
        }
    }
}
