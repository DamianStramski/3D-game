using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    public int requiredCoins = 5;
    public int requiredDiamonds = 3;

    public Transform cameraGoalPosition;
    public float cameraMoveDuration = 2f;
    public string nextSceneName = "Mapa2";

    public GameObject teleportLight;
    public GameObject messagePanel;
    public Text messageText;
    public GameObject abilityHintUI;
    public GameObject coinTextUI;
    public GameObject diamondTextUI;
    public Camera mainCamera;
    public PlayerMovement playerMovement;

    public float abilityHintDuration = 5f;

    private int collectedCoins = 0;
    private int collectedDiamonds = 0;
    private bool goalReached = false;

    void Start()
    {
        if (teleportLight != null)
            teleportLight.SetActive(false);
    }

    public void AddCoin()
    {
        collectedCoins++;
        CheckGoal();
    }

    public void AddDiamond()
    {
        collectedDiamonds++;
        CheckGoal();
    }

    void CheckGoal()
    {
        if (!goalReached && collectedCoins >= requiredCoins && collectedDiamonds >= requiredDiamonds)
        {
            goalReached = true;
            StartCoroutine(HandleGoalReached());
        }
    }

    IEnumerator HandleGoalReached()
    {
        yield return MoveCameraToGoal();

        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            if (messageText != null)
            {
                messageText.text = "Gratulacje! Zebrałeś wszystkie monety i diamenty!\n\n" +
                                   "Portal został odblokowany.\n" +
                                   "Odblokowałeś też podwójny skok – naciśnij dwa razy spację!";
            }
        }

        if (teleportLight != null)
            teleportLight.SetActive(true);

        if (abilityHintUI != null)
            abilityHintUI.SetActive(true);

        if (coinTextUI != null) coinTextUI.SetActive(false);
        if (diamondTextUI != null) diamondTextUI.SetActive(false);

        if (playerMovement != null)
        {
            playerMovement.EnableDoubleJump(true);
        }

        // ZAPISZ do GameManagera
        if (GameManager.Instance != null)
        {
            GameManager.Instance.hasDoubleJump = true;
        }

        yield return new WaitForSeconds(5f);
        if (messagePanel != null) messagePanel.SetActive(false);

        yield return new WaitForSeconds(abilityHintDuration);
        if (abilityHintUI != null) abilityHintUI.SetActive(false);
    }

    IEnumerator MoveCameraToGoal()
    {
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        float elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, cameraGoalPosition.position, elapsed / cameraMoveDuration);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, cameraGoalPosition.rotation, elapsed / cameraMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = cameraGoalPosition.position;
        mainCamera.transform.rotation = cameraGoalPosition.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (goalReached && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
