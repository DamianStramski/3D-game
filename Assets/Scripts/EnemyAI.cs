using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public float walkRadius = 10f;
    public float waitTime = 2f;
    public float chaseRange = 7f;
    public float playerDamageCooldown = 1.5f;
    public float enemyHitCooldown = 0.8f;
    public int maxHealth = 5;
    public float flashDuration = 0.1f;
    public int flashCount = 3;

    private NavMeshAgent agent;
    private Animator animator;
    private GameObject player;
    private float waitTimer = 0f;
    private float lastTimePlayerHit = -999f;
    private float lastTimeEnemyHit = -999f;
    private int currentHealth;
    private bool chasing = false;

    private Renderer[] renderers;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;

        renderers = GetComponentsInChildren<Renderer>();

        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"{gameObject.name} is not placed on a valid NavMesh!");
            enabled = false;
            return;
        }

        GoToRandomPoint();
    }

    void Update()
    {
        if (!agent.isOnNavMesh || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= chaseRange)
        {
            chasing = true;
            agent.SetDestination(player.transform.position);
        }
        else if (chasing && distanceToPlayer > chaseRange + 2f)
        {
            chasing = false;
            GoToRandomPoint();
        }

        if (!chasing && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                GoToRandomPoint();
                waitTimer = 0f;
            }
        }

        if (animator != null)
            animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);
    }

    void GoToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, walkRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }

    public void HandleHitFromPlayer(ControllerColliderHit hit)
    {
        Debug.Log("WYWOŁANO HandleHitFromPlayer!");

        if (!hit.gameObject.CompareTag("Player")) return;

        float now = Time.time;
        bool hitFromAbove = hit.controller.transform.position.y > transform.position.y + 0.5f;

        if (hitFromAbove && now - lastTimeEnemyHit >= enemyHitCooldown)
        {
            TakeDamage(1);
            lastTimeEnemyHit = now;

            Rigidbody rb = hit.controller.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(rb.velocity.x, 8f, rb.velocity.z);
            }
        }
        else if (!hitFromAbove && now - lastTimePlayerHit >= playerDamageCooldown)
        {
            PlayerHealth ph = hit.controller.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(1);
                lastTimePlayerHit = now;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy HP: " + currentHealth);

        StartCoroutine(FlashVisibility());

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(2);
        }
    }

    private IEnumerator FlashVisibility()
    {
        for (int i = 0; i < flashCount; i++)
        {
            SetRenderersEnabled(false);
            yield return new WaitForSeconds(flashDuration);
            SetRenderersEnabled(true);
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void SetRenderersEnabled(bool enabled)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = enabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        float now = Time.time;

        if (now - lastTimePlayerHit >= playerDamageCooldown)
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                Debug.Log("Wróg zadaje obrażenia graczowi");
                ph.TakeDamage(1);
                lastTimePlayerHit = now;
            }
        }
    }
}
