using UnityEngine;
using UnityEngine.Events;

namespace Themes
{
    public class ItemViewThemeEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Color> SetColor;
        [SerializeField] private Theme theme;
        [SerializeField] private ItemView itemView;


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
            var color = theme.GetItemColor(itemView.Value);
            SetColor.Invoke(color);
        }
    }
}