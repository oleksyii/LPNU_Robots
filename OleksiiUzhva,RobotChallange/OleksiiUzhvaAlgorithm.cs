using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace OleksiiUzhva_RobotChallange
{
    public enum Cardinal
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW,
        NONE
    };

    // Dictionary<EnergyStation, List<Cell>>
    public class OleksiiUzhvaAlgorithm : IRobotAlgorithm
    {
        public int RoundCount = 0;

        public string Author => "Oleksii Uzhva";

        public string Description => "Super smart algorithm";

        //public List<List<Dictionary<Position, bool>>> _Stations = new List<List<Dictionary<Position, bool>>>();

        public Dictionary<EnergyStation, List<Cell>> _Stations = new Dictionary<EnergyStation, List<Cell>>();

        public Dictionary<int, EnergyStation> _AssignedStations = new Dictionary<int, EnergyStation>();

        public Dictionary<EnergyStation, int> _CountAssigned = new Dictionary<EnergyStation, int>();

        public Manager Manager;

        public bool b_Done_Assigning_Map = false;

        // Have a dictionary of robotIndexes and corresponding stations

        public OleksiiUzhvaAlgorithm()
        {
            Logger.OnLogRound += Logger_OnLogRound;
            Manager = new Manager();

        }

        private void Logger_OnLogRound(object sender, LogRoundEventArgs e)
        {
            RoundCount++;
        }

        public void AssignCells(Map map, IList<Robot.Common.Robot> robots)
        {
            for (int i = 0; i < map.Stations.Count; i++)
            {
                _Stations.Add(map.Stations[i], new List<Cell>());
                _CountAssigned.Add(map.Stations[i], 0);
                Position stationPos = map.Stations[i].Position;
                for (int n = -2; n <= 2; n++)
                {
                    for (int m = -2; m <= 2; m++)
                    {
                        if (n == 0)
                        {
                            Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                            _Stations[map.Stations[i]].Add(new Cell(temp, Manager.IsCellFree(temp, robots)));
                        }
                        else if (n % 2 == 0 && m % 2 == 0)
                        {
                            Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                            _Stations[map.Stations[i]].Add(new Cell(temp, Manager.IsCellFree(temp, robots)));
                        }
                        else if ((n % 2 == 1 || n % 2 == -1) && (m % 2 == 1 || m % 2 == -1 || m == 0))
                        {
                            Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                            _Stations[map.Stations[i]].Add(new Cell(temp, Manager.IsCellFree(temp, robots)));
                        }
                    }
                }

            }
        }
        /*
         * Assigns each robot a station to stick around to. If num of robots 
         * assigned to a station > 16, then finds another station
         * ExcludedStation - if you don't want to count in that station
         */
        public void AssignMeAStation(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex)
        {
            for (int i = 0; i < 100; i++)
            {
                List<EnergyStation> st = map.GetNearbyResources(robots[robotToMoveIndex].Position, i);
                if (st.Count > 0)
                {
                    if (_CountAssigned[st[0]] <= 16)
                    {
                        _AssignedStations.Add(robotToMoveIndex, st[0]);
                        _CountAssigned[st[0]]++;
                        break;
                    }
                }
            }
        }

        public Cardinal Find_The_Direction(Position currentPosition, Position goalPosition)
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

        /*
         * Needs a rework. To not to jump straight x--; y--, but first check x--, then both
         */
        public Position Shothen_The_Vector(Position goal, Cardinal direction)
        {
            switch(direction)
            {
                case(Cardinal.E):
                    return new Position(goal.X--, goal.Y);
                case (Cardinal.SE):
                    return new Position(goal.X--, goal.Y++);
                case (Cardinal.S):
                    return new Position(goal.X, goal.Y++);
                case (Cardinal.SW):
                    return new Position(goal.X++, goal.Y++);
                case (Cardinal.W):
                    return new Position(goal.X++, goal.Y);
                case (Cardinal.NW):
                    return new Position(goal.X++, goal.Y--);
                case (Cardinal.N):
                    return new Position(goal.X, goal.Y--);
                case (Cardinal.NE):
                    return new Position(goal.X--, goal.Y--);
                default:
                    return goal;
            }
        }

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            // If round == 1 then initialize a list of lists of dictionary<position, bool>
            // called stationAreas that would contain info on where power cells are located.
            //
            // Every next round go through that list and see, if any of them got any robots.
            // If so, change the bool.

            // For now, do the algorithm to get to a station with 13% of my energy left
            // 
            // If station is not in a range to have 13% of my energy after jump, move closer
            // by step, that is equal 10% of current energy state
            //
            // Idea: Get a point. treat a distance like a vector. If cannot jump using 83% of my
            // energy, shorten the vector, so that it's of the length that corresponds to 13% of my energy.
            // To shorten a vector keep an enum, that says in which direction you are pointing.
            //           
            //           N
            //     -NW      NE-
            //  <W              E>
            //     -SW      SE-
            //           S
            // - N (decrement Y)
            // - NE (decrement X and Y)
            // - E (decrement X)
            // - SE (decrement X, increment Y)
            // - S (increment Y)
            // - SW (increment X and Y)
            // - W (increment W)
            // - NW (increment X, decrement Y)
            // Cost of jump = (vector's length)^2

            /*
                
             * 
             * 
             * If rounds are late, don't create robots
             */
            if(RoundCount == 0)
            {
                if (!b_Done_Assigning_Map)
                {
                    AssignCells(map, robots);
                    b_Done_Assigning_Map = true;
                }

                AssignMeAStation(map, robots, robotToMoveIndex);
            }

            RobotCommand Command = new MoveCommand() { NewPosition = robots[robotToMoveIndex].Position};

            // Check if can collct energy



            //Check if can move

            Position goalPosition = _Stations[_AssignedStations[robotToMoveIndex]][0].position;
           
            for(int i = 0; i < 17; i++)
            {
                Cell tmp = _Stations[_AssignedStations[robotToMoveIndex]][i];

                if (Manager.IsCellFree(ref tmp, robots))
                {
                    goalPosition = tmp.position;
                }

            }

            //MOVEMENT
            Cardinal direction = Cardinal.NONE;
            Position currentPosition = robots[robotToMoveIndex].Position;
            direction = Find_The_Direction(currentPosition, goalPosition);

            int availableEnergy = (int)Math.Floor(robots[robotToMoveIndex].Energy * 0.8);
            if (DistanceHelper.Cost(currentPosition, goalPosition) <= availableEnergy)
            {
                Command = new MoveCommand() { NewPosition =  goalPosition };
            }
            else
            {
                availableEnergy = ((int)Math.Floor(robots[robotToMoveIndex].Energy * 0.1) == 0 ? (int)Math.Ceiling(robots[robotToMoveIndex].Energy * 0.1) : (int)Math.Floor(robots[robotToMoveIndex].Energy * 0.1));
                while (DistanceHelper.Cost(currentPosition, goalPosition) > availableEnergy)
                {
                    // Shothen a vector
                    goalPosition = Shothen_The_Vector(goalPosition, direction);
                }
                Command = new MoveCommand() { NewPosition = goalPosition };
            }




            return Command;

            var myRobot = robots[robotToMoveIndex];
            Random rng = new Random();
            var newPos = myRobot.Position;
            //newPos.X = newPos.X + rng.Next(-10, 10);
            newPos.X = newPos.X + 10;



            return new MoveCommand(){ NewPosition = newPos };
        }

        
    }
}
