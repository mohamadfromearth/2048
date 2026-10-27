using System;
using UnityEngine;
using UnityEngine.Events;

namespace Themes
{
    public class ThemeEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Color> SetColor;
        [SerializeField] private Theme theme;
        [SerializeField] private ThemeColorType colorType;


        private void OnEnable()
        {
            theme.OnThemeChanged += OnThemeChanged;
            OnThemeChanged();
        }

        private void OnDisable()
        {
            theme.OnThemeChanged -= OnThemeChanged;
        }


        private void OnThemeChanged()
        {
            var color = GetColor();
            SetColor.Invoke(color);
        }

        private Color GetColor()
        {
            switch (colorType)
            {
                case ThemeColorType.Text:
                    return theme.TextColor;
                case ThemeColorType.Background:
                    return theme.BackgroundColor;
                case ThemeColorType.Primary:
                    return theme.PrimaryColor;
                case ThemeColorType.Secondary:
                    return theme.SecondaryColor;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}