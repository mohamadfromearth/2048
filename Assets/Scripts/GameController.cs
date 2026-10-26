using System.Collections;
using Plugins.Grid;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [Inject] private readonly IGameService _gameService;
    [Inject] private readonly IPlayerService _playerService;
    [SerializeField] private Pool<ItemView> itemsPool;
    [SerializeField] private GridFactory<ItemView> gridFactory;
    private GridSystem2D<ItemView> _itemsGrid;
    [SerializeField] private GameObject itemBackgroundPrefab;
    [SerializeField] private UI ui;
    [SerializeField] private Camera camera;
    [SerializeField] private InputListener inputListener;
    [SerializeField] private float moveItemDuration = 0.2f;
    [SerializeField] private float scaleUpItemDuration = 0.2f;
    [SerializeField] private Vector3 itemScale;
    private WaitForSeconds _moveItemWaitForSeconds;

    private void Start()
    {
        var size = new Vector2Int(4, 4);
        _itemsGrid = gridFactory.Create(size.X, size.Y);
        _moveItemWaitForSeconds = new WaitForSeconds(moveItemDuration);
        _gameService.InitBoard(size);
        SpawnItemBackground(size);
        SpawnFirstItems();
        SetUpCamera();
        ui.SetBetScoreText(_playerService.GetBestScore().ToString());
        ui.SetScoreText("0");
    }

    private void OnEnable()
    {
        _gameService.OnMove += OnMove;
        _gameService.OnMerge += OnMerge;
        inputListener.OnArrowKeyDown += OnArrowClick;
    }

    private void OnDisable()
    {
        _gameService.OnMove -= OnMove;
        _gameService.OnMerge -= OnMerge;
        inputListener.OnArrowKeyDown -= OnArrowClick;
    }

    private void SetUpCamera()
    {
        var center = _itemsGrid.GetGridCenter();
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
                placeHolder.transform.position = _itemsGrid.GetWorldPositionCenter(x, y);
            }
        }
    }

    private void SpawnFirstItems()
    {
        var newItems = _gameService.SpawnNewValues(2);
        foreach (var itemData in newItems) OnSpawn(itemData);
    }


    #region Events

    private void OnMerge(MoveData mergeData)
    {
        var item = _itemsGrid.GetValue(mergeData.From.X, mergeData.From.Y);
        _itemsGrid.SetValue(mergeData.From.X, mergeData.From.Y, null);
        //itemsPool.BackToPool(item);
        var targetItem = _itemsGrid.GetValue(mergeData.To.X, mergeData.To.Y);
        item.MoveTo(targetItem.transform.position, moveItemDuration, () =>
        {
            itemsPool.BackToPool(item);
            targetItem.SetText(mergeData.Value.ToString());
        });
        ui.SetScoreText(_gameService.GetScore().ToString());
        ui.SetBetScoreText(_playerService.GetBestScore().ToString());
    }

    private void OnMove(MoveData moveData)
    {
        var item = _itemsGrid.GetValue(moveData.From.X, moveData.From.Y);
        var to = _itemsGrid.GetWorldPositionCenter(moveData.To.X, moveData.To.Y);
        item.MoveTo(to, moveItemDuration);
        _itemsGrid.SetValue(moveData.From.X, moveData.From.Y, null);
        _itemsGrid.SetValue(moveData.To.X, moveData.To.Y, item);
    }

    private void OnSpawn(SpawnData data)
    {
        var item = itemsPool.Get();
        item.SetScale(Vector3.zero);
        item.ScaleTo(itemScale, scaleUpItemDuration);
        item.SetText(data.Value.ToString());
        _itemsGrid.SetValue(data.Position.X, data.Position.Y, item);
        item.SetPosition(_itemsGrid.GetWorldPositionCenter(data.Position.X, data.Position.Y));
    }

    private void OnArrowClick(KeyCode keyCode)
    {
        if (_gameService.Move(keyCode.ToMoveDirection()))
        {
            var itemDataList = _gameService.SpawnNewValues(1);

            if (itemDataList.Count > 0) StartCoroutine(SpawnItemWithDelay(itemDataList[0]));


            if (_gameService.CanMove() == false)
            {
                ui.SetEndGameText("You lost!");
                ui.ShowEndGame();
            }
        }
    }


    private IEnumerator SpawnItemWithDelay(SpawnData data)
    {
        yield return _moveItemWaitForSeconds;
        OnSpawn(data);
    }

    #endregion
}