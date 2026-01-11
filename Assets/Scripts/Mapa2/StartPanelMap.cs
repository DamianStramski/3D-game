using UnityEngine;
using TMPro;       // konieczne, żeby używać TextMeshPro
using System.Collections;

public class StartPanelMap : MonoBehaviour
{
    public GameObject panel;          // Panel z tekstem
    public TMP_Text infoText;         // Pole tekstowe TextMeshPro
    public GameObject darkOverlay;    // Ciemne tło (Image)
    public float displayTime = 5f;

    private string startInfo = "Przeniosłeś się do krainy zła, gdzie wielki czarnoksiężnik i jego podwładni przejęli władzę i terroryzują obecne tereny. Pozbądź się ich i odzyskaj równowagę na tym świecie!";

    void Start()
    {
        StartCoroutine(ShowInfoTemporarily());
    }

    IEnumerator ShowInfoTemporarily()
    {
        darkOverlay.SetActive(true);  // włącz ciemne tło
        panel.SetActive(true);        // włącz panel
        infoText.text = startInfo;

        yield return new WaitForSeconds(displayTime);

        panel.SetActive(false);
        darkOverlay.SetActive(false); // wyłącz ciemne tło
    }
}
