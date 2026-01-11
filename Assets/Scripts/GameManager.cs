using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool hasDoubleJump = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Nie niszcz obiektu przy zmianie sceny
        }
        else
        {
            Destroy(gameObject); // Zapobiegaj duplikatom
        }
    }
}

