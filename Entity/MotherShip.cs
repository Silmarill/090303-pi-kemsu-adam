using AsteroidSimulation.Common;
using AsteroidSimulation.Observer;
using System;
using System.Collections.Generic;

namespace AsteroidSimulation.Entity {
    public static class MotherShip {
        public static List<HarvesterShip> Fleet = new List<HarvesterShip>();
        public static Dictionary<string, List<Report>> Worklog = new Dictionary<string, List<Report>>();

        public static int ResourcesMined = 0;

        public static void ReceiveReport(string shipName, Report report, int amount) {
            if (Worklog.ContainsKey(shipName)) {
                Worklog[shipName].Add(report);
                ResourcesMined += amount;
            }
        }

        public static void InitializeFleet() {
            for (int shipIdx = 1; shipIdx <= 5; ++shipIdx) {
                HarvesterShip ship = new HarvesterShip($"Harvester-{shipIdx}");
                Fleet.Add(ship);
                Worklog.Add(ship.Name, new List<Report>());
                ChroneManager.AddListener(ship);
            }
        }

        /*public static void AddReport(string shipName, Report report)
        {
            if (Worklog.ContainsKey(shipName))
            {
                Worklog[shipName].Add(report);
            }
        }*/

        public static void AssignTasks(List<Asteroid> asteroids) {
            for (int shipIdx = 0; shipIdx < Fleet.Count; ++shipIdx) {
                HarvesterShip ship = Fleet[shipIdx];

                if (ship.HarvesterState == AsteroidState.Idle) {
                    for (int astIdx = 0; astIdx < asteroids.Count; ++astIdx) {
                        if (asteroids[astIdx].State == AsteroidState.Idle) {
                            ship.Target(asteroids[astIdx]);
                            break;
                        }
                    }
                }
            }
        }

        public static void ShowFullWorklog() {
            Console.WriteLine("\nЖурнал работ");

            List<string> shipNames = new List<string>(Worklog.Keys);

            for (int shipIndex = 0; shipIndex < shipNames.Count; ++shipIndex) {
                string currentShipName = shipNames[shipIndex];
                List<Report> reports = Worklog[currentShipName];

                Console.WriteLine($"Корабль: {currentShipName}");

                for (int reportIndex = 0; reportIndex < reports.Count; ++reportIndex) {
                    Report report = reports[reportIndex];
                    Console.WriteLine($"Задание №{reportIndex + 1}, Добыто: {report.AmountMined}, Астероид: {report.AsteroidSpawnID}");
                }
            }

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
        }
    }
}