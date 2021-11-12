using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;
using System;

public class TurretCreator : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [Inject]
    PoolManager _poolManager;
    [Inject]
    GridManager _gridManager;
    [Inject]
    DataManager _dataManager;
    [Inject]
    EnemySpawnManager _enemySpawnManager;
    [Inject]
    ProjectileManager _projectileManager;

    [BoxGroup("Turret Type")]
    [SerializeField]
    private States.TurretType _turretType;

    [BoxGroup("Price")]
    [SerializeField]
    private int _price;

    private List<GameObject> _turrets = new List<GameObject>();
    private TurretController _currentTurret;
    private Transform _hitedCell;
    private RaycastHit _raycastHit;
    private Ray _ray;

    private float SMALL_MEDIUM_TURRET_Y_OFFSET = 1;
    private float BIG_TURRET_Y_OFFSET = 1.5f;

    void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetData;
        Actions.ToMenuAction += ResetData;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Actions.BeginDragTurret?.Invoke();

        //if (_dataManager.GetGold >= _price)
            GetTurret(_turretType);
        //else
           // _currentTurret = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentTurret == null) return;

        CheckHit(OnDragTrue, OnDragFalse);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Actions.EndDragTurret?.Invoke();

        if (_currentTurret == null) return;

        CheckHit(OnEndDragTrue, OnEndDragFalse);
    }

    private void CheckHit(Action hitAction = null, Action notHitAction = null)
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _raycastHit))
        {
            if (_raycastHit.transform.CompareTag("Cell"))
            {
                _hitedCell = _raycastHit.transform;
                hitAction?.Invoke();
            }
            else
            {
                _hitedCell = null;
                notHitAction?.Invoke();
            }
        }
    }

    private void GetTurret(States.TurretType type)
    {
        switch (type)
        {
            case States.TurretType.Small:
                _currentTurret = _poolManager.GetSmallTurret().GetComponent<TurretController>();
                break;
            case States.TurretType.Medium:
                _currentTurret = _poolManager.GetMediumTurret().GetComponent<TurretController>();
                break;
            case States.TurretType.Big:
                _currentTurret = _poolManager.GetBigTurret().GetComponent<TurretController>();
                break;
        }

        _currentTurret.SetManagers(_projectileManager, _enemySpawnManager);

        if (_currentTurret.HasUsed)
            GetTurret(type);
        else
            _currentTurret.gameObject.SetActive(true);
    }

    private void OnDragTrue()
    {
        SetTurretPosition(_hitedCell);

        if (!_gridManager.GridDictionary[_hitedCell.gameObject])
            _currentTurret.ChangeColorAndAlphaWhenDragging(true);
        else
            _currentTurret.ChangeColorAndAlphaWhenDragging(false);
    }

    private void OnDragFalse()
    {
        if (_currentTurret == null || _raycastHit.transform == null) return;

        _currentTurret.transform.position = _raycastHit.point;
        _currentTurret.ChangeColorAndAlphaWhenDragging(false);
    }

    private void OnEndDragTrue()
    {
        if (!_gridManager.GridDictionary[_hitedCell.gameObject])
        {
            _turrets.Add(_currentTurret.gameObject);
            SetTurretPosition(_hitedCell);
            _currentTurret.HasUsed = true;
            _currentTurret.CanFire = true;
            _gridManager.GridDictionary[_hitedCell.gameObject] = true;

            Actions.SetTurretAction?.Invoke(_price);
        }
        else
        {
            _currentTurret.gameObject.SetActive(false);
        }
    }

    private void OnEndDragFalse()
    {
        _currentTurret.gameObject.SetActive(false);
    }

    private void SetTurretPosition(Transform transform)
    {
        switch (_turretType)
        {
            case States.TurretType.Small:
            case States.TurretType.Medium:
                _currentTurret.SetTurretPosition(new Vector3(transform.position.x, transform.position.y + SMALL_MEDIUM_TURRET_Y_OFFSET, transform.position.z));
                break;
            case States.TurretType.Big:
                _currentTurret.SetTurretPosition(new Vector3(transform.position.x, transform.position.y + BIG_TURRET_Y_OFFSET, transform.position.z));
                break;
        }
    }

    private void ResetData()
    {
        foreach (GameObject turret in _turrets)
        {
            turret.SetActive(false);
        }

        _currentTurret = null;
        _turrets.Clear();
    }
}
