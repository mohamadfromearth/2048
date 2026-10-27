using UnityEngine;

namespace ThemeSettings
{
    [CreateAssetMenu(fileName = "ThemeLockConfig", menuName = "So/ThemeLockConfig")]
    public class ThemeLockConfig : ScriptableObject
    {
        [SerializeField] private int[] requiredScores;

        public bool IsLock(int index, int score)
        {
            return requiredScores[index] > score;
        }
        
        public int Count => requiredScores.Length;
    }
}