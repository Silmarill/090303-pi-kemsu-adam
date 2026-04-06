using System;
using System.Collections.Generic;
using AsteroidSimulator.Models;
using AsteroidSimulator.Interfaces;
using AsteroidSimulator.Managers;

namespace AsteroidSimulator {
  class Program {
    public static AsteroidEmitter s_asteroidEmitter;
    public static List<Asteroid> s_activeAsteroids;
    public static int s_chronCounter;
    public static Random s_random = new Random();

    static void Main(string[] args)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      int poolInitialSize;
      int startAsteroidsCount;
      int spawnInterval;
      int minNewAsteroids;
      int maxNewAsteroids;

      poolInitialSize = 5;
      startAsteroidsCount = 3;
      spawnInterval = 5;
      minNewAsteroids = 1;
      maxNewAsteroids = 3;

      s_chronCounter = 0;

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

        ChronManager.MakeChronTick();

        if (s_chronCounter % spawnInterval == 0)
        {
          int newAsteroidsCount = s_random.Next(minNewAsteroids, maxNewAsteroids);
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