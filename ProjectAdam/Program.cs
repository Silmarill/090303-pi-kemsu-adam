using System;
using System.Collections.Generic;

namespace ProjectAdam {
  class Program {
    static void PrintActive(IReadOnlyList<Asteroid> asteroids) {
      if (asteroids.Count == 0) {
        Console.WriteLine("(no active asteroids)");
        return;
      }

      for (int asteroidIndex = 0; asteroidIndex < asteroids.Count; ++asteroidIndex) {
        asteroids[asteroidIndex].PrintInfo();
      }
    }

    static void Main(string[] args) {
      AsteroidEmitter emitter;
      emitter = new AsteroidEmitter(5);

      List<Asteroid> activeAsteroids;
      activeAsteroids = new List<Asteroid>();

      int chronCounter;
      chronCounter = 0;

      Asteroid asteroid;
      int spawnIndex;
      for (spawnIndex = 0; spawnIndex < 3; ++spawnIndex) {
        asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChronManager.AddListener(asteroid);
      }

      Console.Clear();
      Console.WriteLine("Chron: 0 (initial state)");
      PrintActive(activeAsteroids);

      while (true) {
        Console.WriteLine();
        Console.WriteLine("Press Enter for next chron, Esc to exit.");
        ConsoleKeyInfo key;
        key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape) {
          break;
        }

        if (key.Key != ConsoleKey.Enter) {
          continue;
        }

        ++chronCounter;

        ChronManager.MakeChronTick();

        if (chronCounter % 5 == 0) {
          int spawnCount;
          spawnCount = new Random().Next(1, 4);
          Asteroid spawnedAsteroid;
          int batchIndex;
          for (batchIndex = 0; batchIndex < spawnCount; ++batchIndex) {
            spawnedAsteroid = emitter.Spawn();
            activeAsteroids.Add(spawnedAsteroid);
            ChronManager.AddListener(spawnedAsteroid);
          }
        }

        List<Asteroid> depletedAsteroids;
        depletedAsteroids = new List<Asteroid>();
        Asteroid scanned;
        int scanIndex;
        for (scanIndex = 0; scanIndex < activeAsteroids.Count; ++scanIndex) {
          scanned = activeAsteroids[scanIndex];
          if (scanned.State == AsteroidState.Depleted) {
            depletedAsteroids.Add(scanned);
          }
        }

        Asteroid depleted;
        int depletedIndex;
        for (depletedIndex = 0; depletedIndex < depletedAsteroids.Count; ++depletedIndex) {
          depleted = depletedAsteroids[depletedIndex];
          ChronManager.RemoveListener(depleted);
          emitter.Recycle(depleted);
          activeAsteroids.Remove(depleted);
        }

        Console.Clear();
        Console.WriteLine("Chron: " + chronCounter);
        PrintActive(activeAsteroids);
      }
    }
  }
}