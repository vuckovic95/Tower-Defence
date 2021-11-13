using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class Projectile : MonoBehaviour, IProjectile
{
    [BoxGroup("Speed")]
    [SerializeField]
    private float _speed;
    [BoxGroup("Trail")]
    [SerializeField]
    private GameObject _trail;

    private Transform _transform;
    private Transform _target;
    private float _damage;

    private Vector3 _direction;
    private float _distanceThisFrame;

    void Start()
    {
        _transform = this.transform;
    }

    void Update()
    {
        if(_target == null)
        {
            TurnOff();
            return;
        }

        _direction = _target.position - _transform.position;
        _distanceThisFrame = _speed * Time.deltaTime;

        if(_direction.magnitude < _distanceThisFrame)
        {
            HitTarget();
            return;
        }

        _transform.Translate(_direction.normalized * _distanceThisFrame, Space.World);
        _transform.LookAt(_target);
    }

    public void HitTarget()
    {
        EnemyController enemy = _target.GetComponent<EnemyController>();

        if (enemy != null)
            enemy.TakeDamage(_damage);

        _trail.SetActive(false);
        Actions.ProjectileDestroyedAction?.Invoke(this);
    }

    public void TurnOff()
    {
        _trail.SetActive(false);
        Actions.ProjectileDestroyedAction?.Invoke(this);
    }

    public void Seek(Transform target, float damage)
    {
        _target = target;
        _damage = damage;
        _trail.SetActive(true);
    }

    public void ResetProjectileData()
    {
        _target = null;
        _damage = 0f;
    }
}
