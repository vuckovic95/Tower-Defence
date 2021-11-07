using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static Action StartGameAction;
    public static Action QuitGameAction;
    public static Action EndGameAction;
    public static Action<int> SetTurretAction;
    public static Action<int> ImpactAction;
    public static Action<int> EnemyDestroyedAction;
    public static Action<int> UpdateScore;
    public static Action<int> UpdateGold;
}
