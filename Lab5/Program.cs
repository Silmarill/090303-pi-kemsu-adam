using System;
using System.Collections.Generic;

class Program {
  static void Main() {
    var random = new Random();
    var asteroidEmitter = new AsteroidEmitter(5);
    var activeAsteroids = new List<Asteroid>();
    int chronCounter = 0;

    // инициализируем первые три астероида через пул
    for (int i = 0; i < 3; ++i) {
      Asteroid a = asteroidEmitter.Spawn();
      ChroneManager.AddListener(a);
      activeAsteroids.Add(a);
    }

    Console.Clear();
    Console.WriteLine("=== Asteroid Simulation ===");
    Console.WriteLine("Enter — next chron  |  Esc — exit\n");
    PrintActiveAsteroids(activeAsteroids, chronCounter);

    // цикл обработки хронов
    while (true) {
      ConsoleKeyInfo key = Console.ReadKey(intercept: true);

      if (key.Key == ConsoleKey.Escape) {
        Console.WriteLine("\nSimulation stopped.");
        break;
      }

      if (key.Key != ConsoleKey.Enter) {
        continue;
      }

      // 1. увеличить счётчик хронов
      ++chronCounter;

      // 2. вызвать OnChronTick() для каждого активного астероида через наблюдателя
      ChroneManager.MakeChroneTick();

      // 3. каждые 5 хронов — спавн 1–3 новых астероидов
      if (chronCounter % 5 == 0) {
        int spawnCount = random.Next(1, 4);
        for (int i = 0; i < spawnCount; ++i) {
          Asteroid a = asteroidEmitter.Spawn();
          ChroneManager.AddListener(a);
          activeAsteroids.Add(a);
        }
      }

      // 4. найти Depleted, вернуть в пул и удалить из активного списка
      var toRecycle = new List<Asteroid>();
      foreach (var a in activeAsteroids) {
        if (a.State == AsteroidState.Depleted) {
          toRecycle.Add(a);
        }
      }
      foreach (var a in toRecycle) {
        ChroneManager.RemoveListener(a);
        asteroidEmitter.Recycle(a);
        activeAsteroids.Remove(a);
      }

      // 5. обновить экран
      Console.Clear();
      PrintActiveAsteroids(activeAsteroids, chronCounter);
    }
  }

  static void PrintActiveAsteroids(List<Asteroid> activeAsteroids, int chron) {
    Console.WriteLine("=== Chron: " + chron + " | Active: " + activeAsteroids.Count + " ===\n");
    if (activeAsteroids.Count == 0) {
      Console.WriteLine("  (no active asteroids)");
    } else {
      foreach (var a in activeAsteroids) {
        a.PrintInfo();
      }
    }
    Console.WriteLine("\nEnter — next chron  |  Esc — exit");
  }
}
