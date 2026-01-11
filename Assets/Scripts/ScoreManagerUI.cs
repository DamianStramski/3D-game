using UnityEngine;
using TMPro;

public class ScoreManagerUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI diamondText;

    private int coinCount = 0;
    private int diamondCount = 0;

    public GoalManager goalManager; // <-- dodane

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();

        if (goalManager != null)
            goalManager.AddCoin(); // <-- informuj GoalManager
    }

    public void AddDiamond(int amount)
    {
        diamondCount += amount;
        UpdateUI();

        if (goalManager != null)
            goalManager.AddDiamond(); // <-- informuj GoalManager
    }

    private void UpdateUI()
    {
        coinText.text = "Monety: " + coinCount;
        diamondText.text = "Diamenty: " + diamondCount;
    }
}



