using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "So/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int boardWidth;
    [SerializeField] private int boardHeight;
    [SerializeField] private int targetScore;


    public Vector2Int BoardSize => new Vector2Int(boardWidth, boardHeight);
    
    public int TargetScore => targetScore;
}