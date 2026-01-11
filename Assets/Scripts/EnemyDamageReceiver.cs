using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    public EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Wróg został uderzony przez gracza (trigger)");

            if (enemyAI != null)
            {
                enemyAI.TakeDamage(1);
            }
            else
            {
                Debug.LogWarning("Brak przypisania EnemyAI do EnemyDamageReceiver");
            }
        }
    }
}
