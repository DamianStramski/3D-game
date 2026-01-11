using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float waitTime = 1.5f;

    private Vector3 target;
    private bool isWaiting = false;

    void Start()
    {
        target = pointB.position;
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            // Dopóki nie dotrzemy do celu, poruszaj platformą
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            // Po dotarciu do celu – czekaj
            yield return new WaitForSeconds(waitTime);

            // Zmień cel
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }
}

