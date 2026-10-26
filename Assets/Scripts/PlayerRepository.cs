using UnityEngine;

public class PlayerRepository : IPlayerRepository
{
    private const string BEST_SCORE_PREF_KEY = "BestScore";

    public void SetBestScore(int score) => PlayerPrefs.SetInt(BEST_SCORE_PREF_KEY, score);
    public int GetBestScore() => PlayerPrefs.GetInt(BEST_SCORE_PREF_KEY, 0);
    
}