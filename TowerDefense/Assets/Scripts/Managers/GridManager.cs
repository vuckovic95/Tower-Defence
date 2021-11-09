using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;

public class GridManager : MonoBehaviour
{
    [BoxGroup("Grid Parent")]
    [SerializeField]
    private Transform _gridParent;

    private Dictionary<GameObject, bool> _gridDictionary = new Dictionary<GameObject, bool>();
    void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += PopulateGridDictionary;
    }

    private void PopulateGridDictionary()
    {
        _gridDictionary.Clear();

        foreach (Transform cell in _gridParent)
        {
            _gridDictionary.Add(cell.gameObject, false);
        }
    }

    public Dictionary<GameObject, bool> GridDictionary
    {
        get { return _gridDictionary; }
        set { _gridDictionary = value; }
    }
}
