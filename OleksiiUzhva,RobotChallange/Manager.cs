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
        public bool IsCellFree(Position pos, IList<Robot.Common.Robot> robots)
        {
            bool result = true;
            foreach (Robot.Common.Robot robot in robots)
            {
                if (robot.Position == pos)
                    result = false;
            }
            return result;
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


    }
}
