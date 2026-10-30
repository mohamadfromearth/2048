using System;
using UnityEngine;

namespace Themes
{
    [Serializable]
    public class ThemeData
    {
        [SerializeField] private ItemColors itemColor;
        [SerializeField] private Color textColor;
        [SerializeField] private Color backgroundColor;
        [SerializeField] private Color primaryColor;
        [SerializeField] private Color secondaryColor;
        
        public ItemColors ItemColor => itemColor;
        public Color TextColor => textColor;
        public Color BackgroundColor => backgroundColor;
        public Color PrimaryColor => primaryColor;
        public Color SecondaryColor => secondaryColor;
    }
}