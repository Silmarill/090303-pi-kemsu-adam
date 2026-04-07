using System;
using System.Collections.Generic;

namespace AsteroidSimulation {
  class Program {
    static void Main() {
      var emitter = new AsteroidEmitter(5);
      var activeAsteroids = new List<Asteroid>();
      int chrone = 0;
      var random = new Random();

      for (int i = 0; i < 3; ++i)
        activeAsteroids.Add(emitter.Spawn());

      while (true) {
        Console.Clear();
        Console.WriteLine($"Chrone: {chrone}");

        foreach (var a in activeAsteroids) {
          Console.WriteLine($"ID:{a.CreateID} Spawn:{a.SpawnID} Echos:{a.CurrentEchos}/{a.MaxEchos} State:{a.State}");
        }

        Console.WriteLine("\nEnter - next, Esc - exit");

        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.Escape) break;

        chrone++;

        ChroneManager.MakeChroneTick();

        if (chrone % 5 == 0) {
          int count = random.Next(1, 4);
          for (int i = 0; i < count; ++i)
            activeAsteroids.Add(emitter.Spawn());
        }

        for (int i = activeAsteroids.Count - 1; i >= 0; i--) {
          if (activeAsteroids[i].State == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[i]);
            activeAsteroids.RemoveAt(i);
          }
        }
      }
    }
  }
}