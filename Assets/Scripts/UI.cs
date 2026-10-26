using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[Serializable]
public class UI
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private Button retryButton;


    public void ShowEndGame() => endGamePanel.SetActive(true);

    public void HideEndGame() => endGamePanel.SetActive(false);
    
    public void SetEndGameText(string text) => endGameText.text = text;


    public void AddRetryClickListener(UnityAction clickAction) => retryButton.onClick.AddListener(clickAction);

    public void RemoveRetryClickListener(UnityAction clickAction) => retryButton.onClick.RemoveListener(clickAction);
}