using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgressBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider progressSlider;
    public TextMeshProUGUI progressText;

    [Header("Tracked Objects")]
    public Transform player;
    public Transform bossTrigger;

    private float maxDistance;

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null && bossTrigger != null)
        {
            // Calculate starting distance to the boss arena
            maxDistance = Vector3.Distance(player.position, bossTrigger.position);
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 1f;
        }
    }

    void Update()
    {
        if (player == null || bossTrigger == null) return;

        // Current distance remaining
        float currentDistance = Vector3.Distance(player.position, bossTrigger.position);

        // Calculate 0.0 to 1.0 progress value
        float progress = 1f - Mathf.Clamp01(currentDistance / maxDistance);

        // Update Slider and Text
        progressSlider.value = progress;
        if (progressText != null)
        {
            progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }
    }
}