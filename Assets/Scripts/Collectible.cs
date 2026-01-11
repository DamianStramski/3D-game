using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum ItemType { Coin, Diamond }
    public ItemType itemType;
    public int value = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManagerUI scoreManager = FindObjectOfType<ScoreManagerUI>();
            if (scoreManager != null)
            {
                switch (itemType)
                {
                    case ItemType.Coin:
                        scoreManager.AddCoin(value);
                        break;
                    case ItemType.Diamond:
                        scoreManager.AddDiamond(value);
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}

