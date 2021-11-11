using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;

public class TurretController : MonoBehaviour
{
    [Inject]
    EnemySpawnManager _enemySpawnManager;
    [Inject]
    PoolManager _poolManager;

    [BoxGroup("Fire Bullet Position")]
    [SerializeField]
    private Transform _fireBulletPosition;

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

    [BoxGroup("Laser")]
    [SerializeField]
    private LineRenderer _laser;

    private GameObject _nearestEnemy;
    private Transform _transform;
    private Transform _target;
    private TurretModel _model;
    private float _minDistance;
    private float _distanceToEnemy;
    private float _fireCountdown = 0f;
    private EnemyController _targetEnemy;
    private bool _hasUsed;
    private bool _useLaser;
    private List<Renderer> _renderers = new List<Renderer>();

    private float TURN_SPEED = 10f;

    private void Start()                  
    {
        _transform = this.transform;
        _model = GetComponent<TurretModel>();

        Renderer renderer = GetComponent<Renderer>();

        _renderers.Add(renderer);
        _renderers.Add(_dome.GetComponent<Renderer>());
        _renderers.Add(_cannon.GetComponent<Renderer>());

        Actions.StartGameAction += () => { _hasUsed = false; };
    }

    private void OnEnable()
    {
        ResetTurret();
    }

    private void Update()
    {
        if(_target == null)
        {
            if (_useLaser)
            {
                if (_laser.enabled)
                {
                    _laser.enabled = false;
                }
            }
            return;
        }

        LookAtTarget();

        if (_useLaser)
        {
            Laser();
        }
        else
        {
            if(_fireCountdown <= 0f)
            {
                Shoot();
                _fireCountdown = 1f / _model.FireRange;
            }
            _fireCountdown -= Time.deltaTime;
        }
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

    private void Laser()
    {
        if (!_laser.enabled)
        {
            _laser.enabled = true;
        }

        _laser.SetPosition(0, _fireBulletPosition.position);
        _laser.SetPosition(1, _target.position);
    }

    private void Shoot()
    {

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
        _target = null;
        _dome.transform.rotation = Quaternion.Euler(Vector3.zero);
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
    }

    public bool HasUsed
    {
        get { return _hasUsed; }
        set { _hasUsed = value; }
    }
}
