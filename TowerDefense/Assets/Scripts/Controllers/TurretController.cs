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

    Transform _transform;
    private Material _turretMaterial;
    private TurretModel _model;
    private List<Renderer> _renderers = new List<Renderer>();
    private void Start()
    {
        _transform = this.transform;
        _model = GetComponent<TurretModel>();

        SetMaterialProperties();
    }

    private void Update()
    {
        
    }

    private void SetMaterialProperties()
    {
        _turretMaterial = new Material(Shader.Find("Standard"));
        Renderer renderer = GetComponent<Renderer>();

        _renderers.Add(renderer);
        _renderers.Add(_dome.GetComponent<Renderer>());
        _renderers.Add(_cannon.GetComponent<Renderer>());

        _turretMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        _turretMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _turretMaterial.SetInt("_ZWrite", 0);
        _turretMaterial.DisableKeyword("_ALPHATEST_ON");
        _turretMaterial.DisableKeyword("_ALPHABLEND_ON");
        _turretMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        _turretMaterial.renderQueue = 3000;

        foreach (Renderer rend in _renderers)
        {
            rend.material = _turretMaterial;
        }
        //_turretMaterial.color = new Color(_turretMaterial.color.r, _turretMaterial.color.g, _turretMaterial.color.b, 0.1f);
    }

    public void ChangeColorAndAlphaWhenDragging(bool canSet)
    {
        if (_turretMaterial == null) return;

        if (canSet)
            _turretMaterial.color = new Color(0, 255, 0, 0.2f);
        else
            _turretMaterial.color = new Color(255, 0, 0, 0.2f);
    }

    public void SetTurretPosition(Vector3 position)
    {
        if (_turretMaterial == null) return;

        _transform.position = position;
        _turretMaterial.color = new Color(0, 255, 0, 1);
    }
}
