using System.Collections;
using Plugins.Grid;
using Plugins.PoolStructure;
using Reflex.Attributes;
using Themes;
using UnityEngine;

namespace Game.Presentation
{
    public class GameController : MonoBehaviour
    {
        [Inject] private readonly IGameService _gameService;
        [Inject] private readonly IPlayerService _playerService;
        [SerializeField] private Pool<ItemView> itemsPool;
        [SerializeField] private GridFactory<ItemView> gridFactory;
        [SerializeField] private Theme theme;
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
            var size = _gameService.Size;
            _itemsGrid = gridFactory.Create(size.X, size.Y);
            _moveItemWaitForSeconds = new WaitForSeconds(moveItemDuration);
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
            _gameService.OnWon += OnWin;
            inputListener.OnArrowKeyDown += OnArrowClick;
            ui.AddRetryClickListener(OnRetryClick);
            ui.AddEndGameRetryClickListener(OnEndGameRetryClick);
        }

        private void OnDisable()
        {
            _gameService.OnMove -= OnMove;
            _gameService.OnMerge -= OnMerge;
            _gameService.OnWon -= OnWin;
            inputListener.OnArrowKeyDown -= OnArrowClick;
            ui.RemoveRetryClickListener(OnRetryClick);
            ui.RemoveEndGameRetryClickListener(OnEndGameRetryClick);
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
                targetItem.Value = mergeData.Value;
                targetItem.SetColor(theme.GetItemColor(targetItem.Value));
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
            item.Value = data.Value;
            item.SetColor(theme.GetItemColor(item.Value));
            item.SetPosition(_itemsGrid.GetWorldPositionCenter(data.Position.X, data.Position.Y));
        }

        private void OnWin()
        {
            ui.SetEndGameText("You won!");
            ui.ShowEndGame();
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

            IEnumerator SpawnItemWithDelay(SpawnData data)
            {
                yield return _moveItemWaitForSeconds;
                OnSpawn(data);
            }
        }


        private void OnRetryClick()
        {
            _gameService.Retry();
            var size = _gameService.Size;

            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    var item = _itemsGrid.GetValue(x, y);
                    if (item == null) continue;
                    itemsPool.BackToPool(item);
                    _itemsGrid.SetValue(x, y, null);
                }
            }

            SpawnFirstItems();
            ui.SetScoreText("0");
        }


        private void OnEndGameRetryClick()
        {
            ui.HideEndGame();
            OnRetryClick();
        }

        #endregion
    }
}