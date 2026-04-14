using System;
using System.Collections.Generic;
using System.Linq;

internal class Program {
  static void Main(string[] args) {
    AsteroidEmitter asteroidEmitter = new AsteroidEmitter(5);
    List<Asteroid> activeAsteroids = new List<Asteroid>();
    int chroneCounter = 0;
    Random random = new Random();

    Asteroid asteroid1 = asteroidEmitter.Spawn();
    activeAsteroids.Add(asteroid1);
    ChroneManager.AddListener(asteroid1);
    Asteroid asteroid2 = asteroidEmitter.Spawn();
    activeAsteroids.Add(asteroid2);
    ChroneManager.AddListener(asteroid2);
    Asteroid asteroid3 = asteroidEmitter.Spawn();
    activeAsteroids.Add(asteroid3);
    ChroneManager.AddListener(asteroid3);

    foreach (Asteroid asteroid in activeAsteroids) {
      asteroid.PrintInfo();
    }

    while (true) {
      Console.Clear();

      foreach (Asteroid asteroid in activeAsteroids) {
        if (asteroid.State == AsteroidState.Idle) {
          asteroid.OnChroneTick();
        }
      }


      if (chroneCounter > 0 && chroneCounter % 5 == 0) {
        int count = random.Next(1, 4);
        for (int spawnedCount = 0; spawnedCount < count; spawnedCount++) {
          Asteroid newAsteroid = asteroidEmitter.Spawn();
          activeAsteroids.Add(newAsteroid);
          ChroneManager.AddListener(newAsteroid);
        }
      }

      foreach (Asteroid asteroid in activeAsteroids.ToList()) {
        if (asteroid.State == AsteroidState.Depleted) {
          asteroidEmitter.Recycle(asteroid);
          activeAsteroids.Remove(asteroid);
          ChroneManager.RemoveListener(asteroid);
        }
      }

      chroneCounter++;

      foreach (Asteroid asteroid in activeAsteroids) {
        asteroid.PrintInfo();
      }

    

      ConsoleKeyInfo key = Console.ReadKey(true);

      if (key.Key == ConsoleKey.Escape) {
        break;
      } 
      
    }
  }
}