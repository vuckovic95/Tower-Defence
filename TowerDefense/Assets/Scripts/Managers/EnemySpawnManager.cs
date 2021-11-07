using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Zenject;

public class EnemySpawnManager : MonoBehaviour
{
    [Inject]
    GameManager _gameManager;

    [BoxGroup("Waypoints Parent")]
    [SerializeField]
    private Transform _waypointsParent;

    private List<Transform> _waypoints = new List<Transform>();
    private List<EnemyController> _enemies = new List<EnemyController>();

    void Start()
    {
        PopulateWaypoints();
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {

    }

    private void StartSpawning()
    {

    }

    private void SpawnEnemy()
    {

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

    public List<Transform> GetWaypoints
    {
        get { return _waypoints; }
    }
}
