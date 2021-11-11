using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Zenject;
using System;
using System.Threading;

public class EnemySpawnManager : MonoBehaviour
{
    [Inject]
    PoolManager _poolManager;

    [BoxGroup("Waypoints Parent")]
    [SerializeField]
    private Transform _waypointsParent;

    [BoxGroup("Wave Properties")]
    [SerializeField]
    private int _enemiesPerWave;
    [BoxGroup("Wave Properties")]
    [SerializeField]
    private float _waitForNextEnemy;

    private List<Transform> _waypoints = new List<Transform>();
    private List<EnemyController> _enemies = new List<EnemyController>();
    private EnemyController _currentEnemy;
    private bool _hasFirstWave;
    private int _wavesCounter;
    private int _enemiesPerWaveHelper;

    private void Awake()
    {
        PopulateWaypoints();
    }

    private void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetProperties;
        Actions.ToMenuAction += ResetProperties;
        Actions.StartGameAction += StartSpawning;
        Actions.EnemyDestroyedAction += CheckEnemies;
    }

    private void StartSpawning()
    {
        //Svaki treci talas se broj neprijatelja povecava za 1
        _wavesCounter++;
        if(_wavesCounter % 3 == 0)
        {
            _enemiesPerWaveHelper++; 
            Actions.IncreaseEnemiesAttributes?.Invoke();
            _wavesCounter = 0;
        }

        StartCoroutine(SpawnEnemies(_waitForNextEnemy, () => SpawnEnemy()));

        if (!_hasFirstWave)
            _hasFirstWave = true;
    }

    private void SpawnEnemy()
    {
        int random = UnityEngine.Random.Range(0, 2);

        if(random == 0)
        {
            _currentEnemy = _poolManager.GetEnemy_1().GetComponent<EnemyController>();
        }
        else
        {
            _currentEnemy = _poolManager.GetEnemy_2().GetComponent<EnemyController>();
        }

        _enemies.Add(_currentEnemy);
        Actions.EnemySpawnedAction?.Invoke(_currentEnemy);
        _currentEnemy.SetWaypoints = _waypoints;
        _currentEnemy.SpawnEnemy();
    }

    private void PopulateWaypoints()
    {
        foreach (Transform waypoint in _waypointsParent)
        {
            _waypoints.Add(waypoint);
        }
    }

    private void TurnOffAllEnemies()
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        _enemies.Clear();
    }

    private void CheckEnemies(int points, EnemyController instigator)
    {
        _enemies.Remove(instigator);

        if (_enemies.Count == 0)
            StartSpawning();
    }

    private void ResetProperties()
    {
        StopAllCoroutines();

        foreach (EnemyController enemy in _enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        _enemies.Clear();
        _currentEnemy = null;
        _hasFirstWave = false;
        _wavesCounter = 0;
        _enemiesPerWaveHelper = _enemiesPerWave;
    }

    public IEnumerator SpawnEnemies(float time, Action action = null)
    {
        while(_enemies.Count < _enemiesPerWaveHelper)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }      
    }

    public List<EnemyController> Enemies
    {
        get { return _enemies; }
    }

    public List<Transform> GetWaypoints
    {
        get { return _waypoints; }
    }

    public bool HasFirstWave
    {
        get { return _hasFirstWave; }
        set { _hasFirstWave = value; }
    }
}
