using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OleksiiUzhva_RobotChallange
{
    public class DistanceHelper
    {
        public static int Cost(Position a, Position b)
        {
            return (int)(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static int Cost(Position a, Cell b)
        {
            return (int)(Math.Pow(a.X - b.position.X, 2) + Math.Pow(a.Y - b.position.Y, 2));
        }
        public static int Cost(Cell a, Position b)
        {
            return (int)(Math.Pow(a.position.X - b.X, 2) + Math.Pow(a.position.Y - b.Y, 2));
        }
        public static int Cost(Cell a, Cell b)
        {
            return (int)(Math.Pow(a.position.X - b.position.X, 2) + Math.Pow(a.position.Y - b.position.Y, 2));
        }

        public static Cardinal Find_The_Direction(Position currentPosition, Position goalPosition)
        {
            if (goalPosition.X > currentPosition.X && goalPosition.Y == currentPosition.Y)
                return Cardinal.E;
            else if (goalPosition.X > currentPosition.X && goalPosition.Y < currentPosition.Y)
                return Cardinal.SE;
            else if (goalPosition.X == currentPosition.X && goalPosition.Y < currentPosition.Y)
                return Cardinal.S;
            else if (goalPosition.X < currentPosition.X && goalPosition.Y < currentPosition.Y)
                return Cardinal.SW;
            else if (goalPosition.X < currentPosition.X && goalPosition.Y == currentPosition.Y)
                return Cardinal.W;
            else if (goalPosition.X < currentPosition.X && goalPosition.Y > currentPosition.Y)
                return Cardinal.NW;
            else if (goalPosition.X == currentPosition.X && goalPosition.Y > currentPosition.Y)
                return Cardinal.N;
            else if (goalPosition.X > currentPosition.X && goalPosition.Y > currentPosition.Y)
                return Cardinal.NE;

            return Cardinal.NONE;
        }
    }
}
