using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerProjectile : MonoBehaviour, IProjectile
{
    [BoxGroup("Speed")]
    [SerializeField]
    private float _speed;

    private Transform _transform;
    private float _damage;

    private EnemyController _enemy;
    private Vector3 _direction;
    private float _distanceThisFrame;
    void Start()
    {
        _transform = this._transform;
    }

    void Update()
    {

    }

    public void HitTarget()
    {
        if (_enemy != null)
            _enemy.TakeDamage(_damage);

        Actions.PlayerProjectileDestroyedAction?.Invoke(this);
    }

    public void ResetProjectileData()
    {
        _damage = 0;
    }

    public void Seek(Transform target, float damage)
    {
        _damage = damage;
    }

    public void TurnOff()
    {
        Actions.PlayerProjectileDestroyedAction?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemy = other.GetComponent<EnemyController>();
            HitTarget();
        }

        if (other.CompareTag("Boundarie"))
        {
            TurnOff();
        }
    }
}
