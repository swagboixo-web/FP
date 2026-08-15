using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI totalCoinsText;

    [Header("Upgrade Costs")]
    public int damageCost = 10;
    public int healthCost = 15;
    public int fireRateCost = 20;

    void Update()
    {
        // Constantly updates the coin text at the top of the screen
        if (totalCoinsText != null)
        {
            totalCoinsText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
        }
    }

    // Call this from a UI Button
    public void BuyDamageUpgrade()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (coins >= damageCost)
        {
            PlayerPrefs.SetInt("TotalCoins", coins - damageCost);

            int currentDamage = PlayerPrefs.GetInt("PlayerDamage", 1);
            PlayerPrefs.SetInt("PlayerDamage", currentDamage + 1);
        }
    }

    // Call this from a UI Button
    public void BuyHealthUpgrade()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (coins >= healthCost)
        {
            PlayerPrefs.SetInt("TotalCoins", coins - healthCost);

            int currentHealth = PlayerPrefs.GetInt("PlayerMaxHealth", 100);
            PlayerPrefs.SetInt("PlayerMaxHealth", currentHealth + 20);
        }
    }

    // Call this from a UI Button
    public void BuyFireRateUpgrade()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (coins >= fireRateCost)
        {
            PlayerPrefs.SetInt("TotalCoins", coins - fireRateCost);

            // For fire rate, lower is usually faster (e.g., delay between shots)
            float currentRate = PlayerPrefs.GetFloat("PlayerFireRate", 0.5f);
            PlayerPrefs.SetFloat("PlayerFireRate", currentRate * 0.9f); // 10% faster
        }
    }
}