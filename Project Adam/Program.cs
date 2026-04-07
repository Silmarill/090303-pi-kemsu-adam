using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_Adam {
  class Program {
    static void Main() {
      AsteroidEmitter asteroidEmitter = new AsteroidEmitter(5);
      List<Asteroid> activeAsteroids = new List<Asteroid>();
      int chronCounter = 0;
      Random random = new Random();

      for (int asteroidIndex = 0; asteroidIndex < 3; ++asteroidIndex) {
        Asteroid newAsteroid = asteroidEmitter.Spawn();
        activeAsteroids.Add(newAsteroid);
        ChroneManager.AddListener(newAsteroid);
      }

      while (true) {
        Console.Clear();
        Console.WriteLine($"Chrone #{chronCounter}");
        Console.WriteLine($"Active asteroids: {activeAsteroids.Count}");
        Console.WriteLine();

        foreach (Asteroid asteroid in activeAsteroids) {
          Console.WriteLine($"Asteroid #{asteroid.SpawnID} (CreateID:{asteroid.CreateID}) | Echos: {asteroid.CurrentEchos}/{asteroid.MaxEchos} | State: {asteroid.State}");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter for next chrone, Esc to exit");
        ConsoleKeyInfo pressedKey = Console.ReadKey(true);

        if (pressedKey.Key == ConsoleKey.Escape) {
          break;
        }

        if (pressedKey.Key == ConsoleKey.Enter) {
          ++chronCounter;
          ChroneManager.MakeChroneTick();

          if (chronCounter % 5 == 0) {
            int newAsteroidsCount = random.Next(1, 4);
            for (int asteroidIndex = 0; asteroidIndex < newAsteroidsCount; ++asteroidIndex) {
              Asteroid newAsteroid = asteroidEmitter.Spawn();
              activeAsteroids.Add(newAsteroid);
              ChroneManager.AddListener(newAsteroid);
            }
          }

          List<Asteroid> depletedAsteroids = activeAsteroids.Where(asteroid => asteroid.State == AsteroidState.Depleted).ToList();

          foreach (Asteroid depletedAsteroid in depletedAsteroids) {
            ChroneManager.RemoveListener(depletedAsteroid);
            asteroidEmitter.Recycle(depletedAsteroid);
            activeAsteroids.Remove(depletedAsteroid);
          }
        }
      }
    }
  }
}