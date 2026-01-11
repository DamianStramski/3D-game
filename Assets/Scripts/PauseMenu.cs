using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pausePanel; // Panel z menu pauzy
    public GameObject hud;        // HUD z licznikami monet i diamentów

    public Button RestartButton;  // Przycisk Restart
    public Button ResumeButton;   // Przycisk Wznów
    public Button QuitButton;     // Przycisk Wyjdź

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);       // Ukryj menu pauzy na starcie
        if (hud != null) hud.SetActive(true); // Pokaż HUD
        Time.timeScale = 1f;               // Upewnij się, że gra działa normalnie

        // Ręczne przypisanie akcji do przycisków
        if (RestartButton != null)
        {
            RestartButton.onClick.RemoveAllListeners();
            RestartButton.onClick.AddListener(RestartGame);
        }

        if (ResumeButton != null)
        {
            ResumeButton.onClick.RemoveAllListeners();
            ResumeButton.onClick.AddListener(ResumeGame);
        }

        if (QuitButton != null)
        {
            QuitButton.onClick.RemoveAllListeners();
            QuitButton.onClick.AddListener(QuitGame);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);     // pokaż menu pauzy
        if (hud != null) hud.SetActive(false); // ukryj HUD
        Time.timeScale = 0f;            // zatrzymaj grę
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);    // ukryj menu pauzy
        if (hud != null) hud.SetActive(true);  // pokaż HUD
        Time.timeScale = 1f;            // wznow grę
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;  // wznow czas przed restartem
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // zatrzymaj tryb Play w edytorze
#endif
    }
}
