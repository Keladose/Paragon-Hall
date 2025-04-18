using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class Door
    {

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Undefined
        }
        public Direction direction;


        public static Direction GetOppositeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    return Direction.Undefined;
            }
        }
    }
}
