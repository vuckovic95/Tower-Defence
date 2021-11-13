using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class PlayerProjectile : MonoBehaviour, IProjectile
{
    [BoxGroup("Speed")]
    [SerializeField]
    private float _speed;
    [BoxGroup("Trail")]
    [SerializeField]
    private GameObject _trail;

    private Transform _transform;
    private GameObject _object;
    private float _damage;

    private EnemyController _enemy;
    void Start()
    {
        _object = this.gameObject;
        _transform = this.transform;
    }

    void Update()
    {
        if (_object.activeSelf && States.GameStateReference is States.GameState.Play)
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        _transform.Translate(Vector3.forward.normalized * _speed * Time.deltaTime, Space.Self);
    }

    public void HitTarget()
    {
        if (_enemy != null)
            _enemy.TakeDamage(_damage);

        _trail.SetActive(false);
        Actions.PlayerProjectileDestroyedAction?.Invoke(this);
    }

    public void ResetProjectileData()
    {
        _damage = 0;
    }

    public void Seek(Transform target, float damage)
    {
        _damage = damage;
        _trail.SetActive(true);
    }

    public void TurnOff()
    {
        _trail.SetActive(false);
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
