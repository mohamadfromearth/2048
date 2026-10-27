using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ThemeSettings
{
    [Serializable]
    public class ThemeSettingsUI
    {
        [SerializeField] private GameObject[] lockImages;

        [SerializeField] private GameObject panel;
        [SerializeField] private Button showButton;
        [SerializeField] private Button backButton;


        public void SetLockImagesActive(int index, bool active) => lockImages[index].SetActive(active);

        public void Show() => panel.SetActive(true);
        public void Hide() => panel.SetActive(false);


        public void AddShowClickListener(UnityAction action) => showButton.onClick.AddListener(action);

        public void RemoveShowClickListener(UnityAction action) => showButton.onClick.RemoveListener(action);

        public void AddBackClickListener(UnityAction action) => backButton.onClick.AddListener(action);
        public void RemoveBackClickListener(UnityAction action) => backButton.onClick.RemoveListener(action);
    }
}