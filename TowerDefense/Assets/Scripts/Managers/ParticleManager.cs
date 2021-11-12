using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleManager : MonoBehaviour
{
    [Inject]
    PoolManager _poolManager;
    
    void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.EnemyDestroyedAction += PlayDestroyEnemy;
    }

    private void PlayDestroyEnemy(int points, EnemyController enemy)
    {
        ParticleSystem particle = _poolManager.GetDestroyEnemy();
        particle.transform.position = enemy.transform.position;
        particle.GetComponent<ParticleSystemRenderer>().material = enemy.GetComponent<Renderer>().material;
        particle.Play(false);
    }
}
