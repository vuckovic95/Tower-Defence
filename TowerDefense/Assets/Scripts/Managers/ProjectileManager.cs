using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using Zenject;

public class ProjectileManager : MonoBehaviour
{
    [Inject]
    PoolManager _poolManager;

    private List<Transform> _projectiles = new List<Transform>();
    private Transform _projectile;

    void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetProjectiles;
        Actions.ToMenuAction += ResetProjectiles;
        Actions.ProjectileDestroyedAction += RemoveProjectile;
    }

    private void RemoveProjectile(Projectile projectile)
    {
        if (_projectiles.Count == 0) return;

        projectile.ResetProjectileData();
        projectile.gameObject.SetActive(false);
        _projectiles.Remove(projectile.transform);
    }

    private void ResetProjectiles()
    {
        foreach (Transform projectile in _projectiles)
        {
            projectile.gameObject.SetActive(false);
        }

        _projectiles.Clear();
    }

    public Transform GetProjectile()
    {
        _projectile = _poolManager.GetProjectile();
        _projectiles.Add(_projectile);

        return _projectile;
    }
}
