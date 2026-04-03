using System;
using System.Collections.Generic;
using AsteroidSimulator.Models;
using AsteroidSimulator.Interfaces;
using AsteroidSimulator.Managers;

namespace AsteroidSimulator {
  class Program {
    private static AsteroidEmitter s_asteroidEmitter;
    private static List<Asteroid> s_activeAsteroids;
    private static int s_chronCounter = 0;
    private static Random s_random = new Random();

    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      int poolInitialSize = 5;
      int startAsteroidsCount = 3;
      int spawnInterval = 5;
      int minNewAsteroids = 1;
      int maxNewAsteroids = 3;

      s_asteroidEmitter = new AsteroidEmitter(poolInitialSize);
      s_activeAsteroids = new List<Asteroid>();

      for (int asteroidIndex = 0; asteroidIndex < startAsteroidsCount; ++asteroidIndex)
      {
        Asteroid newAsteroid = s_asteroidEmitter.Spawn();
        s_activeAsteroids.Add(newAsteroid);
        ChronManager.AddListener(newAsteroid);
      }

      while (true)
      {
        Console.Clear();
        ++s_chronCounter;

        Console.WriteLine($"CHRON #{s_chronCounter}");
        Console.WriteLine("=========================================");

        ChronManager.MakeChronTick();

        if (s_chronCounter % spawnInterval == 0)
        {
          int newAsteroidsCount = s_random.Next(minNewAsteroids, maxNewAsteroids + 1);
          Console.WriteLine($"Spawning {newAsteroidsCount} new asteroids!");

          for (int asteroidIndex = 0; asteroidIndex < newAsteroidsCount; ++asteroidIndex)
          {
            Asteroid newAsteroid = s_asteroidEmitter.Spawn();
            s_activeAsteroids.Add(newAsteroid);
            ChronManager.AddListener(newAsteroid);
          }
        }

        List<Asteroid> depletedAsteroids = new List<Asteroid>();

        foreach (var asteroid in s_activeAsteroids)
        {
          if (asteroid.State == AsteroidState.Depleted)
          {
            depletedAsteroids.Add(asteroid);
          }
        }

        foreach (var asteroid in depletedAsteroids)
        {
          s_activeAsteroids.Remove(asteroid);
          ChronManager.RemoveListener(asteroid);
          s_asteroidEmitter.Recycle(asteroid);
        }

        Console.WriteLine($"Active asteroids: {s_activeAsteroids.Count}");
        Console.WriteLine($"Pool size: {s_asteroidEmitter.PoolSize}");
        Console.WriteLine("--- Active asteroids ---");

        if (s_activeAsteroids.Count == 0)
        {
          Console.WriteLine("(no active asteroids)");
        }
        else
        {
          foreach (var asteroid in s_activeAsteroids)
          {
            Console.WriteLine(asteroid.ToString());
          }
        }

        Console.WriteLine("=========================================");
        Console.WriteLine("Press Enter for next chron | ESC to exit");

        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Escape)
        {
          break;
        }
      }
    }
  }
}