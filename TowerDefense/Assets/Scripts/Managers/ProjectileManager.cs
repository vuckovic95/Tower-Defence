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

    private List<Transform> _playerProjectiles = new List<Transform>();
    private Transform _playerProjectile;

    void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetProjectiles;
        Actions.ToMenuAction += ResetProjectiles;
        Actions.ProjectileDestroyedAction += RemoveProjectile;

        Actions.StartGameAction += ResetPlayerProjectiles;
        Actions.ToMenuAction += ResetPlayerProjectiles;
        Actions.PlayerProjectileDestroyedAction += RemovePlayerProjectile;
    }

    private void RemoveProjectile(Projectile projectile)
    {
        if (_projectiles.Count == 0) return;

        projectile.ResetProjectileData();
        projectile.gameObject.SetActive(false);
        _projectiles.Remove(projectile.transform);
    }

    private void RemovePlayerProjectile(PlayerProjectile projectile)
    {
        if (_playerProjectiles.Count == 0) return;

        projectile.ResetProjectileData();
        projectile.gameObject.SetActive(false);
        _playerProjectiles.Remove(projectile.transform);
    }

    private void ResetProjectiles()
    {
        foreach (Transform projectile in _projectiles)
        {
            projectile.gameObject.SetActive(false);
        }

        _projectiles.Clear();
    }

    private void ResetPlayerProjectiles()
    {
        foreach (Transform projectile in _playerProjectiles)
        {
            projectile.gameObject.SetActive(false);
        }

        _playerProjectiles.Clear();
    }

    public Transform GetProjectile()
    {
        _projectile = _poolManager.GetProjectile();
        _projectiles.Add(_projectile);

        return _projectile;
    }

    public Transform GetPlayerProjectile()
    {
        _playerProjectile = _poolManager.GetPlayerProjectile();
        _playerProjectiles.Add(_projectile);

        return _playerProjectile;
    }
}
