using Reflex.Attributes;
using Themes;
using UnityEngine;

namespace ThemeSettings
{
    public class ThemeSettingsController : MonoBehaviour
    {
        [Inject] private IPlayerService playerService;
        [SerializeField] private Theme theme;
        [SerializeField] private ThemeLockConfig lockConfig;
        [SerializeField] private ThemeSettingsUI ui;

        private const int ThemesCount = 3;


        private void OnEnable()
        {
            ui.AddShowClickListener(OnShowThemeClick);
            ui.AddBackClickListener(OnBackClick);
        }

        private void OnDisable()
        {
            ui.RemoveShowClickListener(OnShowThemeClick);
            ui.RemoveBackClickListener(OnBackClick);
        }

        private void OnShowThemeClick()
        {
            ui.Show();
            SetLocksActive();
            SetSelectedThemeActive();
        }

        private void OnBackClick() => ui.Hide();


        private void SetLocksActive()
        {
            var bestScore = playerService.GetBestScore();
            var count = lockConfig.Count;

            for (int i = 0; i < count; i++)
            {
                ui.SetLockImagesActive(i, lockConfig.IsLock(i, bestScore));
            }
        }

        private void SetSelectedThemeActive()
        {
            var themeIndex = (int)theme.Type;
            for (int i = 0; i < ThemesCount; i++)
            {
                ui.SetTickImagesActive(i, i == themeIndex);
            }
        }

        public void OnSelectTheme(int index)
        {
            var bestScore = playerService.GetBestScore();
            if (lockConfig.IsLock(index, bestScore))
            {
                return;
            }

            theme.SetThemeType((ThemeType)index);
            SetSelectedThemeActive();
        }
    }
}