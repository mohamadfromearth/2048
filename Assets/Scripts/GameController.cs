using Plugins.Grid;
using Reflex.Attributes;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Inject] private readonly IGameService gameService;


    [SerializeField] private Pool<ItemView> itemsPool;
    [SerializeField] private GridFactory<ItemView> gridFactory;

    private GridSystem2D<ItemView> itemsGrid;

    [SerializeField] private GameObject itemBackgroundPrefab;
    [SerializeField] private UI ui;
    [SerializeField] private Camera camera;
    [SerializeField] private InputListener inputListener;


    private void Start()
    {
        var size = new Vector2Int(4, 4);
        itemsGrid = gridFactory.Create(size.X, size.Y);
        gameService.InitBoard(size);
        SpawnItemBackground(size);
        SpawnFirstItems();
        SetUpCamera();
    }

    private void OnEnable()
    {
        gameService.OnMove += OnMove;
        gameService.OnMerge += OnMerge;
        gameService.OnSpawn += OnSpawn;
        inputListener.OnArrowKeyDown += OnArrowClick;
    }

    private void OnDisable()
    {
        gameService.OnMove -= OnMove;
        gameService.OnMerge -= OnMerge;
        gameService.OnSpawn -= OnSpawn;
        inputListener.OnArrowKeyDown -= OnArrowClick;
    }

    private void SetUpCamera()
    {
        var center = itemsGrid.GetGridCenter();
        center.z = -10;
        camera.transform.position = center;
    }

    private void SpawnItemBackground(Vector2Int size)
    {
        for (int x = 0; x < size.X; x++)
        {
            for (int y = 0; y < size.Y; y++)
            {
                var placeHolder = Instantiate(itemBackgroundPrefab);
                placeHolder.transform.position = itemsGrid.GetWorldPositionCenter(x, y);
            }
        }
    }

    private void SpawnFirstItems()
    {
        var newItems = gameService.SpawnNewValues(2);

        foreach (var itemData in newItems)
        {
            var item = itemsPool.Get();
            item.SetText(itemData.Value.ToString());
            itemsGrid.SetValue(itemData.Position.X, itemData.Position.Y, item);
            item.SetPosition(itemsGrid.GetWorldPositionCenter(itemData.Position.X, itemData.Position.Y));
        }
    }


    #region Events

    private void OnMerge(MoveData mergeData)
    {
        var item = itemsGrid.GetValue(mergeData.From.X, mergeData.From.Y);
        itemsGrid.SetValue(mergeData.From.X, mergeData.From.Y, null);
        itemsPool.BackToPool(item);
        var targetItem = itemsGrid.GetValue(mergeData.To.X, mergeData.To.Y);
        targetItem.SetText(mergeData.Value.ToString());
    }

    private void OnMove(MoveData moveData)
    {
        var item = itemsGrid.GetValue(moveData.From.X, moveData.From.Y);
        var to = itemsGrid.GetWorldPositionCenter(moveData.To.X, moveData.To.Y);
        item.SetPosition(to);
        itemsGrid.SetValue(moveData.From.X, moveData.From.Y, null);
        itemsGrid.SetValue(moveData.To.X, moveData.To.Y, item);
    }

    private void OnSpawn(SpawnData data)
    {
        var item = itemsPool.Get();
        item.SetText(data.Value.ToString());
        itemsGrid.SetValue(data.Position.X, data.Position.Y, item);
        item.SetPosition(itemsGrid.GetWorldPositionCenter(data.Position.X, data.Position.Y));
    }

    private void OnArrowClick(KeyCode keyCode)
    {
        if (gameService.Move(keyCode.ToMoveDirection()))
        {
            var itemDataList = gameService.SpawnNewValues(1);

            if (itemDataList.Count > 0)
            {
                var itemData = itemDataList[0];
                var item = itemsPool.Get();
                item.SetText(itemData.Value.ToString());
                itemsGrid.SetValue(itemData.Position.X, itemData.Position.Y, item);
                item.SetPosition(itemsGrid.GetWorldPositionCenter(itemData.Position.X, itemData.Position.Y));
            }

            if (gameService.CanMove() == false)
            {
                ui.SetEndGameText("You lost!");
                ui.ShowEndGame();
            }
        }
    }

    #endregion
}