using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : Singleton<ScoreManager>
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
        ZoomAnim();
        curScore += scoreAdd;
        scoreText.text = curScore.ToString();
        
        if (curScore > highestScore)
        {
            highestScore = curScore;
            highestScoreText.text = highestScore.ToString();
            PlayerPrefs.SetInt("HighestScore", highestScore);
        }
    }

    private void ZoomAnim()
    {
        var scale = Vector3.one;
        transform.DOScale(scale * 1.2f, 0.25f).OnComplete(() =>
        {
            transform.DOScale(scale, 0.25f);
        });
    }
}
