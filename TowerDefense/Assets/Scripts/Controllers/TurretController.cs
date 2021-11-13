using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;
using System;

public class TurretController : MonoBehaviour
{
    [BoxGroup("Turret Parts")]
    [SerializeField]
    private GameObject _dome;
    [BoxGroup("Turret Parts")]
    [SerializeField]
    private GameObject _cannon;

    [BoxGroup("Materials")]
    [SerializeField]
    private Material _turretMaterial;
    [BoxGroup("Materials")]
    [SerializeField]
    private Material _turretMaterialDragTrue;
    [BoxGroup("Materials")]
    [SerializeField]
    private Material _turretMaterialDragFalse;

    private EnemySpawnManager _enemySpawnManager;
    private ProjectileManager _projectileManager;

    private GameObject _nearestEnemy;
    private GameObject _projectileObject;
    private Transform _transform;
    private Transform _target;
    private TurretModel _model;
    private float _minDistance;
    private float _distanceToEnemy;
    private float _fireCountdown = 0f;
    private EnemyController _targetEnemy;
    private Projectile _projectile;
    private bool _hasUsed;
    private bool _canFire;
    private List<Renderer> _renderers = new List<Renderer>();

    private float TURN_SPEED = 10f;
    private float SELF_ROTATION_SPEED = 0.2f;

    private void Start()                  
    {
        _transform = this.transform;
        _model = GetComponent<TurretModel>();

        Renderer renderer = GetComponent<Renderer>();

        _renderers.Add(renderer);
        _renderers.Add(_dome.GetComponent<Renderer>());
        _renderers.Add(_cannon.GetComponent<Renderer>());

        SubscribeToActions();
    }

    private void OnEnable()
    {
        ResetTurret();
    }

    private void Update()
    {
        if(States.GameStateReference is States.GameState.Play)
        {
            if (_target == null || !_canFire)
            {
                if (_hasUsed)
                    _dome.transform.Rotate(0, SELF_ROTATION_SPEED, 0, Space.Self);
            }
            else
            {
                LookAtTarget();

                if (_fireCountdown <= 0f)
                {
                    Shoot();
                    _fireCountdown = _model.FireRange;
                }
                _fireCountdown -= Time.deltaTime;
            }
        }
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += () => { _hasUsed = false; };
    }

    private void UpdateTarget()
    {
        _nearestEnemy = null;
        _minDistance = Mathf.Infinity;

        foreach (EnemyController enemy in _enemySpawnManager.Enemies)
        {
            _distanceToEnemy = Vector3.Distance(_transform.position, enemy.transform.position);
            if(_distanceToEnemy < _minDistance)
            {
                _minDistance = _distanceToEnemy;
                _nearestEnemy = enemy.gameObject;
            }
        }

        if (_nearestEnemy != null && _minDistance <= _model.DistanceRange)
        {
            _target = _nearestEnemy.transform;
            _targetEnemy = _nearestEnemy.GetComponent<EnemyController>();
        }
        else
            _target = null;
    }

    private void LookAtTarget()
    {
        Vector3 direction = _target.position - _transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(_dome.transform.rotation, lookRotation, Time.deltaTime * TURN_SPEED).eulerAngles;
        _dome.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Shoot()
    {
        _projectileObject = _projectileManager.GetProjectile().gameObject;
        _projectileObject.transform.position = _cannon.transform.position;

        _projectile = _projectileObject.GetComponent<Projectile>();

        if(_projectile != null)
        {
            _projectile.Seek(_target, _model.Damage);
        }
    }

    private void SetMaterial(Material material)
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.material = material;
        }
    }

    private void ResetTurret()
    {
        StopAllCoroutines();
        _canFire = false;
        _target = null;
        _projectileObject = null;
        _nearestEnemy = null;
        _targetEnemy = null;
        _projectile = null;
        _dome.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _model.DistanceRange);
    }

    public void ChangeColorAndAlphaWhenDragging(bool canSet)
    {
        if (canSet)
            SetMaterial(_turretMaterialDragTrue);
        else
            SetMaterial(_turretMaterialDragFalse);
    }

    public void SetTurretPosition(Vector3 position)
    {
        _transform.position = position;
        SetMaterial(_turretMaterial);

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public void SetManagers(ProjectileManager projectileManager, EnemySpawnManager enemySpawnManager)
    {
        _enemySpawnManager = enemySpawnManager;
        _projectileManager = projectileManager;
    }

    public bool HasUsed
    {
        get { return _hasUsed; }
        set { _hasUsed = value; }
    }

    public bool CanFire
    {
        get { return _canFire; }
        set { _canFire = value; }
    }
}
