using System;
using System.Collections.Generic;
using System.Linq;
using AsteroidSimulator.Models;

namespace ProjectAdam {
  class Program {
    private const int InitialAsteroidCount = 3;
    private const int AsteroidPoolSize = 10;
    private const int AsteroidSpawnInterval = 5;
    private const int MinAsteroidsToSpawn = 1;
    private const int MaxAsteroidsToSpawnExclusive = 4;

    static void Main(string[] args) {
      AsteroidEmitter emitter = new AsteroidEmitter(AsteroidPoolSize);
      List<Asteroid> activeAsteroids = new List<Asteroid>();
      MotherShip motherShip = new MotherShip();

      int chronCounter = 0;

      for (int i = 0; i < InitialAsteroidCount; i++) {
        Asteroid asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChronManager.AddListener(asteroid);
      }

      Console.Clear();
      Console.WriteLine("=== ECHO MINING SIMULATION ===");
      Console.WriteLine("Press Enter for next chron, Esc to exit\n");

      motherShip.PrintStatus(activeAsteroids);

      while (true) {
        ConsoleKeyInfo key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape) {
          break;
        }

        if (key.Key != ConsoleKey.Enter) {
          continue;
        }

        chronCounter++;

        ChronManager.MakeChronTick();
        motherShip.OnChronTick(activeAsteroids);

        if (chronCounter % AsteroidSpawnInterval == 0) {
          int spawnCount = new Random().Next(MinAsteroidsToSpawn, MaxAsteroidsToSpawnExclusive);
          for (int i = 0; i < spawnCount; i++) {
            Asteroid newAsteroid = emitter.Spawn();
            activeAsteroids.Add(newAsteroid);
            ChronManager.AddListener(newAsteroid);
            Console.WriteLine($"[{chronCounter}] New asteroid #{newAsteroid.SpawnID} appeared!");
          }
        }

        List<Asteroid> depletedAsteroids = activeAsteroids.Where(a => a.State == AsteroidState.Depleted).ToList();
        foreach (var asteroid in depletedAsteroids) {
          ChronManager.RemoveListener(asteroid);
          emitter.Recycle(asteroid);
          activeAsteroids.Remove(asteroid);
          Console.WriteLine($"[{chronCounter}] Asteroid #{asteroid.SpawnID} depleted and removed");
        }

        Console.Clear();
        Console.WriteLine($"=== CHRON {chronCounter} ===\n");

        motherShip.PrintStatus(activeAsteroids);

        if (chronCounter == 15) {
          motherShip.PrintWorklog();
          Console.WriteLine("Reached chron 15. Press Esc to exit or Enter to continue...");
        }
      }

      Console.WriteLine("\nSimulation ended.");
      motherShip.PrintWorklog();
    }
  }
}