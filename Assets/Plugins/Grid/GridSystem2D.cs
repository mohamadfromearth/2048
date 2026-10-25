using System;
using UnityEngine;

namespace Plugins.Grid
{
    /// <summary>
    /// A generic 2D grid implementation that can be used in both 2D and 3D.
    /// The grid stores values of a specified type and provides methods for accessing and modifying
    /// these values based on grid coordinates or world coordinates. The grid can be displayed
    /// either vertically (X-Y plane) or horizontally (X-Z plane) using a CoordinateConverter.
    /// </summary>
    public class GridSystem2D<T>
    {
        readonly int width;
        readonly int height;
        readonly float cellSize;
        readonly Vector3 origin;
        readonly T[,] gridArray;

        readonly CoordinateConverter coordinateConverter;

        public event Action<int, int, T> OnValueChangeEvent;

        public static GridSystem2D<T> VerticalGrid(int width, int height, float cellSize, Vector3 origin,
            bool debug = false)
        {
            return new GridSystem2D<T>(width, height, cellSize, origin, new VerticalConverter(), debug);
        }

        public static GridSystem2D<T> HorizontalGrid(int width, int height, float cellSize, Vector3 origin,
            bool debug = false)
        {
            return new GridSystem2D<T>(width, height, cellSize, origin, new HorizontalConverter(), debug);
        }

        public GridSystem2D(int width, int height, float cellSize, Vector3 origin,
            CoordinateConverter coordinateConverter,
            bool debug)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.origin = origin;
            this.coordinateConverter = coordinateConverter ?? new VerticalConverter();

            gridArray = new T[width, height];
        }


        // Set a value from a grid position
        public void SetValue(Vector3 worldPosition, T value)
        {
            Vector2Int pos = coordinateConverter.WorldToGrid(worldPosition, cellSize, origin);
            SetValue(pos.x, pos.y, value);
        }


        public Vector3 GetGridCenter()
        {
            int x = width / 2;
            int y = height / 2;

            var center = coordinateConverter.GridToWorld(x, y, cellSize, origin);
            center.z = 0;


            var dummyPoint1 = coordinateConverter.GridToWorld(0, 0, cellSize, origin);
            var dummyPoint2 = coordinateConverter.GridToWorld(1, 1, cellSize, origin);

            if (width % 2 == 0)
            {
                center.x -= (dummyPoint2.x - dummyPoint1.x) / 2;
            }

            if (height % 2 == 0)
            {
                center.y -= (dummyPoint2.y - dummyPoint1.y) / 2;
            }

            return center;
        }

        public void SetValue(int x, int y, T value)
        {
            if (IsValid(x, y))
            {
                gridArray[x, y] = value;
                OnValueChangeEvent?.Invoke(x, y, value);
            }
        }

        // Get a value from a grid position
        public T GetValue(Vector3 worldPosition)
        {
            Vector2Int pos = GetXY(worldPosition);
            return GetValue(pos.x, pos.y);
        }

        public T GetValue(int x, int y)
        {
            return IsValid(x, y) ? gridArray[x, y] : default;
        }

        bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

        public Vector2Int GetXY(Vector3 worldPosition) =>
            coordinateConverter.WorldToGrid(worldPosition, cellSize, origin);

        public Vector3 GetWorldPositionCenter(int x, int y) =>
            coordinateConverter.GridToWorld(x, y, cellSize, origin);

        Vector3 GetWorldPosition(int x, int y) => coordinateConverter.GridToWorld(x, y, cellSize, origin);

        void DrawDebugLines()
        {
            const float duration = 100f;
            var parent = new GameObject("Debugging");


            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, duration);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, duration);
        }


        public abstract class CoordinateConverter
        {
            public abstract Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin);

            public abstract Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin);

            public abstract Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin);

            public abstract Vector3 Forward { get; }
        }


        public class ScreenDependentConverter : CoordinateConverter
        {
            private float _minX;
            private float _minY;
            private float _maxX;
            private float _maxY;
            private Vector3 _minPos;
            private Vector3 _maxPos;

            public ScreenDependentConverter(float minX, float minY, float maxX, float maxY, Vector3 minPos,
                Vector3 maxPos)
            {
                _minX = minX;
                _minY = minY;
                _maxX = maxX;
                _maxY = maxY;
                _minPos = minPos;
                _maxPos = maxPos;
            }


            public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
            {
                var worldXt = x / _maxX;
                var worldYt = y / _maxY;

                return new Vector3(Mathf.Lerp(_minPos.x, _maxPos.x, worldXt), Mathf.Lerp(_minPos.y, _maxPos.y, worldYt),
                    0);
            }

            public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
            {
                throw new NotImplementedException();
            }

            public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
            {
                var maxXDistance = _maxPos.x - _minPos.x;
                var maxYDistance = _maxPos.y - _minPos.y;

                var currentXDistance = _maxPos.x - worldPosition.x;
                var currentYDistance = _maxPos.y - worldPosition.y;

                int x = Mathf.RoundToInt(Mathf.Lerp(0, _maxX, 1 - currentXDistance / maxXDistance));
                int y = Mathf.RoundToInt(Mathf.Lerp(0, _maxY, 1 - currentYDistance / maxYDistance));


                return new Vector2Int(x, y);
            }

            public override Vector3 Forward { get; }
        }


        /// <summary>
        /// A coordinate converter for vertical grids, where the grid lies on the X-Y plane.
        /// </summary>
        public class VerticalConverter : CoordinateConverter
        {
            public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
            {
                return new Vector3(x, y, 0) * cellSize + origin;
            }

            public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
            {
                return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0) + origin;
            }

            public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
            {
                Vector3 gridPosition = (worldPosition - origin) / cellSize;
                var x = Mathf.FloorToInt(gridPosition.x);
                var y = Mathf.FloorToInt(gridPosition.y);
                return new Vector2Int(x, y);
            }

            public override Vector3 Forward => Vector3.forward;
        }

        /// <summary>
        /// A coordinate converter for horizontal grids, where the grid lies on the X-Z plane.
        /// </summary>
        public class HorizontalConverter : CoordinateConverter
        {
            public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
            {
                return new Vector3(x * cellSize + cellSize * 0.5f, 0, y * cellSize + cellSize * 0.5f) + origin;
            }

            public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
            {
                return new Vector3(x, 0, y) * cellSize + origin;
            }

            public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
            {
                Vector3 gridPosition = (worldPosition - origin) / cellSize;
                var x = Mathf.FloorToInt(gridPosition.x);
                var y = Mathf.FloorToInt(gridPosition.z);
                return new Vector2Int(x, y);
            }

            public override Vector3 Forward => -Vector3.up;
        }


    }
}