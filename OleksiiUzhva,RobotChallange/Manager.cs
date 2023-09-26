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
            foreach(Robot.Common.Robot robot in robots)
            {
                if(robot.Position != pos)
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

    }
}
