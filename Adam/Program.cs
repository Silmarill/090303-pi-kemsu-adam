using Adam;
using System;
using System.Collections.Generic;

namespace Asteroids {
  internal class Program {
    static void Main(string[] args) {
      int poolInitialSize = 5;
      int startingAsteroidsCount = 3;
      int spawnInterval = 5;
      int minSpawnAmount = 1;
      int maxSpawnAmount = 3;

      AsteroidEmitter asteroidEmitter = new AsteroidEmitter(poolInitialSize);
      List<Asteroid> activeAsteroidsList = new List<Asteroid>();
      Random randomGenerator = new Random();

      MotherShip motherShip = new MotherShip(5, 500, 50);
      motherShip.SetEmitter(asteroidEmitter);

      for (int asteroidIndex = 0; asteroidIndex < startingAsteroidsCount; asteroidIndex++) {
        Asteroid newAsteroid = asteroidEmitter.Spawn();
        activeAsteroidsList.Add(newAsteroid);
        motherShip.AddAsteroid(newAsteroid);
      }

      Console.WriteLine("=== ASTEROIDS: MATRIARCH STATION ===");
      Console.WriteLine("[Enter] Next chron | [R] Worklog | [Esc] Exit\n");

      bool isProgramRunning = true;

      while (isProgramRunning) {
        Console.Clear();
        Console.WriteLine($"=== CHRON #{ChronoManager.CurrentTick} ===\n");

        motherShip.PrintAsteroidsInfo();
        Console.WriteLine();
        motherShip.PrintHarvestersInfo();
        Console.WriteLine();
        motherShip.PrintTotalMined();
        Console.WriteLine($"\nActive: {motherShip.GetActiveAsteroidsCount()} | Pool: {asteroidEmitter.AvailableCount}");
        Console.Write("\n[Enter] Next | [R] Full Worklog | [Esc] Exit: ");

        ConsoleKeyInfo pressedKey = Console.ReadKey(true);

        if (pressedKey.Key == ConsoleKey.Escape) {
          isProgramRunning = false;
        } else if (pressedKey.Key == ConsoleKey.R) {
          motherShip.PrintFullWorklog();
          Console.WriteLine("\nPress any key...");
          Console.ReadKey(true);
        } else if (pressedKey.Key == ConsoleKey.Enter) {

          ChronoManager.MakeChronTick();

          if (ChronoManager.CurrentTick % spawnInterval == 0) {
            int newAsteroidsAmount = randomGenerator.Next(minSpawnAmount, maxSpawnAmount + 1);

            for (int asteroidIndex = 0; asteroidIndex < newAsteroidsAmount; asteroidIndex++) {
              Asteroid newAsteroid = asteroidEmitter.Spawn();
              activeAsteroidsList.Add(newAsteroid);
              motherShip.AddAsteroid(newAsteroid);
            }
          }

          List<Asteroid> depletedAsteroidsList = new List<Asteroid>();

          foreach (Asteroid currentAsteroid in activeAsteroidsList) {
            if (currentAsteroid.State == AsteroidState.Depleted) {
              depletedAsteroidsList.Add(currentAsteroid);
            }
          }

          foreach (Asteroid depletedAsteroid in depletedAsteroidsList) {
            activeAsteroidsList.Remove(depletedAsteroid);
          }

          if (ChronoManager.CurrentTick % 15 == 0) {
            Console.Clear();
            Console.WriteLine($"=== CHRON #{ChronoManager.CurrentTick} - WORKLOG ===\n");
            motherShip.PrintFullWorklog();
            Console.WriteLine("\nPress any key...");
            Console.ReadKey(true);
          }
        }
      }

      ChronoManager.ClearAllListeners();
      Console.WriteLine("\nProgram finished.");
    }
  }
} 