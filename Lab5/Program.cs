using System;
using System.Collections.Generic;

class Program {
  static void Main() {
    var random = new Random();
    var asteroidEmitter = new AsteroidEmitter(5);
    var activeAsteroids = new List<Asteroid>();
    int chronCounter = 0;

    for (int index = 0; index < 3; ++index) {
      Asteroid asteroid = asteroidEmitter.Spawn();
      ChroneManager.AddListener(asteroid);
      activeAsteroids.Add(asteroid);
    }

    Console.Clear();
    Console.WriteLine("=== Asteroid Simulation ===");
    Console.WriteLine("Enter — next chron  |  Esc — exit\n");
    PrintActiveAsteroids(activeAsteroids, chronCounter);

    while (true) {
      ConsoleKeyInfo key = Console.ReadKey();

      if (key.Key == ConsoleKey.Escape) {
        Console.WriteLine("\nSimulation stopped.");
        break;
      }

      if (key.Key != ConsoleKey.Enter) {
        continue;
      }

      ++chronCounter;

      ChroneManager.MakeChroneTick();

      if (chronCounter % 5 == 0) {
        int spawnCount = random.Next(1, 4);
        for (int index = 0; index < spawnCount; ++index) {
          Asteroid asteroid = asteroidEmitter.Spawn();
          ChroneManager.AddListener(asteroid);
          activeAsteroids.Add(asteroid);
        }
      }

      var toRecycle = new List<Asteroid>();
      foreach (Asteroid asteroid in activeAsteroids) {
        if (asteroid.state == AsteroidState.Depleted) {
          toRecycle.Add(asteroid);
        }
      }
      foreach (Asteroid asteroid in toRecycle) {
        ChroneManager.RemoveListener(asteroid);
        asteroidEmitter.Recycle(asteroid);
        activeAsteroids.Remove(asteroid);
      }

      Console.Clear();
      PrintActiveAsteroids(activeAsteroids, chronCounter);
    }
  }

  static void PrintActiveAsteroids(List<Asteroid> activeAsteroids, int chron) {
    Console.WriteLine("=== Chron: " + chron + " | Active: " + activeAsteroids.Count + " ===\n");
    if (activeAsteroids.Count == 0) {
      Console.WriteLine("  (no active asteroids)");
    } else {
      foreach (Asteroid asteroid in activeAsteroids) {
        asteroid.PrintInfo();
      }
    }
    Console.WriteLine("\nEnter — next chron  |  Esc — exit");
  }
}
