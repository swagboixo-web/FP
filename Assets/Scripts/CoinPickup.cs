using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin Value")]
    public int coinValue = 1; // Diverse: change this to 5 or 10 for rare coins!

    void OnTriggerEnter(Collider other)
    {
        // Make sure the object touching the coin has the exact Tag "Player"
        if (other.CompareTag("Player"))
        {
            // 1. Get the current total coins saved in the game memory
            int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);

            // 2. Add this coin's value to the total and save it
            PlayerPrefs.SetInt("TotalCoins", currentCoins + coinValue);
            PlayerPrefs.Save();

            // 3. Destroy the coin so it can't be collected twice
            Destroy(gameObject);
        }
    }
}