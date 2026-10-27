using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[Serializable]
public class UI
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button endGameRetryButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI betScoreText;


    public void ShowEndGame() => endGamePanel.SetActive(true);

    public void HideEndGame() => endGamePanel.SetActive(false);

    public void SetEndGameText(string text) => endGameText.text = text;


    public void AddRetryClickListener(UnityAction clickAction) => retryButton.onClick.AddListener(clickAction);

    public void RemoveRetryClickListener(UnityAction clickAction) => retryButton.onClick.RemoveListener(clickAction);

    public void AddEndGameRetryClickListener(UnityAction clickAction) =>
        endGameRetryButton.onClick.AddListener(clickAction);

    public void RemoveEndGameRetryClickListener(UnityAction clickAction) =>
        endGameRetryButton.onClick.RemoveListener(clickAction);

    public void SetScoreText(string text) => scoreText.text = text;
    public void SetBetScoreText(string text) => betScoreText.text = text;
}