using System;
using System.Collections.Generic;
using AsteroidSimulation.Chron;
using AsteroidSimulation.Models;
using AsteroidSimulation.Pool;

namespace AsteroidSimulation {
  class Program {
    public static AsteroidEmitter asteroidEmitter;
    public static List<Asteroid> activeAsteroids;
    public static int chronCounter;
    public static int spawnInterval;
    public static int minSpawnCount;
    public static int maxSpawnCount;
    public static int rangeOffset;
    public static int lifeofAsteroids;

    static void Main(string[] args) {
      chronCounter = 0;
      spawnInterval = 5;
      minSpawnCount = 1;
      maxSpawnCount = 3;
      rangeOffset = 1;
      lifeofAsteroids = 5;

      Console.WriteLine("=== Симуляция астероидов с хронами ===");
      Console.WriteLine("Нажмите Enter для следующего хрона, Esc для выхода.\n");

      asteroidEmitter = new AsteroidEmitter(lifeofAsteroids);
      activeAsteroids = new List<Asteroid>();

      int initialAsteroidCount;
      Asteroid newAsteroid;

      initialAsteroidCount = 3;

      for (int asteroidIndex = 0; asteroidIndex < initialAsteroidCount; ++asteroidIndex) {
        newAsteroid = asteroidEmitter.Spawn();
        activeAsteroids.Add(newAsteroid);
        ChronManager.AddListener(newAsteroid);
      }

      while (true) {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == ConsoleKey.Escape) {
          break;
        } else if (keyInfo.Key == ConsoleKey.Enter) {
          ProcessChronTick();
        }
      }
    }

    private static void ProcessChronTick() {
      ++chronCounter;

      Asteroid newAsteroid;
      Asteroid currentAsteroid;
      Random randomGenerator;

      Console.WriteLine($"\n--- Хрон {chronCounter} ---");

      ChronManager.MakeChronTick();

      if (chronCounter % spawnInterval == 0) {
        randomGenerator = new Random();
        Console.WriteLine($"Появление {randomGenerator.Next(minSpawnCount, maxSpawnCount + rangeOffset)} новых астероидов!");

        for (int spawnIndex = 0; spawnIndex < randomGenerator.Next(minSpawnCount, maxSpawnCount + rangeOffset); ++spawnIndex) {
          newAsteroid = asteroidEmitter.Spawn();
          activeAsteroids.Add(newAsteroid);
          ChronManager.AddListener(newAsteroid);
        }
      }

      for (int depletedCheckIndex = activeAsteroids.Count - rangeOffset; depletedCheckIndex >= 0; --depletedCheckIndex) {
        currentAsteroid = activeAsteroids[depletedCheckIndex];

        if (currentAsteroid.State == AsteroidState.Depleted) {
          ChronManager.RemoveListener(currentAsteroid);
          asteroidEmitter.Recycle(currentAsteroid);
          activeAsteroids.RemoveAt(depletedCheckIndex);
        }
      }

      Console.Clear();
      Console.WriteLine($"=== Состояние системы (Хрон {chronCounter}) ===");
      Console.WriteLine($"Активных астероидов: {activeAsteroids.Count}\n");

      if (activeAsteroids.Count == 0) {
        Console.WriteLine("Нет активных астероидов.");
      } else {
        for (int printIndex = 0; printIndex < activeAsteroids.Count; ++printIndex) {
          activeAsteroids[printIndex].PrintInfo();
        }
      }

      Console.WriteLine("\nНажмите Enter для следующего хрона, Esc для выхода.");
    }
  }
}