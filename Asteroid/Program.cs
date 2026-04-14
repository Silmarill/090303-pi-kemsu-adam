using System;
using System.Collections.Generic;

namespace Asteroid
{
  class Program {
    static void Main() {
      Console.Title = "Asteroid Simulation with Harvesters";
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      int initialPoolSize;
      initialPoolSize = 10;

      AsteroidEmitter asteroidEmitter;
      asteroidEmitter = new AsteroidEmitter(initialPoolSize);

      List<Asteroid> activeAsteroids;
      activeAsteroids = new List<Asteroid>();

      int initialActiveCount;
      initialActiveCount = 5;

      for (int asteroidIndex = 0; asteroidIndex < initialActiveCount; ++asteroidIndex) {
        Asteroid newAsteroid;
        newAsteroid = asteroidEmitter.Spawn();

        ChronManager.AddListener(newAsteroid);
        activeAsteroids.Add(newAsteroid);
      }

      MotherShip motherShip;
      motherShip = new MotherShip(asteroidEmitter, activeAsteroids);

      Random randomGenerator;
      randomGenerator = new Random();

      int spawnInterval;
      spawnInterval = 5;

      int minSpawnCount;
      minSpawnCount = 1;

      int maxSpawnCount;
      maxSpawnCount = 3;

      int fullWorklogChron;
      fullWorklogChron = 15;

      bool isRunning;
      isRunning = true;

      Console.WriteLine(
        "=== ASTEROID SIMULATION WITH HARVESTERS ===" +
        $"Initial pool size: {initialPoolSize}" +
        "Harvester fleet: 5 ships (Capacity: 500, BiteSize: 50)" +
        "\nPress ENTER for next chron, R for harvester totals, ESC to exit\n"
        );

      while (isRunning) {
        ConsoleKeyInfo keyInfo;
        keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == ConsoleKey.Escape) {
          isRunning = false;

          break;
        }

        if (keyInfo.Key == ConsoleKey.R) {
          motherShip.PrintHarvesterTotals();

          continue;
        }

        if (keyInfo.Key != ConsoleKey.Enter) {
          continue;
        }

        ChronManager.MakeChronTick();

        int currentChron;
        currentChron = ChronManager.GetCurrentChron();

        int increasingSpawn;
        increasingSpawn = 1;

        if (currentChron % spawnInterval == 0 && currentChron > 0) {
          int spawnCount;
          spawnCount = randomGenerator.Next(minSpawnCount, maxSpawnCount + increasingSpawn);

          Console.WriteLine($"\n>>> Chron {currentChron}: spawning {spawnCount} new asteroid(s)");

          for (int newIndex = 0; newIndex < spawnCount; ++newIndex) {
            Asteroid newAsteroid;
            newAsteroid = asteroidEmitter.Spawn();

            ChronManager.AddListener(newAsteroid);
            activeAsteroids.Add(newAsteroid);
          }
        }

        List<Asteroid> depletedAsteroids;
        depletedAsteroids = new List<Asteroid>();

        foreach (Asteroid asteroid in activeAsteroids) {
          if (asteroid.state == AsteroidState.Depleted) {
            depletedAsteroids.Add(asteroid);
          }
        }

        foreach (Asteroid depleted in depletedAsteroids) {
          ChronManager.RemoveListener(depleted);
          asteroidEmitter.Recycle(depleted);
          activeAsteroids.Remove(depleted);
        }

        Console.Clear();

        Console.WriteLine(
          $"=== CHRON {currentChron} ===" +
          $"Active asteroids: {activeAsteroids.Count}" +
          $"Available in pool: {asteroidEmitter.GetAvailableCount()}" +
          "\n--- ACTIVE ASTEROIDS ---"
          );

        int increasingAstInd;
        increasingAstInd = 1;

        if (activeAsteroids.Count == 0) {
          Console.WriteLine("  No active asteroids");
        } 
        
        else {
          for (int asteroidIndex = 0; asteroidIndex < activeAsteroids.Count; ++asteroidIndex) {
            int displayNumber;
            displayNumber = asteroidIndex + increasingAstInd;

            Console.WriteLine($"  {displayNumber,2}. {activeAsteroids[asteroidIndex]}");
          }
        }

        motherShip.PrintFleetStatus();
        motherShip.PrintHarvesterTotals();

        if (currentChron % fullWorklogChron == 0 && currentChron > 0) {
          motherShip.PrintFullWorklog();
        }

        Console.WriteLine(
          "\n--- POOL STATUS ---" +
          $"  Available: {asteroidEmitter.GetAvailableCount()}" +
          $"  Total spawned: {currentChron}" +
          "\nPress ENTER for next chron, R for totals, ESC to exit"
          );
      }

      Console.Clear();
      Console.WriteLine(
        "=== SIMULATION ENDED ===" +
        $"Total chrons passed: {ChronManager.GetCurrentChron()}" +
        "\nPress any key to exit..."
        );
      Console.ReadKey();
    }
  }
}
