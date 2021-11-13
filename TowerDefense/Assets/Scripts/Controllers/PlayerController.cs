using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Inject]
    ProjectileManager _projectileManager;

    [BoxGroup("Health")]
    [SerializeField]
    private Image _healthImage;

    [BoxGroup("Fire Point")]
    [SerializeField]
    private Transform _firePoint;

    private PlayerModel _model;
    private GameObject _projectileObject;
    private PlayerProjectile _projectile;
    private float _health;
    private float _fireCountdown = 0f;
    private bool _canFire;

    private void Start()
    {
        _model = GetComponent<PlayerModel>();
        SubscribeToActions();
    }
    private void Update()
    {
        if (_canFire && States.GameStateReference is States.GameState.Play)
        {
            if (Input.GetMouseButton(0))
            {
                if (_fireCountdown <= 0f)
                {
                    Fire();                   
                    _fireCountdown = _model.FireRate;
                }              
            }
            _fireCountdown -= Time.deltaTime;
        }
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetPlayer;
        Actions.ToMenuAction += ResetPlayer;
        Actions.StartGameAction += () => { _canFire = true; };
        Actions.ImpactAction += UpdateHealth;
        Actions.BeginDragTurret += () => { _canFire = false; };
        Actions.EndDragTurret += () => { _canFire = true; };
    }  

    private void Fire()
    {
        _projectileObject = _projectileManager.GetPlayerProjectile().gameObject;
        _projectileObject.transform.position = _firePoint.position;
        _projectileObject.transform.rotation = _firePoint.transform.rotation;
        _projectileObject.SetActive(true);

        _projectile = _projectileObject.GetComponent<PlayerProjectile>();

        if (_projectile != null)
        {
            _projectile.Seek(null, _model.Damage);
        }
    }

    private void ResetPlayer()
    {
        _health = _model.Health;
        _healthImage.fillAmount = 1;
        _projectileObject = null;
        _projectile = null;
    }

    private void UpdateHealth(float healthToDecrease)
    {
        _health -= healthToDecrease;
        _healthImage.fillAmount = _health / 100;
        UpdateHealthBar(_health);

        if (_health <= 0)
            Actions.EndGameAction?.Invoke();
    }

    private void UpdateHealthBar(float health)
    {
        if (health >= 80)
            _healthImage.color = Color.green;
        else if (health < 80 && health > 30)
            _healthImage.color = new Color(255, 165, 0);
        else if (health < 30)
            _healthImage.color = Color.red;
    }
}
