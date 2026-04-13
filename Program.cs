using System;
using System.Collections.Generic;

class Program {
  static void Main(string[] args) {
    // Блок объявления
    int initialPoolSize;
    int startAsteroidsCount;
    int spawnInterval;
    int minSpawnCount;
    int maxSpawnCount;
    int fullLogInterval;
    int newAsteroidsCount;
    bool isSpawnTick;

    AsteroidEmitter emitter;
    List<Asteroid> activeAsteroids;
    Random random;
    int chroneCounter;
    MotherShip motherShip;

    // Блок инициализации
    initialPoolSize = 5;
    startAsteroidsCount = 3;
    spawnInterval = 5;
    minSpawnCount = 1;
    maxSpawnCount = 4;
    fullLogInterval = 15;

    emitter = new AsteroidEmitter(initialPoolSize);
    activeAsteroids = new List<Asteroid>();
    random = new Random();
    chroneCounter = 0;
    motherShip = new MotherShip();

    // Начальный спавн астероидов
    for (int asteroidIndex = 0; asteroidIndex < startAsteroidsCount; ++asteroidIndex) {
      activeAsteroids.Add(emitter.Spawn());
    }

    // Передача данных материнскому кораблю
    motherShip.SetActiveAsteroids(activeAsteroids);

    PrintInfo(chroneCounter, activeAsteroids, motherShip);

    // Основной цикл
    while (true) {
      Console.WriteLine("\nPress [Enter] — next chrone | [R] — summary | [Esc] — exit");

      ConsoleKey pressedKey = Console.ReadKey(true).Key;

      if (pressedKey == ConsoleKey.Escape) {
        break;
      }

      if (pressedKey == ConsoleKey.Enter) {
        // Увеличение хрона
        chroneCounter++;

        // Проверка спавна
        isSpawnTick = chroneCounter % spawnInterval == 0;

        // Обновление системы
        ChroneManager.MakeChroneTick();

        // Спавн новых астероидов
        if (isSpawnTick) {
          newAsteroidsCount = random.Next(minSpawnCount, maxSpawnCount);

          for (int spawnIndex = 0; spawnIndex < newAsteroidsCount; ++spawnIndex) {
            activeAsteroids.Add(emitter.Spawn());
          }
        }

        // Удаление истощенных астероидов
        for (int asteroidIndex = activeAsteroids.Count - 1; asteroidIndex >= 0; --asteroidIndex) {
          if (activeAsteroids[asteroidIndex].state == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[asteroidIndex]);
            activeAsteroids.RemoveAt(asteroidIndex);
          }
        }

        // Лог
        if (chroneCounter % fullLogInterval == 0) {
          motherShip.PrintFullWorklog();
        }

        PrintInfo(chroneCounter, activeAsteroids, motherShip);
      }
      else if (pressedKey == ConsoleKey.R) {
        motherShip.PrintSummary();
      }
    }
  }

  static void PrintInfo(int chrone, List<Asteroid> activeAsteroids, MotherShip motherShip) {
    // Очистка консоли для нового кадра
    Console.Clear();

    // Заголовок
    Console.WriteLine($"=== Chrone: {chrone} === Active asteroids: {activeAsteroids.Count} === MotherShip active ===\n" +
                      $"{new string('-', 70)}\n" +
                      $"=== ASTEROIDS ===");

    // Астероиды
    for (int asteroidIndex = 0; asteroidIndex < activeAsteroids.Count; ++asteroidIndex) {
      Asteroid asteroid = activeAsteroids[asteroidIndex];

      ConsoleColor asteroidColor;

      if (asteroid.state == AsteroidState.Mining) {
        asteroidColor = ConsoleColor.Cyan;
      }
      else if (asteroid.currentEchos <= 200) {
        asteroidColor = ConsoleColor.Red;
      }
      else if (asteroid.currentEchos <= 500) {
        asteroidColor = ConsoleColor.DarkYellow;
      }
      else {
        asteroidColor = ConsoleColor.Green;
      }

      Console.ForegroundColor = asteroidColor;
      Console.WriteLine(asteroid);
      Console.ResetColor();
    }

    // Флот
    Console.WriteLine($"{new string('-', 70)}\n=== HARVESTER FLEET ===");

    for (int shipIndex = 0; shipIndex < motherShip.fleet.Count; ++shipIndex) {
      HarvesterShip ship = motherShip.fleet[shipIndex];
      Console.WriteLine(ship);
    }

    // Сводка
    motherShip.PrintSummary();
  }
}