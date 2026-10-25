using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

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
                Debug.Log("Out of bounds");
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
                _gameRepository.SetValue(current, value + nextValue);
                _gameRepository.SetValue(next, 0);
                isMergeConsumed = true;
                result = true;
                continue;
            }

            if (IsFree(value))
            {
                Debug.Log("IsFree!");
                _gameRepository.SetValue(current, nextValue);
                _gameRepository.SetValue(next, 0);
                result = true;
            }
            else
            {
                Debug.Log("IsNotFree!");

                _gameRepository.SetValue(next, 0);
                _gameRepository.SetValue(current + offset, nextValue);
                result = true;
                current += offset;
            }
        }
    }

    public void SpawnNewValue()
    {
        var boardSize = _gameRepository.GetBoardSize();


        var freeList = new List<Vector2Int>();

        for (int x = 0; x < boardSize.X; x++)
        {
            for (int y = 0; y < boardSize.Y; y++)
            {
                var pos = new Vector2Int(x, y);
                var value = _gameRepository.GetValue(pos);
                if (value == 0)
                {
                    freeList.Add(new Vector2Int(x, y));
                }
            }
        }

        if (freeList.Count > 0)
        {
            var pos = freeList[new Random().Next(0, freeList.Count)];
            _gameRepository.SetValue(pos, 2);
        }
    }

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