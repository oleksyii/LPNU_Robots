using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace OleksiiUzhva_RobotChallange
{
    public class OleksiiUzhvaAlgorithm : IRobotAlgorithm
    {
        public string Author => "Oleksii Uzhva";

        public string Description => "Super smart algorithm";

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            var myRobot = robots[robotToMoveIndex];
            Random rng = new Random();
            var newPos = myRobot.Position;
            //newPos.X = newPos.X + rng.Next(-10, 10);
            newPos.X = newPos.X + 10;



            return new MoveCommand(){ NewPosition = newPos };
        }
    }
}
