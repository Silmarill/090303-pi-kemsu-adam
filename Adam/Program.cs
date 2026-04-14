using System;
using System.Collections.Generic;
using AsteroidSimulation.Chron;
using AsteroidSimulation.Models;
using AsteroidSimulation.Pool;

namespace AsteroidSimulation
{
  class Program
  {
    private static AsteroidEmitter asteroidEmitter;
    private static List<Asteroid> activeAsteroids;
    private static int chronCounter;
    private static int spawnInterval;
    private static int minSpawnCount;
    private static int maxSpawnCount;

    static void Main(string[] args)
    {
      chronCounter = 0;
      spawnInterval = 5;
      minSpawnCount = 1;
      maxSpawnCount = 3;

      Console.WriteLine("=== Симуляция астероидов с хронами ===");
      Console.WriteLine("Нажмите Enter для следующего хрона, Esc для выхода.\n");

      asteroidEmitter = new AsteroidEmitter(5);
      activeAsteroids = new List<Asteroid>();

      int initialAsteroidCount = 3;
      int asteroidIndex;

      for (asteroidIndex = 0; asteroidIndex < initialAsteroidCount; ++asteroidIndex)
      {
        Asteroid newAsteroid = asteroidEmitter.Spawn();
        activeAsteroids.Add(newAsteroid);
        ChronManager.AddListener(newAsteroid);
      }

      while (true)
      {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == ConsoleKey.Escape)
        {
          break;
        }
        else if (keyInfo.Key == ConsoleKey.Enter)
        {
          ProcessChronTick();
        }
      }
    }

    int depletedCheckIndex;
    int spawnCount;
    int spawnIndex;
    int printIndex;

    private static void ProcessChronTick()
    {
      ++chronCounter;

      Console.WriteLine($"\n--- Хрон {chronCounter} ---");

      ChronManager.MakeChronTick();

      if (chronCounter % spawnInterval == 0)
      {
        Random randomGenerator = new Random();
        spawnCount = randomGenerator.Next(minSpawnCount, maxSpawnCount + 1);

        Console.WriteLine($"Появление {spawnCount} новых астероидов!");

        for (spawnIndex = 0; spawnIndex < spawnCount; ++spawnIndex)
        {
          Asteroid newAsteroid = asteroidEmitter.Spawn();
          activeAsteroids.Add(newAsteroid);
          ChronManager.AddListener(newAsteroid);
        }
      }

      for (depletedCheckIndex = activeAsteroids.Count - 1; depletedCheckIndex >= 0; --depletedCheckIndex)
      {
        Asteroid currentAsteroid = activeAsteroids[depletedCheckIndex];

        if (currentAsteroid.State == AsteroidState.Depleted)
        {
          ChronManager.RemoveListener(currentAsteroid);
          asteroidEmitter.Recycle(currentAsteroid);
          activeAsteroids.RemoveAt(depletedCheckIndex);
        }
      }

      Console.Clear();
      Console.WriteLine($"=== Состояние системы (Хрон {chronCounter}) ===");
      Console.WriteLine($"Активных астероидов: {activeAsteroids.Count}\n");

      if (activeAsteroids.Count == 0)
      {
        Console.WriteLine("Нет активных астероидов.");
      }
      else
      {

        for (printIndex = 0; printIndex < activeAsteroids.Count; ++printIndex)
        {
          Asteroid currentAsteroid = activeAsteroids[printIndex];
          currentAsteroid.PrintInfo();
        }
      }

      Console.WriteLine("\nНажмите Enter для следующего хрона, Esc для выхода.");
    }
  }
}