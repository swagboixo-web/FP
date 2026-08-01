using Unity.Cinemachine; 
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulse;

    private void OnEnable()
    {
        GameEvents.OnPlayerHit += () => Shake(0.4f);
        GameEvents.OnEnemyDied += () => Shake(0.15f);
        GameEvents.OnPlayerDied += () => Shake(1.0f);
    }

    private void Shake(float force)
    {
        if (impulse) impulse.GenerateImpulse(force);
    }
}