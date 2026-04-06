using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp16 {
  class Program {
    static void Main() {
      var emitter = new AsteroidEmitter(5);

      List<Asteroid> activeAsteroids = new List<Asteroid>();

      for (int asteroidIndex = 0; asteroidIndex < 3; asteroidIndex++) { 
        var asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChroneManager.AddListener(asteroid); 
        Console.WriteLine($"Asteroid ID: {asteroid.CreateID}, MaxEchos: {asteroid.MaxEchos}, CurrentEchos: {asteroid.CurrentEchos}, State: {asteroid.State}, SpawnID: {asteroid.SpawnID}");
      }

      int chronCount = 0;

      Console.WriteLine("\nНажмите Enter для перехода к следующему хрону, Esc — для выхода.");

      while (true) {
        var key = Console.ReadKey().KeyChar;

        if (key == 27) {
          break;
        }

        if (key == 13) {
          chronCount++;
          ProcessChron(chronCount, emitter, activeAsteroids);
          DisplayAsteroids(activeAsteroids);
        }
      }
    }

    static void ProcessChron(int chronCount, AsteroidEmitter emitter, List<Asteroid> activeAsteroids) {
      ChroneManager.MakeChroneTick(); 

      var depletedAsteroids = activeAsteroids.Where(a => a.State == AsteroidState.Depleted).ToList(); 
      foreach (var asteroid in depletedAsteroids) {
        activeAsteroids.Remove(asteroid);
        emitter.Recycle(asteroid);
        ChroneManager.RemoveListener(asteroid);
        Console.WriteLine($"Астероид {asteroid.CreateID} исчерпан и возвращён в пул.");
      }

      if (chronCount % 5 == 0) {
        var random = new Random();
        var newCount = random.Next(1, 4); 
        for (int asteroidIndex = 0; asteroidIndex < newCount; ++asteroidIndex) {
          var newAsteroid = emitter.Spawn();
          activeAsteroids.Add(newAsteroid);
          ChroneManager.AddListener(newAsteroid); 
          Console.WriteLine($"Спавн нового астероида: ID={newAsteroid.CreateID}, Echos={newAsteroid.CurrentEchos}");
        }
      }

      Console.WriteLine($"Хрон {chronCount} обработан.");
    }

    static void DisplayAsteroids(List<Asteroid> activeAsteroids) {
      Console.WriteLine("\nАктивные астероиды:");
      foreach (var asteroid in activeAsteroids) {
        Console.WriteLine($"ID: {asteroid.CreateID}, Echos: {asteroid.CurrentEchos}/{asteroid.MaxEchos}, Состояние: {asteroid.State}, SpawnID: {asteroid.SpawnID}");
      }

      Console.WriteLine();
    }
  }
}
