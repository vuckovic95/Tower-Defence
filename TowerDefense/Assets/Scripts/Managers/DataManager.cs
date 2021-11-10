using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int _score;
    private int _highScore;
    private int _gold;

    private void Start()
    {
        LoadHighScore();
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += ResetScore;
        Actions.StartGameAction += ResetGold;
        Actions.EnemyDestroyedAction += IncreaseScore;
        Actions.EnemyDestroyedAction += IncreaseGold;
        Actions.EndGameAction += CheckIsHighScore;
        Actions.SetTurretAction += DecreaseGold;
    }

    private void IncreaseScore(int scoreToEncrease, EnemyController instigator)
    {
        _score += scoreToEncrease;

        Actions.UpdateScore?.Invoke(_score);
    }

    private void IncreaseGold(int goldToIncrease, EnemyController instigator)
    {
        _gold += goldToIncrease;

        Actions.UpdateGold(_gold);
    }

    private void DecreaseGold(int goldToDecrease)
    {
        _gold -= goldToDecrease;

        Actions.UpdateGold?.Invoke(_gold);
    }

    private void ResetScore()
    {
        _score = 0;
    }

    private void ResetGold()
    {
        _gold = 0;
    }

    private void CheckIsHighScore()
    {
        if(_score > _highScore)
        {
            _highScore = _score;

            PlayerPrefs.SetInt("HighScore", _highScore);
        }
    }

    private void LoadHighScore()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public int GetGold
    {
        get { return _gold; }
    }

    public int GetScore
    {
        get { return _score; }
    }

    public int GetHighScore
    {
        get { return _highScore; }
    }
}
