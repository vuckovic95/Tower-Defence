using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;

public class EnemyController : MonoBehaviour
{
    [BoxGroup("Enemy Properties")]
    [SerializeField]
    private float _speed;

    private Transform _target;
    private int _pointIndex;

    private void Start()
    {
        SubscribeToActions();
    }

    private void FixedUpdate()
    {
        
    }
    private void SubscribeToActions()
    {

    }

    public void SetProperties()
    {
        _pointIndex = 0;
    }
}
