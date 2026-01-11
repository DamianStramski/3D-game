using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 3;
    private int currentHearts;

    public Image[] hearts; // Serduszka jako UI Image – przeciągnij w Inspector
    public GameObject startPanel;
    public GameObject pausePanel;

    private float fallStartY;
    private bool isFalling;
    public float fallDamageThreshold = 5f; // Ile jednostek musi spaść, żeby dostać obrażenia

    // Invincibility
    public float invincibilityDuration = 2f;
    public float flashInterval = 0.1f;
    private bool isInvincible = false;
    private Renderer[] renderers;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();

        // Pobierz wszystkie renderery (na wypadek, gdyby gracz miał więcej niż 1 mesh)
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        // Ukryj serca, gdy aktywny panel start lub pauzy
        bool showHearts = !(startPanel.activeSelf || pausePanel.activeSelf);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = showHearts && i < currentHearts;
        }

        // Detekcja spadania
        if (!isFalling && !IsGrounded())
        {
            fallStartY = transform.position.y;
            isFalling = true;
        }

        // Detekcja lądowania
        if (isFalling && IsGrounded())
        {
            float fallDistance = fallStartY - transform.position.y;

            if (fallDistance > fallDamageThreshold)
            {
                int damage = Mathf.FloorToInt(fallDistance / fallDamageThreshold);
                TakeDamage(damage);
            }

            isFalling = false;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            RestartGame();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHearts;
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Mapa1");
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            foreach (Renderer rend in renderers)
                rend.enabled = false;

            yield return new WaitForSeconds(flashInterval);

            foreach (Renderer rend in renderers)
                rend.enabled = true;

            yield return new WaitForSeconds(flashInterval);

            elapsed += flashInterval * 2;
        }

        isInvincible = false;
    }
}
