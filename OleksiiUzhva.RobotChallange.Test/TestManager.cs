using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using OleksiiUzhva_RobotChallange;
using Robot.Common;
using System.Collections.Generic;
using R = Robot.Common.Robot;

namespace OleksiiUzhva.RobotChallange.Test
{
    [TestClass]
    public class TestManager
    {
        [TestMethod]
        public void TestIsCellFree()
        {
            Manager manager = new Manager();
            //manager.IsCellFree()
            var map = new Map();

            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() {Energy = 200, Position = new Position(1,1)}
            };

            var cell = new Cell(new Position(1, 2), true);

            bool result = manager.IsCellFree(ref cell, robots);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsRobotOnStation()
        {
            Manager manager = new Manager();
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot() {Energy = 200, Position = new Position(1,1)}
            };
            var cell = new Cell(new Position(1, 1));
            int robotToMoveIndex = 0;

            Assert.IsTrue(manager.IsRobotOnStation(robots, cell, robotToMoveIndex));

        }

        [TestMethod]
        public void TestDistanceCost()
        {
            Cell cell1 = new Cell(new Position(0, 0));
            Cell cell2 = new Cell(new Position(0, 5));

            Assert.IsTrue((DistanceHelper.Cost(cell1, cell2) == 25));
        }

        [TestMethod]
        public void TestFindTheDirection()
        {
            Position pos1 = new Position(0, 0);
            Position pos2 = new Position(0, 5);

            Assert.IsTrue(DistanceHelper.Find_The_Direction(pos1, pos2) == Cardinal.N);
        }

        [TestMethod]
        public void TestGetAuthorRobotCount()
        {
            Manager manager = new Manager();
            var robots = new List<R>()
            {
                new R {Position = new Position(1, 1), Energy = 200, OwnerName = "Andrii"},
                new R {Position = new Position(2, 1), Energy = 500, OwnerName = "Yulian"},
                new R {Position = new Position(3, 1), Energy = 300, OwnerName = "Oleg"},
                new R {Position = new Position(4, 1), Energy = 500, OwnerName = "Petro"},
                new R {Position = new Position(5, 1), Energy = 400, OwnerName = "Yulian"},
                new R {Position = new Position(6, 1), Energy = 500, OwnerName = "Oleg"},
                new R {Position = new Position(0, 1), Energy = 800, OwnerName = "Yulian"}
            };
            Assert.AreEqual(3, Manager.GetAuthorRobotCount(robots, "Yulian"));
            Assert.AreEqual(2, Manager.GetAuthorRobotCount(robots, "Oleg"));
            Assert.AreEqual(1, Manager.GetAuthorRobotCount(robots, "Petro"));
            Assert.AreEqual(0, Manager.GetAuthorRobotCount(robots, "Ivan"));
        }

        [TestMethod]
        public void TestIsWithinRadius()
        {
            var p0 = new Position(1, 1);
            var p1 = new Position(2, 4);
            var p2 = new Position(0, 0);
            var p3 = new Position(1, 5);
            Assert.AreEqual(false, Manager.IsWithinRadius(p0, p1, 2));
            Assert.AreEqual(true, Manager.IsWithinRadius(p0, p1, 3));
            Assert.AreEqual(true, Manager.IsWithinRadius(p0, p2, 2));
            Assert.AreEqual(true, Manager.IsWithinRadius(p0, p2, 1));
            Assert.AreEqual(false, Manager.IsWithinRadius(p0, p3, 2));
            Assert.AreEqual(false, Manager.IsWithinRadius(p0, p3, 3));
        }

        [TestMethod]
        public void TestGetNearbyRobotCount()
        {
            var p0 = new Position(1, 1);
            var p1 = new Position(5, 5);
            var robots = new List<R>()
            {
                new R {Position = new Position(1, 0), Energy = 200, OwnerName = "Andrii"},
                new R {Position = new Position(2, 1), Energy = 500, OwnerName = "Yulian"},
                new R {Position = new Position(4, 1), Energy = 500, OwnerName = "Petro"},
                new R {Position = new Position(5, 1), Energy = 400, OwnerName = "Yulian"},
                new R {Position = new Position(6, 1), Energy = 500, OwnerName = "Oleg"},
            };
            Assert.AreEqual(2, Manager.GetNearbyRobotCount(robots, p0));
            Assert.AreEqual(0, Manager.GetNearbyRobotCount(robots, p1));
        }

        [TestMethod]
        public void TestCollision()
        {
            Assert.IsTrue(Manager.IsCollision(new Position(1, 1), new Position(2, 2)));
            Assert.IsFalse(Manager.IsCollision(new Position(0, 0), new Position(8, 9)));
        }
        [TestMethod]
        public void TestAvailablePosition()
        {
            var p0 = new Position(1, 1);
            var p1 = new Position(2, 4);
            var p2 = new Position(-10, 20);
            var p3 = new Position(10, 150);
            var map = new Map();
            map.Stations.Add(new EnergyStation()
            {
                Energy = 500,
                Position = new Position(2, 2),
                RecoveryRate = 2
            });
            var robots = new List<R>()
            {
                new R {Position = new Position(1, 1), Energy = 200, OwnerName = "Yulian"},
                new R {Position = new Position(5, 1), Energy = 500, OwnerName = "Yulian"}
            };
            Assert.AreEqual(false, Manager.IsAvailablePosition(map, robots, p0, "Yulian"));
            Assert.AreEqual(true, Manager.IsAvailablePosition(map, robots, p1, "Yulian"));
            Assert.AreEqual(true, Manager.IsAvailablePosition(map, robots, p1, "Andrii"));
            Assert.AreEqual(false, Manager.IsAvailablePosition(map, robots, p2, "Andrii"));
            Assert.AreEqual(false, Manager.IsAvailablePosition(map, robots, p3, "Yulian"));
        }


        [TestMethod]
        public void TestDist()
        {
            Position p0 = new Position(1, 1);
            Position p1 = new Position(2, 4);
            Position p2 = new Position(0, 20);
            Position p3 = new Position(10, 50);

            Assert.IsTrue(10 == Manager.DistanceCost(p0, p1));
            Assert.IsTrue(362 == Manager.DistanceCost(p0, p2));
            Assert.IsTrue(2482 == Manager.DistanceCost(p0, p3));
        }
    }
}
