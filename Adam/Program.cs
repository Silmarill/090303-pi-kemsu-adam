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
      int chronTickCounter = 0;
      Random randomGenerator = new Random();

      for (int asteroidIndex = 0; asteroidIndex < startingAsteroidsCount; asteroidIndex++) {
        Asteroid newAsteroid = asteroidEmitter.Spawn();
        activeAsteroidsList.Add(newAsteroid);
      }

      Console.WriteLine("=== ASTEROIDS ===\n[Enter] Next\n[Esc] Exit\n");

      bool isProgramRunning = true;

      while (isProgramRunning) {
        Console.Clear();
        Console.WriteLine("=== ACTIVE ASTEROIDS ===\n");

        if (activeAsteroidsList.Count == 0) {
          Console.WriteLine("No active asteroids");
        } else {
          foreach (Asteroid currentAsteroid in activeAsteroidsList) {
            currentAsteroid.PrintInfo();
          }
        }

        Console.WriteLine($"\nChron #{chronTickCounter} | Active: {activeAsteroidsList.Count} | Pool: {asteroidEmitter.AvailableCount}");
        Console.Write("[Enter] Next chron | [Esc] Exit: ");

        ConsoleKeyInfo pressedKey = Console.ReadKey(true);

        if (pressedKey.Key == ConsoleKey.Escape) {
          isProgramRunning = false;
        } else if (pressedKey.Key == ConsoleKey.Enter) {
          chronTickCounter++;

          foreach (Asteroid asteroid in activeAsteroidsList) {
            asteroid.OnChronTick();
          }

          if (chronTickCounter % spawnInterval == 0) {
            int newAsteroidsAmount = randomGenerator.Next(minSpawnAmount, maxSpawnAmount + 1);

            for (int asteroidIndex = 0; asteroidIndex < newAsteroidsAmount; asteroidIndex++) {
              Asteroid newAsteroid = asteroidEmitter.Spawn();
              activeAsteroidsList.Add(newAsteroid);
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
            asteroidEmitter.Recycle(depletedAsteroid);
          }
        }
      }

      Console.WriteLine("\nProgram finished.");
    }
  }
} 