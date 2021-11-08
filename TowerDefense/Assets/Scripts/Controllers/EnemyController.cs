using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [BoxGroup("Health")]
    [SerializeField]
    private Image _healthBar;

    private float _speed;
    private float _health;
    private float _damage;
    private int _pointsToGive;

    private List<Transform> _waypoints = new List<Transform>();

    private Transform _transform;
    private Transform _target;
    private int _pointIndex;
    private EnemyModel _model;
    private Vector3 _direction;

    private void Awake()
    {
        _transform = this.transform;
    }

    private void Start()
    {
        SubscribeToActions();

        _model = GetComponent<EnemyModel>();
        _transform = this.transform;
        ResetProperties();
    }

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            _direction = _target.position - _transform.position;
            _transform.Translate(_direction.normalized * _speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(_transform.position, _target.position) <= 0.1f)
            {
                GetNextWaypoint();
            }
        }
    }
    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetProperties;
        Actions.IncreaseEnemiesAttributes += IncreaseEnemyAttributes;
    }

    private void GetNextWaypoint()
    {
        if(_pointIndex >= _waypoints.Count - 1)
        {
            Impact();
            return;
        }

        _pointIndex++;
        _target = _waypoints[_pointIndex];
    }

    private void Impact()
    {
        Actions.ImpactAction?.Invoke(_damage);      
        Die();
    }

    private void IncreaseEnemyAttributes()
    {
        _speed += _speed / 10;
        _damage += _damage / 10;
        _health += _health / 10;
    }

    private void Die()
    {
        Actions.EnemyDestroyedAction?.Invoke(_pointsToGive, this);
        this.gameObject.SetActive(false);
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }

    private void SetProperties()
    {
        this.gameObject.SetActive(true);
        _pointIndex = 0;
        _target =_waypoints[0];
        _transform.position = _target.position;
    }

    private void ResetProperties()
    {
        _speed = _model.Speed;
        _health = _model.Health;
        _damage = _model.Damage;
        _pointsToGive = _model.PointsToGive;
    }

    public void SpawnEnemy()
    {
        SetProperties();
    }

    public List<Transform> SetWaypoints
    {
        set { _waypoints = value; }
    }
}
