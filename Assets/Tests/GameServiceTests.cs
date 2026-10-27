using Game;

namespace Tests
{
    using NUnit.Framework;
    using UnityEngine;
    using Vector2Int = Vector2Int;


    public class GameServiceTests
    {
        private GameService _gameService;
        private GameRepository _gameRepository;

        [SetUp]
        public void SetUp()
        {
            _gameRepository = new GameRepository();
            var gameConfig = ScriptableObject.CreateInstance<GameConfig>();
            gameConfig.boardHeight = 4;
            gameConfig.boardWidth = 4;
            gameConfig.targetScore = 2048;
            _gameService = new GameService(_gameRepository, new PlayerService(new PlayerRepository()), gameConfig);
        }

        [Test]
        public void Merge()
        {
            SetBoardForMerge();
            _gameService.Move(MoveDirection.Top);

            var getMergeResult = _gameService.GetValue(new Vector2Int(0, 3));
            

            Assert.IsTrue(getMergeResult == 4);
        }

        private void SetBoardForMerge()
        {
            _gameRepository.SetValue(new Vector2Int(0, 0), 2);
            _gameRepository.SetValue(new Vector2Int(0, 1), 2);
        }
    }
}