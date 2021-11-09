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

    [BoxGroup("Turret Type")]
    [SerializeField]
    private States.TurretType _turretType;

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
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GetTurret(_turretType);
    }

    public void OnDrag(PointerEventData eventData)
    {
        CheckHit(OnDragTrue, OnDragFalse);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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

        _currentTurret.gameObject.SetActive(true);
    }

    private void OnDragTrue()
    {
        SetTurretPosition(_hitedCell.position);
        _currentTurret.ChangeColorAndAlphaWhenDragging(true);
    }

    private void OnDragFalse()
    {
        if (_currentTurret == null) return;
        SetTurretPosition(_raycastHit.point);
        //_currentTurret.transform.position = _raycastHit.point;
        _currentTurret.ChangeColorAndAlphaWhenDragging(false);
    }

    private void OnEndDragTrue()
    {
        _turrets.Add(_currentTurret.gameObject);

        SetTurretPosition(_hitedCell.position);
    }

    private void SetTurretPosition(Vector3 transform)
    {
        switch (_turretType)
        {
            case States.TurretType.Small:
            case States.TurretType.Medium:
                _currentTurret.SetTurretPosition(new Vector3(transform.x, transform.y + SMALL_MEDIUM_TURRET_Y_OFFSET, transform.z));
                break;
            case States.TurretType.Big:
                _currentTurret.SetTurretPosition(new Vector3(transform.x, transform.y + BIG_TURRET_Y_OFFSET, transform.z));
                break;
        }
    }

    private void OnEndDragFalse()
    {
        _currentTurret.gameObject.SetActive(false);
    }

    private void ResetData()
    {
        _currentTurret = null;
        _turrets.Clear();
    }
}
