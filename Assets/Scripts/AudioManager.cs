using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Clips")]
    [SerializeField] private AudioClip sfxShoot;
    [SerializeField] private AudioClip sfxExplosion;
    [SerializeField] private AudioClip sfxPlayerHit;

    [Header("Music")]
    [SerializeField] private AudioClip musicNormal;
    [SerializeField] private AudioClip musicBoss;
    [SerializeField] private AudioClip musicVictory;
    [SerializeField] private float crossfadeDuration = 1.5f;

    private AudioSource _musicA;
    private AudioSource _musicB;
    private AudioSource _sfxSource;

    private void Awake()
    {
        Instance = this;
        _musicA = gameObject.AddComponent<AudioSource>();
        _musicB = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();

        _musicA.loop = true;
        _musicB.loop = true;
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerFired += () => PlaySFX(sfxShoot);
        GameEvents.OnEnemyDied += () => PlaySFX(sfxExplosion);
        GameEvents.OnPlayerHit += () => PlaySFX(sfxPlayerHit);

        GameEvents.OnBossWaveStarted += () => StartCoroutine(CrossfadeTo(musicBoss));
        GameEvents.OnAllWavesCleared += () => StartCoroutine(CrossfadeTo(musicVictory));
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) _sfxSource.PlayOneShot(clip);
    }

    public IEnumerator CrossfadeTo(AudioClip next)
    {
        _musicB.clip = next;
        _musicB.Play();
        float t = 0;

        while (t < crossfadeDuration)
        {
            t += Time.deltaTime;
            float blend = t / crossfadeDuration;
            _musicA.volume = 1 - blend;
            _musicB.volume = blend;
            yield return null;
        }

        _musicA.Stop();
        (_musicA, _musicB) = (_musicB, _musicA); // Swap references
    }
}
