using System;
using Plugins.Grid;
using UnityEngine;

[Serializable]
public class GridFactory<T>
{
    [SerializeField] private float cellSize;


    public GridSystem2D<T> Create(int width, int height)
    {
        return new GridSystem2D<T>(width, height, cellSize, Vector3.zero,
            new GridSystem2D<T>.VerticalConverter(), false);
    }
}