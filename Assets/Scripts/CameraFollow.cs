using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Gracz
    public Vector3 offset = new Vector3(0, 2, -4); // domyślny offset
    public float smoothSpeed = 5f;  // płynność ruchu

    void LateUpdate()
    {
        // Obliczamy pozycję za graczem, bazując na jego rotacji
        Vector3 desiredPosition = target.position + target.forward * offset.z + target.up * offset.y + target.right * offset.x;

        // Płynne przejście kamery do pozycji
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Kamera zawsze patrzy na gracza
        transform.LookAt(target.position + Vector3.up * 1.5f); // punkt patrzenia lekko nad postacią
    }
}


