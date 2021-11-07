using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using MoreMountains.NiceVibrations;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {

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
        MMVibrationManager.Haptic(HapticTypes.Failure);
    }

    private void ToMenu()
    {
        SwitchGameState("Menu");
    }
}
