using System;
using DefaultNamespace;
using Reflex.Attributes;
using UnityEngine;


[Serializable]
public class ItemViewList
{
    public ItemView[] itemViews;
}

public class GameController : MonoBehaviour
{
    [Inject] private readonly IGameService gameService;

    [SerializeField] private ItemViewList[] itemViews;


    private void Start()
    {
        gameService.InitBoard(new Vector2Int(4, 4));
        gameService.SpawnNewValue();
        gameService.SpawnNewValue();

        UpdateText();
    }


    private void OnMerge(MoveData moveData)
    {
    }

    private void OnMove(MoveData moveData)
    {
    }

    private void OnSpawn(SpawnData data)
    {
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(MoveDirection.Top);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(MoveDirection.Left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(MoveDirection.Right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(MoveDirection.Down);
        }
    }


    private void Move(MoveDirection direction)
    {
        if (gameService.Move(direction))
        {
            gameService.SpawnNewValue();
            UpdateText();
        }
    }

    private void UpdateText()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                itemViews[y].itemViews[x].SetText(gameService.GetValue(new Vector2Int(x, y)).ToString());
            }
        }
    }
}