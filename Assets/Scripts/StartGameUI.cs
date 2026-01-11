using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject coinText;
    public GameObject diamondText;

    void Start()
    {
        Time.timeScale = 0f;
        startPanel.SetActive(true);

        // Ukryj UI zbieranych przedmiotów
        coinText.SetActive(false);
        diamondText.SetActive(false);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;

        // Pokazuj UI zbieranych przedmiotów
        coinText.SetActive(true);
        diamondText.SetActive(true);
    }

    void Update()
    {
        if (startPanel.activeSelf && Input.anyKeyDown)
        {
            StartGame();
        }
    }
}

