using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;

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

    Transform _transform;
    private TurretModel _model;
    private List<Renderer> _renderers = new List<Renderer>();
    private bool _hasUsed;
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

    private void Update()
    {
        
    }

    private void SetMaterial(Material material)
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.material = material;
        }
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
