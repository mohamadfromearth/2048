using System;
using System.Collections.Generic;
using Random = System.Random;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerService _playerService;
    private readonly List<Vector2Int> freeValues = new(16);
    private readonly Random _random = new();

    private readonly List<MoveDirection> directions = new()
    {
        MoveDirection.Top,
        MoveDirection.Right,
        MoveDirection.Left,
        MoveDirection.Down,
    };

    private int _score;


    public GameService(IGameRepository gameRepository, IPlayerService playerService)
    {
        _gameRepository = gameRepository;
        _playerService = playerService;
    }

    public event Action<MoveData> OnMove;
    public event Action<MoveData> OnMerge;

    public void InitBoard(Vector2Int size) => _gameRepository.InitBoard(size);


    public bool Move(MoveDirection direction)
    {
        var current = GetStart(direction);
        var offset = GetOffset(direction);
        var endOffset = GetEndOffset(direction);
        bool isMergeConsumed = false;
        bool result = false;

        while (true)
        {
            var next = GetNextFilledPosition(current, offset);

            if (IsOutOfBounds(next))
            {
                current = next - offset + endOffset;
                isMergeConsumed = false;
                if (IsOutOfBounds(current))
                {
                    return result;
                }

                continue;
            }

            var value = _gameRepository.GetValue(current);
            var nextValue = _gameRepository.GetValue(next);


            if (isMergeConsumed == false && CanMerge(value, nextValue))
            {
                var mergeValue = value + nextValue;
                _gameRepository.SetValue(current, mergeValue);
                _gameRepository.SetValue(next, 0);
                isMergeConsumed = true;
                result = true;

                _playerService.SetBestScore(mergeValue);

                if (_score < mergeValue)
                {
                    _score = mergeValue;
                    _gameRepository.SetScore(_score);
                }


                OnMerge(new MoveData()
                {
                    From = next,
                    To = current,
                    Value = mergeValue,
                });


                continue;
            }

            if (IsFree(value))
            {
                _gameRepository.SetValue(current, nextValue);
                _gameRepository.SetValue(next, 0);
                result = true;
            }
            else
            {
                current += offset;
                _gameRepository.SetValue(next, 0);
                _gameRepository.SetValue(current, nextValue);
                if (current != next) result = true;
            }

            OnMove(new MoveData() { From = next, To = current });
        }
    }

    public bool CanMove()
    {
        foreach (var moveDirection in directions)
        {
            if (CanMove(moveDirection)) return true;
        }

        return false;
    }

    private bool CanMove(MoveDirection direction)
    {
        var current = GetStart(direction);
        var offset = GetOffset(direction);
        var endOffset = GetEndOffset(direction);
        var currentValue = _gameRepository.GetValue(current);
        if (IsFree(currentValue)) return true;


        while (true)
        {
            var next = current + offset;

            if (IsOutOfBounds(next))
            {
                current = next - offset + endOffset;
                next = current + offset;

                if (IsOutOfBounds(current))
                {
                    return false;
                }
            }

            currentValue = _gameRepository.GetValue(current);
            var nextValue = _gameRepository.GetValue(next);

            if (IsFree(currentValue)) return true;
            if (IsFree(nextValue)) return true;
            if (currentValue == nextValue) return true;
            current = next;
        }
    }


    private List<SpawnData> _spawnedData = new(10);

    public List<SpawnData> SpawnNewValues(int count)
    {
        freeValues.Clear();
        var boardSize = _gameRepository.GetBoardSize();


        for (int x = 0; x < boardSize.X; x++)
        {
            for (int y = 0; y < boardSize.Y; y++)
            {
                var pos = new Vector2Int(x, y);
                var value = _gameRepository.GetValue(pos);
                if (value == 0)
                {
                    freeValues.Add(new Vector2Int(x, y));
                }
            }
        }

        _spawnedData.Clear();


        for (int i = 0; i < count; i++)
        {
            if (freeValues.Count > 0)
            {
                var index = _random.Next(0, freeValues.Count);
                var pos = freeValues[index];
                var value = 2;
                _gameRepository.SetValue(pos, value);

                _spawnedData.Add(new SpawnData()
                {
                    Position = pos,
                    Value = value,
                });
                freeValues.RemoveAt(index);
            }
            else
            {
                return _spawnedData;
            }
        }

        return _spawnedData;
    }

    public int GetScore() => _gameRepository.GetScore();


    public int GetValue(Vector2Int position) => _gameRepository.GetValue(position);

    private Vector2Int GetNextFilledPosition(Vector2Int position, Vector2Int offset)
    {
        var next = position + offset;
        if (IsOutOfBounds(next))
        {
            return next;
        }

        var nextValue = _gameRepository.GetValue(next);

        while (IsFree(nextValue) && IsOutOfBounds(next) == false)
        {
            nextValue = _gameRepository.GetValue(next);

            if (IsFree(nextValue) == false) return next;


            next += offset;
        }

        return next;
    }


    private bool IsOutOfBounds(Vector2Int pos)
    {
        var size = _gameRepository.GetBoardSize();
        if (pos.X < 0 || pos.X >= size.X || pos.Y < 0 || pos.Y >= size.Y) return true;
        return false;
    }

    private bool CanMerge(int value, int nextValue)
    {
        if (value == 0 || nextValue == 0 || value != nextValue) return false;
        return true;
    }

    private bool IsFree(int value) => value == 0;

    private Vector2Int GetStart(MoveDirection direction)
    {
        var size = _gameRepository.GetBoardSize();
        switch (direction)
        {
            case MoveDirection.Top:
                return new Vector2Int(0, size.Y - 1);
            case MoveDirection.Down:
            case MoveDirection.Left:
                return new Vector2Int(0, 0);
            case MoveDirection.Right:
                return new Vector2Int(size.X - 1, 0);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private Vector2Int GetOffset(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Top:
                return new Vector2Int(0, -1);
            case MoveDirection.Down:
                return new Vector2Int(0, 1);
            case MoveDirection.Left:
                return new Vector2Int(1, 0);
            case MoveDirection.Right:
                return new Vector2Int(-1, 0);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private Vector2Int GetEndOffset(MoveDirection direction)
    {
        var size = _gameRepository.GetBoardSize();
        switch (direction)
        {
            case MoveDirection.Top:
                return new Vector2Int(1, size.Y - 1);
            case MoveDirection.Down:
                return new Vector2Int(1, 1 - size.Y);
            case MoveDirection.Left:
                return new Vector2Int(1 - size.X, 1);
            case MoveDirection.Right:
                return new Vector2Int(size.X - 1, 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private Vector2Int GetEnd(MoveDirection direction)
    {
        var size = _gameRepository.GetBoardSize();

        switch (direction)
        {
            case MoveDirection.Top:
                return new Vector2Int(size.X - 1, 0);
            case MoveDirection.Down:
            case MoveDirection.Left:
                return new Vector2Int(size.X - 1, size.Y - 1);
            case MoveDirection.Right:
                return new Vector2Int(0, size.Y - 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}