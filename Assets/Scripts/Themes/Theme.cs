using System;
using Game;
using UnityEngine;

namespace Themes
{
    [CreateAssetMenu(fileName = "Theme", menuName = "So/Theme")]
    public class Theme : ScriptableObject
    {
        private const string CurrentThemeIndexPrefKey = "CurrentThemeIndexPrefKey";

        [SerializeField] private ItemColors[] itemColors;
        [SerializeField] private Color[] textColors;
        [SerializeField] private Color[] backgroundColors;
        [SerializeField] private Color[] primaryColors;
        [SerializeField] private Color[] secondaryColors;
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
            var itemColor = itemColors[(int)_type];
            var t = Mathf.Clamp01(value / 100f);
            t = 1f - Mathf.Pow(1f - t, 3f);
            return Color.Lerp(itemColor.MinColor, itemColor.MaxColor, t);
        }

        public Color TextColor => textColors[(int)_type];
        public Color BackgroundColor => backgroundColors[(int)_type];
        public Color PrimaryColor => primaryColors[(int)_type];
        
        public Color SecondaryColor => secondaryColors[(int)_type];


        public void SetThemeType(ThemeType type)
        {
            _type = type;
            PlayerPrefs.SetInt(CurrentThemeIndexPrefKey, (int)type);
            OnThemeChanged?.Invoke();
        }
    }
}