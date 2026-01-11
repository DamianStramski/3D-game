using UnityEngine;

public class PlatformActivator : MonoBehaviour
{
    public TriggeredPlatformMovement platformScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Trigger zadziałał z graczem: {other.name}");
            platformScript.Activate();
        }
    }
}

