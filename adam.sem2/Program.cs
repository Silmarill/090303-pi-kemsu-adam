using System;
using System.Collections.Generic;
using AsteroidSimulator.Models;
using AsteroidSimulator.Managers;

namespace AsteroidSimulator {
  class Program {
    public static AsteroidEmitter AsteroidEmitter;
    public static List<Asteroid> ActiveAsteroids;
    public static int ChronCounter;
    public static Random RandomGenerator;

    static void Main(string[] args) {
      int poolInitialSize;
      int startAsteroidsCount;
      int spawnInterval;
      int minNewAsteroids;
      int maxNewAsteroids;
      int asteroidIndex;
      int newAsteroidsCount;
      int depletedIndex;
      Asteroid newAsteroid;
      List<Asteroid> depletedAsteroids;
      ConsoleKeyInfo pressedKey;

      Console.OutputEncoding = System.Text.Encoding.UTF8;

      poolInitialSize = 5;
      startAsteroidsCount = 3;
      spawnInterval = 5;
      minNewAsteroids = 1;
      maxNewAsteroids = 3;

      AsteroidEmitter = new AsteroidEmitter(poolInitialSize);
      ActiveAsteroids = new List<Asteroid>();
      RandomGenerator = new Random();
      ChronCounter = 0;

      for (asteroidIndex = 0; asteroidIndex < startAsteroidsCount; ++asteroidIndex) {
        newAsteroid = AsteroidEmitter.Spawn();
        ActiveAsteroids.Add(newAsteroid);
        ChronManager.AddListener(newAsteroid);
      }

      while (true) {
        int chronIncrement;

        chronIncrement = 1;

        Console.Clear();
        ChronCounter = ChronCounter + chronIncrement;

        Console.WriteLine("CHRON #" + ChronCounter);

        ChronManager.MakeChronTick();

        if (ChronCounter % spawnInterval == 0) {
          int rangeOffset;

          rangeOffset = 1;
          newAsteroidsCount = RandomGenerator.Next(minNewAsteroids, maxNewAsteroids + rangeOffset);
          Console.WriteLine("Spawning " + newAsteroidsCount + " new asteroids!");

          for (asteroidIndex = 0; asteroidIndex < newAsteroidsCount; ++asteroidIndex) {
            newAsteroid = AsteroidEmitter.Spawn();
            ActiveAsteroids.Add(newAsteroid);
            ChronManager.AddListener(newAsteroid);
          }
        }

        depletedAsteroids = new List<Asteroid>();

        foreach (Asteroid currentAsteroid in ActiveAsteroids) {
          if (currentAsteroid.State == AsteroidState.Depleted) {
            depletedAsteroids.Add(currentAsteroid);
          }
        }

        for (depletedIndex = 0; depletedIndex < depletedAsteroids.Count; ++depletedIndex) {
          Asteroid asteroidToRemove;

          asteroidToRemove = depletedAsteroids[depletedIndex];
          ActiveAsteroids.Remove(asteroidToRemove);
          ChronManager.RemoveListener(asteroidToRemove);
          AsteroidEmitter.Recycle(asteroidToRemove);
        }

        Console.WriteLine("Active asteroids: " + ActiveAsteroids.Count);
        Console.WriteLine("Pool size: " + AsteroidEmitter.GetPoolSize());
        Console.WriteLine("--- Active asteroids ---");

        if (ActiveAsteroids.Count == 0) {
          Console.WriteLine("(no active asteroids)");
        }
        else {
          foreach (Asteroid currentAsteroid in ActiveAsteroids)
          {
            Console.WriteLine(currentAsteroid.ToString());
          }
        }

        Console.WriteLine("Press Enter for next chron | ESC to exit");

        pressedKey = Console.ReadKey(true);
        if (pressedKey.Key == ConsoleKey.Escape) {
          break;
        }
      }
    }
  }
}