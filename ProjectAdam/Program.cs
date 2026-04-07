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

      for (int i = 0; i < 3; i++) {
        Asteroid asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChroneManager.AddListener(asteroid);
      }

      Print(activeAsteroids);

    }



    static void Print(List<Asteroid> asteroids) {
      Console.WriteLine("\nActive asteroids: ");

      foreach (var asteroid in asteroids) {
        Console.WriteLine(
          "----------------" +
          $"\nCreateID: {asteroid.CreateID}" +
          $"\nSpawnID: {asteroid.SpawnID}" +
          $"\nEchos: {asteroid.CurrentEchos}" +
          $"\nState: {asteroid.State}"+
          "\n----------------\n");
      }
    }

  }
}