using System;
using Game;
using UnityEngine;

namespace Themes
{
    [CreateAssetMenu(fileName = "Theme", menuName = "So/Theme")]
    public class Theme : ScriptableObject
    {
        private const string CurrentThemeIndexPrefKey = "CurrentThemeIndexPrefKey";

        [SerializeField] private ThemeData[] themeDataList;

        [SerializeField] private GameConfig gameConfig;
        private ThemeType _type;

        public ThemeType Type => _type;
        public event Action OnThemeChanged;

        private void OnEnable()
        {
            _type = (ThemeType)PlayerPrefs.GetInt(CurrentThemeIndexPrefKey, 0);
        }

        public Color GetItemColor(int value)
        {
            var itemColor = themeDataList[(int)_type].ItemColor;
            var t = Mathf.Clamp01(value / 100f);
            t = 1f - Mathf.Pow(1f - t, 3f);
            return Color.Lerp(itemColor.MinColor, itemColor.MaxColor, t);
        }

        public Color TextColor => themeDataList[(int)_type].TextColor;
        public Color BackgroundColor => themeDataList[(int)_type].BackgroundColor;
        public Color PrimaryColor => themeDataList[(int)_type].PrimaryColor;

        public Color SecondaryColor => themeDataList[(int)_type].SecondaryColor;


        public void SetThemeType(ThemeType type)
        {
            _type = type;
            PlayerPrefs.SetInt(CurrentThemeIndexPrefKey, (int)type);
            OnThemeChanged?.Invoke();
        }
    }
}