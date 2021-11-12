using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static Action StartGameAction;
    public static Action QuitGameAction;
    public static Action EndGameAction;
    public static Action ToMenuAction;
    public static Action<int> SetTurretAction;
    public static Action<float> ImpactAction;
    public static Action<int, EnemyController> EnemyDestroyedAction;
    public static Action<Projectile> ProjectileDestroyedAction;
    public static Action<int> UpdateScore;
    public static Action<int> UpdateGold;
    public static Action IncreaseEnemiesAttributes;
    public static Action<EnemyController> EnemySpawnedAction;
    public static Action BeginDragTurret;
    public static Action EndDragTurret;
}
