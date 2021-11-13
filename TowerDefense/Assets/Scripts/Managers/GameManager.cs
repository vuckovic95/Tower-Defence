using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using System;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        SetAppSettings();
    }

    private void SetAppSettings()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.StartGameAction += StartGame;
        Actions.EndGameAction += EndGame;
        Actions.ToMenuAction += ToMenu;
    }

    private void SwitchGameState(string state)
    {
        switch (state)
        {
            case "Menu":
                States.GameStateReference = States.GameState.Menu;
                return;
            case "Play":
                States.GameStateReference = States.GameState.Play;
                return;
            case "End":
                States.GameStateReference = States.GameState.End;
                return;
        }
    }

    private void StartGame()
    {
        SwitchGameState("Play");
    }

    private void EndGame()
    {
        SwitchGameState("End");
    }

    private void ToMenu()
    {
        SwitchGameState("Menu");
    }
}
