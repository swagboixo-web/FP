using UnityEngine;
using DG.Tweening;

public class SmartCoin : MonoBehaviour
{
    public int coinValue = 1;

    void Start()
    {
        // 1. Pop up into the air
        transform.DOJump(transform.position + Vector3.up * 2f, 1.5f, 1, 0.4f).OnComplete(() =>
        {
            // 2. Find the top corner of the screen in 3D space
            Vector3 screenTopRight = new Vector3(Screen.width * 0.85f, Screen.height * 0.9f, Camera.main.nearClipPlane + 3f);
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(screenTopRight);

            // 3. Fly to the screen and update the score
            transform.DOMove(targetPosition, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
                PlayerPrefs.SetInt("TotalCoins", currentCoins + coinValue);
                PlayerPrefs.Save();

                Destroy(gameObject);
            });
        });
    }
}