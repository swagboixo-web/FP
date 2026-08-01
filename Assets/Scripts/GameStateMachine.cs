using System.Collections;
using UnityEngine;

public enum GameState { MainMenu, Playing, Paused, BossWarning, GameOver, Victory }

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject bossWarningPanel;

    private void Awake() => Instance = this;

    private void OnEnable()
    {
        GameEvents.OnPlayerDied += () => TransitionTo(GameState.GameOver);
        GameEvents.OnBossWaveStarted += () => TransitionTo(GameState.BossWarning);
        GameEvents.OnAllWavesCleared += () => TransitionTo(GameState.Victory);
    }

    public void TransitionTo(GameState next)
    {
        CurrentState = next;

        if (victoryPanel) victoryPanel.SetActive(next == GameState.Victory);
        if (bossWarningPanel) bossWarningPanel.SetActive(next == GameState.BossWarning);

        // Auto-return to playing 3 seconds after Boss Warning
        if (next == GameState.BossWarning)
        {
            StartCoroutine(BackToPlayingAfter(3f));
        }
    }

    private IEnumerator BackToPlayingAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        TransitionTo(GameState.Playing);
    }
}
