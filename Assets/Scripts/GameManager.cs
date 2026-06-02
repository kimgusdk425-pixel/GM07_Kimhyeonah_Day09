using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject); //æ¿¿Ã πŸ≤ÓæÓµµ ªË¡¶µ«¡ˆ æ ∞‘ «ÿ¡‹
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        UodateScoreUI();
    }
    
    public void AddScore(int amount)
    {
        score += amount;
        UodateScoreUI();
    }
    private void UodateScoreUI()
    {
        scoreText.text = $"Score : {score}";
    }
}
