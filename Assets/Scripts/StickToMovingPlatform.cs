using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class StickToMovingPlatform : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 platformVelocity;
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            Transform newPlatform = hit.collider.transform;

            if (currentPlatform != newPlatform)
            {
                currentPlatform = newPlatform;
                lastPlatformPosition = currentPlatform.position;
            }
        }
        else
        {
            currentPlatform = null;
        }
    }

    void LateUpdate()
    {
        if (currentPlatform != null)
        {
            Vector3 newPlatformPosition = currentPlatform.position;
            Vector3 platformDelta = newPlatformPosition - lastPlatformPosition;

            if (platformDelta != Vector3.zero)
            {
                controller.Move(platformDelta);
            }

            lastPlatformPosition = newPlatformPosition;
        }
    }
}
