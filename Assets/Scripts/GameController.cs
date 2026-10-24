using DefaultNamespace;
using Reflex.Attributes;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Inject] private readonly IGameService gameService;


    private void Start()
    {
        gameService.InitBoard(new Vector2Int(4, 4));
    }
}