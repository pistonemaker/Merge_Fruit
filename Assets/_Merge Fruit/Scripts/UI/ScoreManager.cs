using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int curScore;
    public int highestScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highestScoreText;

    private void OnEnable()
    {
        curScore = 0;   
        highestScore = PlayerPrefs.GetInt("HighestScore");
        scoreText.text = curScore.ToString();
        highestScoreText.text = highestScore.ToString();
        this.RegisterListener(EventID.On_Change_Score, param => UpdateScore((int) param));
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.On_Change_Score, param => UpdateScore((int) param));
    }

    private void UpdateScore(int scoreAdd)
    {
        curScore += scoreAdd;
        scoreText.text = curScore.ToString();
        
        if (curScore > highestScore)
        {
            highestScore = curScore;
            highestScoreText.text = highestScore.ToString();
            PlayerPrefs.SetInt("HighestScore", highestScore);
        }
    }
}
