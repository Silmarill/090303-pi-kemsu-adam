using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAdam {
  internal class Program {
    static void Main(string[] args) {
      AsteroidEmitter emitter = new AsteroidEmitter(5);
      List<Asteroid> activeAsteroids = new List<Asteroid>();

      int chroneCount = 0;

      for (int i = 0; i < 3; ++i) {
        Asteroid asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChroneManager.AddListener(asteroid);
      }

      Print(activeAsteroids);

      Console.WriteLine("\nEnter: next chrone, Esc: exit");

      while (true) {
        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape)
          break;

        if (key.Key == ConsoleKey.Enter) {
          ++chroneCount;

          Console.WriteLine($"\nChrone: {chroneCount}");

          ChroneManager.MakeChroneTick();

          for (int i = activeAsteroids.Count - 1; i >= 0; --i) {
            Asteroid asteroid = activeAsteroids[i];

            if (asteroid.State == AsteroidState.Depleted) {
              Console.WriteLine($"Asteroid {asteroid.CreateID} depleted and then removed ");

              ChroneManager.RemoveListener(asteroid);
              emitter.Recycle(asteroid);
              activeAsteroids.RemoveAt(i);
            }
          }

          if (chroneCount % 5 == 0) {
            Console.WriteLine("New asteroids: ");
            Random rand = new Random();
            int spawnCount = rand.Next(1, 4);

            for (int i = 0; i < spawnCount; ++i) {
              Asteroid asteroid = emitter.Spawn();
              activeAsteroids.Add(asteroid);
              ChroneManager.AddListener(asteroid);

              Console.WriteLine($"Spawned Asteroid CreateID: {asteroid.CreateID}, SpawnID: {asteroid.SpawnID}");
            }
          }

          Print(activeAsteroids);

        }
      }
    }

    static void Print(List<Asteroid> asteroids) {
      Console.WriteLine("\nActive asteroids: ");

      foreach (var asteroid in asteroids) {
        Console.WriteLine(
          "----------------" +
          $"\nCreateID: {asteroid.CreateID}" +
          $"\nSpawnID: {asteroid.SpawnID}" +
          $"\nEchos: {asteroid.CurrentEchos}" +
          $"\nState: {asteroid.State}" +
          "\n----------------\n");
      }
    }
  }
}