using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PushObjects : MonoBehaviour
{
    public float pushPower = 2.0f;

    private Animator animator;
    private CharacterController characterController;

    private Vector3 deferredPushback = Vector3.zero;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            Debug.LogWarning("Brak komponentu Animator na obiekcie " + gameObject.name);
        }

        if (characterController == null)
        {
            Debug.LogError("Brak komponentu CharacterController na obiekcie " + gameObject.name);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Tylko obiekty z Rigidbody i bez isKinematic
        if (body == null || body.isKinematic)
            return;

        // Sprawdź, czy obiekt ma tag Pushable
        if (!hit.collider.CompareTag("Pushable"))
            return;

        // Oblicz kierunek i siłę pchnięcia
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;

        // Uruchom animację
        if (animator != null)
        {
            animator.SetBool("IsPushing", true);
        }

        // Zapisz pushback do wykonania w Update
        deferredPushback = -hit.normal * 0.05f;
    }

    void Update()
    {
        if (!IsPushingSomething() && animator != null)
        {
            animator.SetBool("IsPushing", false);
        }

        // Wykonaj pushback i wyczyść
        if (deferredPushback != Vector3.zero)
        {
            characterController.Move(deferredPushback);
            deferredPushback = Vector3.zero;
        }
    }

    private bool IsPushingSomething()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        return Physics.Raycast(ray, 1.2f, LayerMask.GetMask("Default"));
    }
}
