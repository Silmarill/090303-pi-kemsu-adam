using System;
using System.Collections.Generic;

class Program {
  static void Main(string[] args) {
    const int initialPoolSize = 5;
    const int startAsteroidsCount = 3;
    const int spawnInterval = 5;
    const int minSpawn = 1;
    const int maxSpawn = 4;

    AsteroidEmitter emitter = new AsteroidEmitter(initialPoolSize);
    List<Asteroid> activeAsteroids = new List<Asteroid>();
    Random random = new Random();
    int chroneCounter = 0;

    // Начальное наполнение активного списка
    for (int asteroidIndex = 0; asteroidIndex < startAsteroidsCount; ++asteroidIndex) {
      activeAsteroids.Add(emitter.Spawn());
    }

    PrintInfo(chroneCounter, activeAsteroids);

    // Главный цикл программы
    while (true) {
      Console.WriteLine("\nНажмите [Enter] для следующего хрона или [Esc] для выхода...");
      ConsoleKey key = Console.ReadKey(true).Key;

      if (key == ConsoleKey.Escape) {
        break;
      }

      if (key == ConsoleKey.Enter) {
        chroneCounter++;
        bool isSpawnTick = (chroneCounter % spawnInterval == 0);

        // Уведомление всех астероидов о новом хроне
        ChroneManager.MakeChroneTick();

        // Спавн новых астероидов каждые 5 хронов
        if (isSpawnTick) {
          int newAsteroidsCount = random.Next(minSpawn, maxSpawn);

          for (int spawnIndex = 0; spawnIndex < newAsteroidsCount; ++spawnIndex) {
            activeAsteroids.Add(emitter.Spawn());
          }
        }

        // Возврат истощённых астероидов в пул (обратный цикл для безопасного удаления)
        for (int asteroidIndex = activeAsteroids.Count - 1; asteroidIndex >= 0; --asteroidIndex) {
          if (activeAsteroids[asteroidIndex].State == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[asteroidIndex]);
            activeAsteroids.RemoveAt(asteroidIndex);
          }
        }

        PrintInfo(chroneCounter, activeAsteroids);
      }
    }
  }

  static void PrintInfo(int chrone, List<Asteroid> activeAsteroids) {
    const int lineLength = 50;
    const int criticalResourceLevel = 200;
    const int mediumResourceLevel = 500;

    Console.Clear();
    Console.WriteLine($"=== Хрон: {chrone} === Активных астероидов: {activeAsteroids.Count}");
    Console.WriteLine(new string('-', lineLength));

    if (activeAsteroids.Count == 0) {
      Console.WriteLine("Нет активных астероидов.");
    } else {
      foreach (var asteroid in activeAsteroids) {
        if (asteroid.CurrentEchos <= criticalResourceLevel) {
          Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (asteroid.CurrentEchos <= mediumResourceLevel) {
          Console.ForegroundColor = ConsoleColor.DarkYellow;
        }
        else {
          Console.ForegroundColor = ConsoleColor.Green;
        }

        Console.WriteLine(asteroid.ToString());
        Console.ResetColor();
      }
    }

    Console.WriteLine(new string('-', lineLength));
  }
}