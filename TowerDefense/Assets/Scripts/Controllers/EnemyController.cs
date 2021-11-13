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

    public float _speed;
    public float _health;
    public float _damage;
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
        _model = GetComponent<EnemyModel>();
        SubscribeToActions();
        ResetProperties();
    }

    private void Update()
    {
        if (this.gameObject.activeSelf && States.GameStateReference is States.GameState.Play)
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
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }

    private void IncreaseEnemyAttributes()
    {
        _speed = _model.Speed + _model.Speed / 10;
        _damage = _model.Damage + _model.Damage / 10;
        _health = _model.Health + _model.Health / 10;
    }

    private void Die()
    {
        Actions.EnemyDestroyedAction?.Invoke(_pointsToGive, this);
        this.gameObject.SetActive(false);      
    }

    private void ResetProperties()
    {
        _speed = _model.Speed;
        _health = _model.Health;
        _damage = _model.Damage;
        _pointsToGive = _model.PointsToGive;

        _healthBar.color = Color.green;
        _healthBar.fillAmount = 1;
    }

    private void UpdateHealthBar(float health)
    {
        if (health >= 80)
            _healthBar.color = Color.green;
        else if (health < 80 && health > 30)
            _healthBar.color = new Color(255, 165, 0);
        else if (health < 30)
            _healthBar.color = Color.red;
    }

    public void TakeDamage(float healthToDecrease)
    {
        _health -= healthToDecrease;
        _healthBar.fillAmount = _health / 100;
        UpdateHealthBar(_health);

        if (_health <= 0)
        {
            Die();
        }
    }

    public void SpawnEnemy(List<Transform> waypoints)
    {
        this.gameObject.SetActive(true);
        _waypoints = waypoints;
        _pointIndex = 0;
        _target = _waypoints[0];
        _transform.position = _target.position;
    }
}
