using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [BoxGroup("Health")]
    [SerializeField]
    private Image _healthImage;

    private PlayerModel _model;
    private float _health;

    private void Start()
    {
        _model = GetComponent<PlayerModel>();
        SubscribeToActions();
    }
    private void Update()
    {

    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetPlayer;
        Actions.ToMenuAction += ResetPlayer;
        Actions.ImpactAction += UpdateHealth;
    }  

    private void ResetPlayer()
    {
        _health = _model.Health;
        _healthImage.fillAmount = 1;
    }

    private void UpdateHealth(float healthToDecrease)
    {
        _health -= healthToDecrease;
        _healthImage.fillAmount = _health / 100;

        if (_health <= 0)
            Actions.EndGameAction?.Invoke();
    }
}
