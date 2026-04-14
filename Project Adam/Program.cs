using System;
using System.Collections.Generic;
using AsteroidSimulator.Models;

namespace AsteroidSimulator {
  class Program {
    static void Main(string[] args) {
      int stepValue;
      int zeroValue;
      int chronCounter;
      int poolInitialSize;
      int startAsteroidsCount;
      int spawnIntervalTicks;
      int minSpawnAmount;
      int maxSpawnAmount;
      int warningThreshold;
      int warningStepValue;
      int worklogPrintInterval;
      int asteroidIndex;
      int newAsteroidsCount;
      int maxExclusiveValue;
      int warningBaseValue;
      Asteroid newAsteroid;
      List<Asteroid> depletedAsteroids;
      AsteroidEmitter asteroidPool;
      List<Asteroid> activeAsteroids;
      MotherShip motherStation;
      Random randomGenerator;

      stepValue = 1;
      zeroValue = 0;
      chronCounter = zeroValue;
      poolInitialSize = 10;
      startAsteroidsCount = 5;
      spawnIntervalTicks = 4;
      minSpawnAmount = 1;
      maxSpawnAmount = 2;
      warningStepValue = 5;
      worklogPrintInterval = 15;
      warningBaseValue = poolInitialSize + warningStepValue;
      warningThreshold = warningBaseValue;

      asteroidPool = new AsteroidEmitter(poolInitialSize);
      activeAsteroids = new List<Asteroid>();
      motherStation = new MotherShip(5);
      randomGenerator = new Random();

      for (asteroidIndex = zeroValue; asteroidIndex < startAsteroidsCount; asteroidIndex = asteroidIndex + stepValue) {
        newAsteroid = asteroidPool.Spawn();
        activeAsteroids.Add(newAsteroid);
      }

      while (true) {
        Console.Clear();
        chronCounter = chronCounter + stepValue;

        Console.WriteLine("===== CHRON #" + chronCounter + " =====");

        motherStation.OnChronTick(activeAsteroids);

        if (chronCounter % spawnIntervalTicks == zeroValue) {
          maxExclusiveValue = maxSpawnAmount + stepValue;
          newAsteroidsCount = randomGenerator.Next(minSpawnAmount, maxExclusiveValue);
          Console.WriteLine("Spawning " + newAsteroidsCount + " new asteroids!");

          for (asteroidIndex = zeroValue; asteroidIndex < newAsteroidsCount; asteroidIndex = asteroidIndex + stepValue) {
            newAsteroid = asteroidPool.Spawn();
            activeAsteroids.Add(newAsteroid);

            if (newAsteroid.CreateId > warningThreshold) {
              Console.WriteLine("WARNING: Pool was empty, created new asteroid.");
              warningThreshold = warningThreshold + warningStepValue;
            }
          }
        }

        depletedAsteroids = new List<Asteroid>();

        foreach (Asteroid currentAsteroid in activeAsteroids) {
          if (currentAsteroid.State == AsteroidState.Depleted) {
            depletedAsteroids.Add(currentAsteroid);
          }
        }

        foreach (Asteroid deadAsteroid in depletedAsteroids) {
          activeAsteroids.Remove(deadAsteroid);
          asteroidPool.Recycle(deadAsteroid);
        }

        Console.WriteLine("\nActive asteroids: " + activeAsteroids.Count);

        foreach (Asteroid currentAsteroid in activeAsteroids) {
          Console.WriteLine("  " + currentAsteroid.ToString());
        }

        Console.WriteLine("\nHarvester Fleet:");

        foreach (HarvesterShip currentShip in motherStation.Fleet) {
          Console.WriteLine("  " + currentShip.ToString());
        }

        Console.WriteLine("\nTotal mined (Echos):");

        foreach (KeyValuePair<string, int> miningEntry in motherStation.GetTotalMined()) {
          Console.WriteLine("  " + miningEntry.Key + ": " + miningEntry.Value);
        }

        if (chronCounter % worklogPrintInterval == zeroValue) {
          motherStation.PrintWorklog();
        }

        Console.WriteLine("\nPress Enter for next chron | ESC to exit");

        ConsoleKeyInfo pressedKey;
        pressedKey = Console.ReadKey(true);

        if (pressedKey.Key == ConsoleKey.Escape) {
          break;
        }
      }
    }
  }
}