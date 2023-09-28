using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OleksiiUzhva_RobotChallange
{
    public class Manager
    {
        //public static bool IsCellFree(Position pos, IList<Robot.Common.Robot> robots)
        //{
        //    foreach (Robot.Common.Robot robot in robots)
        //    {
        //        if (robot.Position != pos)
        //            return true;
        //    }
        //    return false;
        //}

        public bool IsCellFree(Position pos, IList<Robot.Common.Robot> robots)
        {
            foreach (Robot.Common.Robot robot in robots)
            {
                if (robot.Position != pos)
                    return true;
            }
            return false;
        }

        public bool IsCellFree(ref Cell cell, IList<Robot.Common.Robot> robots)
        {
            Position pos = cell.position;
            bool result = true;
            foreach (Robot.Common.Robot robot in robots)
            {
                if (robot.Position == pos)
                {
                    result = false;
                    cell.free = false;
                    return result;
                }
                    
            }
            cell.free = true;
            return result;
        }

        public bool IsRobotOnStation(IList<Robot.Common.Robot> robots, Dictionary<int, Position> _AssignedStations, Dictionary<Position, List<Cell>> _Stations, int robotToMoveIndex)
        {
            Position currentPosition = robots[robotToMoveIndex].Position;

            foreach(Cell cell in _Stations[_AssignedStations[robotToMoveIndex]])
            {
                if(cell.position == currentPosition) { return true; }
            }
            return false;
        }

        public bool IsRobotOnStation(IList<Robot.Common.Robot> robots, Cell cell, int robotToMoveIndex)
        {
            Position currentPosition = robots[robotToMoveIndex].Position;

            if (cell.position == currentPosition) { return true; }

            return false;
        }

        public bool ContatinsTwoRobotsWithHigherPriority;

        private const int NearbyRadius = 2;
        private const int EnergyRadius = 1;

        public static int DistanceCost(Position center, Position distant) => (center.X - distant.X) * (center.X - distant.X) + (center.Y - distant.Y) * (center.Y - distant.Y);

        public static int GetAuthorRobotCount(IList<Robot.Common.Robot> robots, string author) => ((IEnumerable<Robot.Common.Robot>)robots).Where<Robot.Common.Robot>((Func<Robot.Common.Robot, bool>)(robot => robot.OwnerName == author)).Count<Robot.Common.Robot>();

        public static bool IsWithinRadius(Position center, Position point, int radius) => Math.Abs(center.X - point.X) <= radius && Math.Abs(center.Y - point.Y) <= radius;

        public static int GetNearbyRobotCount(IList<Robot.Common.Robot> robots, Position position) => ((IEnumerable<Robot.Common.Robot>)robots).Where<Robot.Common.Robot>((Func<Robot.Common.Robot, bool>)(robot => Manager.IsWithinRadius(position, robot.Position, NearbyRadius))).Count<Robot.Common.Robot>();

        public static bool IsCollision(Position p1, Position p2)
        {
            int x = Math.Abs(p1.X - p2.X);
            int y = Math.Abs(p1.Y - p2.Y);
            return (x < 3) && (y < 3);
        }

        public static bool IsAvailablePosition(
  Map map,
  IList<Robot.Common.Robot> robots,
  Position position,
  string author)
        {
            //if (!map.IsValid(position))
            //    return false;
            if (position.X > 100 || position.X < 0 || position.Y > 100 || position.Y < 0)
                return false;
            foreach (Robot.Common.Robot robot in (IEnumerable<Robot.Common.Robot>)robots)
            {
                if (Position.Equals(robot.Position, position) && robot.OwnerName == author)
                    return false;
            }
            return true;
        }

    }
}
