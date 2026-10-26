using UnityEngine;

public static class Ext
{
    public static MoveDirection ToMoveDirection(this KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.UpArrow:
                return MoveDirection.Top;
            case KeyCode.DownArrow:
                return MoveDirection.Down;
            case KeyCode.RightArrow:
                return MoveDirection.Right;
            case KeyCode.LeftArrow:
                return MoveDirection.Left;
            default:
                return MoveDirection.Left;
        }
    }
}