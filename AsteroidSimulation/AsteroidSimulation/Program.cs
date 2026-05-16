using System;
using System.Collections.Generic;

namespace AsteroidSimulation {
  internal class Program {
    static void Main(string[] args) {
      int countChrons = 0;
      int countSpawnID = 0;

      AsteroidEmitter asteroidItems = new AsteroidEmitter(5);
      MotherShip motherShip = new MotherShip(5, 500, 50);
      bool isRun = true;
      Random random = new Random();

      motherShip.SetEmitter(asteroidItems);

      for (int indexI = 0; indexI < 3; ++indexI) {
        Asteroid newAsteroid = asteroidItems.Spawn();
        ++countSpawnID;
        newAsteroid.SetSpawnId(countSpawnID);
        motherShip.AddAsteroid(newAsteroid);
        ChroneManager.AddListener(newAsteroid);
      }

      foreach (var harvester in motherShip.Fleet) {
        ChroneManager.AddListener(harvester);
      }

      Console.WriteLine("=== MATRIARCH STATION SIMULATION ===");
      Console.WriteLine("Press Enter for next chron");
      Console.WriteLine("Press R for total mined report");
      Console.WriteLine("Press Esc for exit");
      Console.WriteLine("=====================================\n");

      while (isRun) {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == ConsoleKey.Escape) {
          isRun = false;
        }
        else if (keyInfo.Key == ConsoleKey.Enter) {
          ++countChrons;
          Console.Clear();
          Console.WriteLine($"=== CHRON #{countChrons} ===\n");

          ChroneManager.MakeChroneTick();

          motherShip.UpdateHarvesters();

          if (countChrons % 5 == 0) {
            int createdAsteroid = random.Next(1, 4);
            Console.WriteLine($"✨ NEW ASTEROIDS: +{createdAsteroid} ✨\n");

            for (int indexI = 0; indexI < createdAsteroid; ++indexI) {
              Asteroid newAsteroid = asteroidItems.Spawn();
              ++countSpawnID;
              newAsteroid.SetSpawnId(countSpawnID);
              motherShip.AddAsteroid(newAsteroid);
              ChroneManager.AddListener(newAsteroid);
            }
          }

          List<Asteroid> activeAsteroids = motherShip.GetActiveAsteroids();
          for (int indexI = activeAsteroids.Count - 1; indexI >= 0; --indexI) {
            if (activeAsteroids[indexI].State == AsteroidState.Depleted) {
              motherShip.RemoveAsteroid(activeAsteroids[indexI]);
            }
          }

          PrintAsteroidsInfo(motherShip);
          PrintHarvestersInfo(motherShip);
          motherShip.PrintTotalMined();

          if (countChrons % 15 == 0) {
            motherShip.PrintWorkLog();
          }

          Console.WriteLine("=====================================");
          Console.WriteLine("Press Enter | R for report | Esc for exit");
        }
        else if (keyInfo.Key == ConsoleKey.R) {
          Console.Clear();
          Console.WriteLine($"=== CURRENT CHRON: {countChrons} ===\n");
          motherShip.PrintTotalMined();
          Console.WriteLine("Press Enter for next chron");
        }
      }
    }

    static void PrintAsteroidsInfo(MotherShip motherShip) {
      List<Asteroid> asteroids = motherShip.GetActiveAsteroids();
      Console.WriteLine($"\nACTIVE ASTEROIDS: {asteroids.Count}");
      Console.WriteLine("----------------------------------------");

      if (asteroids.Count == 0) {
        Console.WriteLine("No active asteroids");
      }
      else {
        for (int indexI = 0; indexI < asteroids.Count; ++indexI) {
          Console.WriteLine($"[{indexI + 1}] {asteroids[indexI]}");
        }
      }
      Console.WriteLine("----------------------------------------\n");
    }

    static void PrintHarvestersInfo(MotherShip motherShip) {
      Console.WriteLine("HARVESTER FLEET:");
      Console.WriteLine("----------------------------------------");

      foreach (var harvester in motherShip.Fleet) {
        string status = harvester.GetState() == HarvesterState.Mining ? "⛏️ Mining" : "💤 Idle";

        string asteroidInfo = harvester.CurrentAsteroid != null
          ? $" | Asteroid SpawnID: {harvester.CurrentAsteroid.SpawnId}"
          : "";

        Console.WriteLine($"{harvester.GetName()}: {status} | Cargo: {harvester.GetCargoCurrent()}/500 | Mined: {harvester.GetAsteroidsMined()}{asteroidInfo}");
      }
      Console.WriteLine("----------------------------------------\n");
    }
  }
}