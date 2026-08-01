using System;
using UnityEngine;
using static Readme;

public static class GameEvents
{
    // Gameplay & Audio Events
    public static Action OnPlayerFired;
    public static Action OnEnemyDied;
    public static Action OnPlayerHit;
    public static Action OnPlayerDied;

    // Wave & Boss Events
    public static Action<int> OnWaveStarted;
    public static Action OnWaveComplete;
    public static Action OnBossWaveStarted;
    public static Action<float> OnBossHealthChanged;
    public static Action OnBossDefeated;
    public static Action OnAllWavesCleared;
}
