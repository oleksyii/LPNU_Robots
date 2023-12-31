﻿using Robot.Common;
using Robot.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public class OleksiiUzhvaAlgorithm : IRobotAlgorithm
    {
        public int RoundCount = 0;

        public int CountMyBots = 10;

        public string Author => "Oleksii Uzhva";

        public string Description => "Super smart algorithm";

        public Dictionary<Position, List<Cell>> _Stations = new Dictionary<Position, List<Cell>>();

        public Dictionary<int, Position> _AssignedStations = new Dictionary<int, Position>();

        public Dictionary<Position, int> _CountAssigned = new Dictionary<Position, int>();

        public Dictionary<int, int> _RobotsCreated = new Dictionary<int, int>();

        public Manager Manager;

        public bool b_Done_Assigning_Map = false;

        public const int c_EnergyToCreateRobot = 200;

        // Have a dictionary of robotIndexes and corresponding stations
        //
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
            foreach (EnergyStation station in map.Stations)
            {
                if (!_Stations.ContainsKey(station.Position))
                {

                    _Stations.Add(station.Position, new List<Cell>());
                    _CountAssigned.Add(station.Position, 0);
                    for (int n = -2; n <= 2; n++)
                    {
                        for (int m = -2; m <= 2; m++)
                        {
                            Position stationPos = station.Position;

                            if (n == 0)
                            {
                                Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                                if (map.IsValid(temp))
                                    _Stations[station.Position].Add(new Cell(temp, Manager.IsCellFree(temp, robots)));
                            }
                            else if (n % 2 == 0 && m % 2 == 0)
                            {
                                Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                                if (map.IsValid(temp))
                                    _Stations[station.Position].Add(new Cell(temp, Manager.IsCellFree(temp, robots)));
                            }
                            else if ((n % 2 == 1 || n % 2 == -1) && (m % 2 == 1 || m % 2 == -1 || m == 0))
                            {
                                Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                                if (map.IsValid(temp))
                                    _Stations[station.Position].Add(new Cell(temp, Manager.IsCellFree(temp, robots)));
                            }
                        }
                    }
                }
            }


        }

        public void AssureAssignCells(Map map, IList<Robot.Common.Robot> robots)
        {
            foreach (EnergyStation station in map.Stations)
            {
                // if i > 0. Error was, that I try to check the cell that is not going to be valid.
                int i = _Stations[station.Position].Count - 1;
                for (int n = 2; n >= -2; n--)
                {
                    for (int m = 2; (m >= -2) && (i >= 0); m--)
                    {
                        Position stationPos = station.Position;

                        if (n == 0)
                        {
                            if (_Stations[station.Position][i].position != new Position(stationPos.X + n, stationPos.Y + m))
                            {
                                Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                                if (map.IsValid(temp))
                                    _Stations[station.Position][i].position = temp;
                                else
                                    i++;
                            }
                            i--;
                        }
                        else if (n % 2 == 0 && m % 2 == 0)
                        {
                            if (_Stations[station.Position][i].position != new Position(stationPos.X + n, stationPos.Y + m))
                            {
                                Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                                if (map.IsValid(temp))
                                    _Stations[station.Position][i].position = temp;
                                else
                                    i++;
                            }
                            i--;
                        }
                        else if ((n % 2 == 1 || n % 2 == -1) && (m % 2 == 1 || m % 2 == -1 || m == 0))
                        {
                            if (_Stations[station.Position][i].position != new Position(stationPos.X + n, stationPos.Y + m))
                            {
                                Position temp = new Position(stationPos.X + n, stationPos.Y + m);
                                if (map.IsValid(temp))
                                    _Stations[station.Position][i].position = temp;
                                else
                                    i++;
                            }
                            i--;
                        }
                        
                    }

                }
            }
        }

        /*
         * Assigns each robot a station to stick around to. If num of robots 
         * assigned to a station > than some amount, then finds another station
         * ExcludedStation - if you don't want to count in that station
         */
        public void AssignMeAStation(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex)
        {
            if(!_RobotsCreated.ContainsKey(robotToMoveIndex))
            {
                _RobotsCreated.Add(robotToMoveIndex, 0);
            }

            for (int i = 0; i < 100; i++)
            {
                List<EnergyStation> st = map.GetNearbyResources(robots[robotToMoveIndex].Position, i);
                if (st.Count > 0)
                {
                    foreach (EnergyStation stat in st)
                    {
                        if ((_CountAssigned[stat.Position] < 2) && robots.Count < 1500)
                        {
                            _AssignedStations.Add(robotToMoveIndex, stat.Position);
                            _CountAssigned[stat.Position]++;
                            return;
                        }
                    }
                }
            }
        }

        public void ReassignMeAStation(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex, ref List<Position> ExcludedStation)
        {
            if (!_RobotsCreated.ContainsKey(robotToMoveIndex))
            {
                _RobotsCreated.Add(robotToMoveIndex, 0);
            }

            --_CountAssigned[_AssignedStations[robotToMoveIndex]];
            bool stop = false;
            for (int i = 0; i < 100 && !stop; i++)
            {
                List<EnergyStation> st = map.GetNearbyResources(robots[robotToMoveIndex].Position, i);
                if (st.Count > 0)
                {
                    foreach (EnergyStation stat in st)
                    {
                        if ((_CountAssigned[stat.Position] < 2) && !ExcludedStation.Contains(stat.Position))
                        {
                            if (_AssignedStations.ContainsKey(robotToMoveIndex))
                            {
                                _AssignedStations[robotToMoveIndex] = stat.Position;
                                stop = true;
                                break;
                            }
                            else
                            {
                                _AssignedStations.Add(robotToMoveIndex, stat.Position);
                                _CountAssigned[stat.Position]++;
                                stop = true;
                                break;
                            }
                        }
                    }
                }
            }

            AssureMeAStation(map, robots, robotToMoveIndex, ref ExcludedStation);
        }

        public void AssureMeAStation(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex)
        {
            int count = 0;

            foreach (Cell cell in _Stations[_AssignedStations[robotToMoveIndex]])
            {
                foreach (Robot.Common.Robot robot in robots)
                {
                    if (robot.Position == cell.position && robot.OwnerName != "Oleksii Uzhva" && robots.IndexOf(robot) < robotToMoveIndex)
                    {
                        count++;
                    }
                }

            }
            if (count >= 2)
            {
                List<Position> ExcludedStations = new List<Position>()
                    {
                        _AssignedStations[robotToMoveIndex]
                    };
                ReassignMeAStation(map, robots, robotToMoveIndex, ref ExcludedStations);
            }
        }

        public void AssureMeAStation(Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex, ref List<Position> ExcludedStations)
        {
            int count = 0;

            foreach (Cell cell in _Stations[_AssignedStations[robotToMoveIndex]])
            {
                foreach (Robot.Common.Robot robot in robots)
                {
                    if (robot.Position == cell.position && robot.OwnerName != "Oleksii Uzhva" && robots.IndexOf(robot) < robotToMoveIndex)
                    {
                        count++;

                    }
                }

            }
            if (count >= 2)
            {
                ExcludedStations.Add(_AssignedStations[robotToMoveIndex]);
                ReassignMeAStation(map, robots, robotToMoveIndex, ref ExcludedStations);
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
            switch (direction)
            {
                case (Cardinal.E):
                    return new Position(--goal.X, goal.Y);
                case (Cardinal.SE):
                    return new Position(--goal.X, ++goal.Y);
                case (Cardinal.S):
                    return new Position(goal.X, ++goal.Y);
                case (Cardinal.SW):
                    return new Position(++goal.X, ++goal.Y);
                case (Cardinal.W):
                    return new Position(++goal.X, goal.Y);
                case (Cardinal.NW):
                    return new Position(++goal.X, --goal.Y);
                case (Cardinal.N):
                    return new Position(goal.X, --goal.Y);
                case (Cardinal.NE):
                    return new Position(--goal.X, --goal.Y);
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


            if(RoundCount == 0)
            {
                if (!b_Done_Assigning_Map)
                {
                    AssignCells(map, robots);
                    b_Done_Assigning_Map = true;
                }

                AssignMeAStation(map, robots, robotToMoveIndex);
                int a = 0;
                int b = a + 10;
            }

            if(!_AssignedStations.ContainsKey(robotToMoveIndex))
            {
                AssignMeAStation(map, robots, robotToMoveIndex);
            }

            RobotCommand Command = new MoveCommand() { NewPosition = robots[robotToMoveIndex].Position };
            Position currentPosition = robots[robotToMoveIndex].Position;

            AssureMeAStation(map, robots, robotToMoveIndex);

            if (Manager.IsRobotOnStation(robots, _AssignedStations, _Stations, robotToMoveIndex) && robots[robotToMoveIndex].Energy > c_EnergyToCreateRobot && (robots.Count <= 2*_Stations.Count)  && CountMyBots < 100 && _RobotsCreated[robotToMoveIndex] < 5)
            {
                Command = new CreateNewRobotCommand();
                CountMyBots++;
                _RobotsCreated[robotToMoveIndex]++;
            }
            else if (Manager.IsRobotOnStation(robots, _AssignedStations, _Stations, robotToMoveIndex))
            {
                Command = new CollectEnergyCommand();
            }

            else
            {

                

                if (robots[robotToMoveIndex].Position == _AssignedStations[robotToMoveIndex]) { }


                // TODO: (optional) Check whether a station can hold those two robots
                // (by checking recovery rate)
                //
                


                Position goalPosition = new Position (_Stations[_AssignedStations[robotToMoveIndex]][0].position.X, _Stations[_AssignedStations[robotToMoveIndex]][0].position.Y);

                int cost = DistanceHelper.Cost(currentPosition, _Stations[_AssignedStations[robotToMoveIndex]][0]);
                
                foreach(Cell cell in _Stations[_AssignedStations[robotToMoveIndex]])
                {
                    int cost2 = DistanceHelper.Cost(currentPosition, cell);
                    if (cost2 < cost && Manager.IsCellFree(cell.position, robots))
                    {
                        goalPosition = cell.position;
                        cost = cost2;
                    }

                }
                

                //MOVEMENT
                // TODO: rework a moving calculation
                Cardinal direction = Find_The_Direction(currentPosition, goalPosition);

                int availableEnergy = (int)Math.Floor(robots[robotToMoveIndex].Energy * 0.83);
                if (DistanceHelper.Cost(currentPosition, goalPosition) <= availableEnergy)
                {
                    
                    Command = new MoveCommand() { NewPosition = goalPosition };
                }
                else
                {
                    availableEnergy = ((int)Math.Floor(robots[robotToMoveIndex].Energy * 0.15) == 0 ? (int)Math.Ceiling(robots[robotToMoveIndex].Energy * 0.15) : (int)Math.Floor(robots[robotToMoveIndex].Energy * 0.15));
                    while (DistanceHelper.Cost(currentPosition, goalPosition) > availableEnergy)
                    {
                        // Shothen a vector
                        goalPosition = Shothen_The_Vector(goalPosition, direction);
                        direction = Find_The_Direction(currentPosition, goalPosition);
                    }
                    Command = new MoveCommand() { NewPosition = goalPosition };
                }
            }

            AssureAssignCells(map, robots);

            return Command;
        }

        
    }
}
