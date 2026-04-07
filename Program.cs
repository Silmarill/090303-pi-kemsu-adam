using System;
using System.Collections.Generic;

class Program {
  static void Main(string[] args) {
    const int initialPoolSize = 5;
    const int startAsteroidsCount = 3;
    const int spawnInterval = 5;
    const int minSpawn = 1;
    const int maxSpawn = 4;

    AsteroidEmitter emitter;
    List<Asteroid> activeAsteroids;
    Random random;
    int chroneCounter;
    int asteroidIndex;

    emitter = new AsteroidEmitter(initialPoolSize);
    activeAsteroids = new List<Asteroid>();
    random = new Random();
    chroneCounter = 0;

    // Начальное наполнение активного списка
    for (asteroidIndex = 0; asteroidIndex < startAsteroidsCount; ++asteroidIndex) {
      activeAsteroids.Add(emitter.Spawn());
    }

    PrintInfo(chroneCounter, activeAsteroids);

    // Главный цикл программы
    while (true) {
      ConsoleKey key;

      Console.WriteLine("\nНажмите [Enter] для следующего хрона или [Esc] для выхода...");
      key = Console.ReadKey(true).Key;

      // Выход из программы при нажатии [Esc]
      if (key == ConsoleKey.Escape) {
        break;
      }

      // Обработка такта времени при нажатии [Enter]
      if (key == ConsoleKey.Enter) {
        int newAsteroidsCount;
        bool isSpawnTick;

        chroneCounter++;
        isSpawnTick = (chroneCounter % spawnInterval == 0);

        // Синхронизация объектов с тактом времени
        ChroneManager.MakeChroneTick();

        if (isSpawnTick) {
          int spawnIndex;
          newAsteroidsCount = random.Next(minSpawn, maxSpawn);

          for (spawnIndex = 0; spawnIndex < newAsteroidsCount; ++spawnIndex) {
            activeAsteroids.Add(emitter.Spawn());
          }
        }

        // Возврат истощенных объектов в пул
        for (asteroidIndex = activeAsteroids.Count - 1; asteroidIndex >= 0; --asteroidIndex) {
          if (activeAsteroids[asteroidIndex].State == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[asteroidIndex]);
            activeAsteroids.RemoveAt(asteroidIndex);
          }
        }

        PrintInfo(chroneCounter, activeAsteroids);
      }
    }
  }

  // Метод для отображения информации о текущем состоянии программы
  static void PrintInfo(int chrone, List<Asteroid> activeAsteroids) {
    const int lineLength = 50;
    const int criticalResourceLevel = 200;
    const int mediumResourceLevel = 500;

    Console.Clear();
    Console.WriteLine($"=== Хрон: {chrone} ==="+
    "Активных астероидов: {activeAsteroids.Count}"+
    new string('-', lineLength));

    // Вывод информации о каждом активном астероиде с цветовой индикацией уровня ресурсов
    if (activeAsteroids.Count == 0) {
      Console.WriteLine("Нет активных астероидов.");
    }
    else {
      foreach (var currentAsteroid in activeAsteroids) {
        // Установка цвета в зависимости от остатка ресурсов
        if (currentAsteroid.CurrentEchos <= criticalResourceLevel) {
          Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (currentAsteroid.CurrentEchos <= mediumResourceLevel) {
          Console.ForegroundColor = ConsoleColor.DarkYellow;
        }
        else {
          Console.ForegroundColor = ConsoleColor.Green;
        }

        Console.WriteLine(currentAsteroid.ToString());
        Console.ResetColor();
      }
    }

    Console.WriteLine(new string('-', lineLength));
  }
}