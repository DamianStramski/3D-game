using UnityEngine;
using System.Collections;

public class TriggeredPlatformMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float waitTime = 1.5f;

    private bool isActive = false;
    private Vector3 target;
    private bool isWaiting = false;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        if (!isActive || isWaiting) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            StartCoroutine(SwitchTargetAfterWait());
        }
    }

    IEnumerator SwitchTargetAfterWait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Zmiana celu
        target = (target == pointA.position) ? pointB.position : pointA.position;

        isWaiting = false;
    }

    public void Activate()
    {
        isActive = true;
    }
}
