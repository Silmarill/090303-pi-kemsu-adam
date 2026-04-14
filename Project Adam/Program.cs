using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Adam {
  class Program {
    public static int MinAsteroidsPerSpawn = 1;
    public static int MaxAsteroidsPerSpawn = 4;
    public static int FifthSpawnIntervalInChrones = 5;
    public static int InitialAsteroidsCount = 3;
    public static int InitialPoolSize = 5;
    public static int FullWorklogChrone = 15;


    static void Main() {
      AsteroidEmitter asteroidEmitter = new AsteroidEmitter(InitialPoolSize);
      List<Asteroid> activeAsteroids = new List<Asteroid>();
      MotherShip motherShip = new MotherShip();
      int chroneCounter = 0;
      int nextJobNumber = 1;
      Random random = new Random();

      for (int asteroidIndex = 0; asteroidIndex < InitialAsteroidsCount; ++asteroidIndex) {
        Asteroid newAsteroid = asteroidEmitter.Spawn();
        activeAsteroids.Add(newAsteroid);
        ChroneManager.AddListener(newAsteroid);
      }

      while (true) {
        Console.Clear();
        Console.WriteLine($"Chrone #{chroneCounter}");
        Console.WriteLine($"Active asteroids: {activeAsteroids.Count}");
        Console.WriteLine();

        foreach (Asteroid asteroid in activeAsteroids) {
          Console.WriteLine($"Asteroid #{asteroid.SpawnID} (CreateID:{asteroid.CreateID}) | Echos: {asteroid.CurrentEchos}/{asteroid.MaxEchos} | State: {asteroid.State}");
        }

        Console.WriteLine();
        foreach (HarvesterShip ship in motherShip.Fleet) {
          Console.WriteLine($"{ship.Name} | Status: {ship.State} | Cargo: {ship.CargoCurrent}/{ship.CargoCapacity} | Mined: {ship.AsteroidsMined}");
        }

        Console.WriteLine();
        foreach (HarvesterShip ship in motherShip.Fleet) {
          int totalMined = motherShip.Worklog[ship.Name].Sum(report => report.AmountMined);
          Console.WriteLine($"{ship.Name}: {totalMined}");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter for next chrone, R for worklog, Esc to exit");
        ConsoleKeyInfo pressedKey = Console.ReadKey(true);

        if (pressedKey.Key == ConsoleKey.Escape) {
          break;
        }

        if (pressedKey.Key == ConsoleKey.R) {
          foreach (HarvesterShip ship in motherShip.Fleet) {
            Console.WriteLine($"\n{ship.Name}:");
            foreach (Report report in motherShip.Worklog[ship.Name]) {
              Console.WriteLine($"Job #{report.JobNumber} | Asteroid #{report.AsteroidSpawnID} | Mined: {report.AmountMined}");
            }
          }
          Console.WriteLine("\nPress any key to continue...");
          Console.ReadKey(true);
          continue;
        }

        if (pressedKey.Key == ConsoleKey.Enter) {
          ++chroneCounter;
          ChroneManager.MakeChroneTick();

          if (chroneCounter % FifthSpawnIntervalInChrones == 0 && chroneCounter > 0) {
            int newAsteroidsCount = random.Next(MinAsteroidsPerSpawn, MaxAsteroidsPerSpawn);
            for (int asteroidIndex = 0; asteroidIndex < newAsteroidsCount; ++asteroidIndex) {
              Asteroid newAsteroid = asteroidEmitter.Spawn();
              activeAsteroids.Add(newAsteroid);
              ChroneManager.AddListener(newAsteroid);
            }
          }

          foreach (HarvesterShip ship in motherShip.Fleet) {
            if (ship.State == HarvesterState.Mining) {
              Asteroid targetAsteroid = activeAsteroids.FirstOrDefault(asteroid => asteroid.SpawnID == ship.CurrentAsteroidSpawnID);
              if (targetAsteroid != null) {
                ship.Mine(targetAsteroid);
              }
            }
          }

          foreach (HarvesterShip ship in motherShip.Fleet) {
            if (ship.State == HarvesterState.Mining) {
              Asteroid targetAsteroid = activeAsteroids.FirstOrDefault(asteroid => asteroid.SpawnID == ship.CurrentAsteroidSpawnID);
              if (ship.CargoCurrent >= ship.CargoCapacity || (targetAsteroid != null && targetAsteroid.State == AsteroidState.Depleted)) {
                Report newReport = new Report(nextJobNumber, ship.CurrentAsteroidSpawnID, ship.CargoCurrent);
                motherShip.Worklog[ship.Name].Add(newReport);
                ++nextJobNumber;
                ship.CargoCurrent = 0;
                ++ship.AsteroidsMined;
                ship.State = HarvesterState.Idle;
                ship.CurrentAsteroidSpawnID = -1;
              }
            }
          }

          List<Asteroid> depletedAsteroids = activeAsteroids.Where(asteroid => asteroid.State == AsteroidState.Depleted).ToList();
          foreach (Asteroid depletedAsteroid in depletedAsteroids) {
            ChroneManager.RemoveListener(depletedAsteroid);
            asteroidEmitter.Recycle(depletedAsteroid);
            activeAsteroids.Remove(depletedAsteroid);
          }

          List<HarvesterShip> idleShips = motherShip.Fleet.Where(ship => ship.State == HarvesterState.Idle).ToList();
          List<Asteroid> idleAsteroids = activeAsteroids.Where(asteroid => asteroid.State == AsteroidState.Idle).ToList();

          for (int shipIndex = 0; shipIndex < idleShips.Count && shipIndex < idleAsteroids.Count; ++shipIndex) {
            idleShips[shipIndex].State = HarvesterState.Mining;
            idleShips[shipIndex].CurrentAsteroidSpawnID = idleAsteroids[shipIndex].SpawnID;
            idleAsteroids[shipIndex].State = AsteroidState.Mining;
          }

          if (chroneCounter == FullWorklogChrone) {
            Console.Clear();
            Console.WriteLine($"Chrone #{chroneCounter} - FULL WORKLOG:");
            foreach (HarvesterShip ship in motherShip.Fleet) {
              Console.WriteLine($"\n{ship.Name}:");
              foreach (Report report in motherShip.Worklog[ship.Name]) {
                Console.WriteLine($"Job #{report.JobNumber} | Asteroid #{report.AsteroidSpawnID} | Mined: {report.AmountMined}");
              }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
          }
        }
      }
    }
  }
}