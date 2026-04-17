using System;
using System.Collections.Generic;

namespace AsteroidSimulation {
  internal class Program {
    static void Main(string[] args) {
      int countChrons = 0;
      int countAsteroidItems = 5;
      int countSpawnID = 0;
      int minCorrectNum = 0;

      AsteroidEmitter asteroidItems = new AsteroidEmitter(countAsteroidItems);
      List<Asteroid> activeAsteroid = new List<Asteroid>();
      MotherShip motherShip = new MotherShip(5, 500, 50);
      bool isRun = true;
      Random random = new Random();

      motherShip.SetEmitter(asteroidItems);

      for (int indexI = 0; indexI < 3; ++indexI) {
        Asteroid newAsteroid = asteroidItems.Spawn();
        ++countSpawnID;
        newAsteroid.SetSpawnId(countSpawnID);
        activeAsteroid.Add(newAsteroid);
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

          if (countChrons % 5 == minCorrectNum && countChrons > 0) {
            int createdAsteroid = random.Next(1, 4);
            Console.WriteLine($"✨ NEW ASTEROIDS: +{createdAsteroid} ✨\n");

            for (int indexI = 0; indexI < createdAsteroid; ++indexI) {
              Asteroid newAsteroid = asteroidItems.Spawn();
              ++countSpawnID;
              newAsteroid.SetSpawnId(countSpawnID);
              activeAsteroid.Add(newAsteroid);
              motherShip.AddAsteroid(newAsteroid);
              ChroneManager.AddListener(newAsteroid);
            }
          }

          for (int indexI = activeAsteroid.Count - 1; indexI >= minCorrectNum; --indexI) {
            if (activeAsteroid[indexI].State == AsteroidState.Depleted) {
              ChroneManager.RemoveListener(activeAsteroid[indexI]);
              motherShip.RemoveAsteroid(activeAsteroid[indexI]);
              activeAsteroid.RemoveAt(indexI);
            }
          }

          PrintAsteroidsInfo(activeAsteroid);
          PrintHarvestersInfo(motherShip);
          motherShip.PrintTotalMined();

          if (countChrons % 15 == minCorrectNum && countChrons > 0) {
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

    static void PrintAsteroidsInfo(List<Asteroid> asteroids) {
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
        string asteroidInfo = harvester.currentAsteroid != null ? $" | Asteroid SpawnID: {harvester.currentAsteroid.SpawnId}" : "";
        Console.WriteLine($"{harvester.GetName()}: {status} | Cargo: {harvester.GetCargoCurrent()}/500{asteroidInfo}");
      }
      Console.WriteLine("----------------------------------------\n");
    }
  }
}