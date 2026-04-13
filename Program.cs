using System;
using System.Collections.Generic;

class Program {
  static void Main(string[] args) {
    // Объявление
    int initialPoolSize;
    int startAsteroidsCount;
    int spawnInterval;
    int minSpawnCount;
    int maxSpawnCount;
    int fullLogInterval;
    int newAsteroidsCount;
    int chroneCounter;
    bool isSpawnTick;

    // Поля
    AsteroidEmitter emitter;
    List<Asteroid> activeAsteroids;
    Random random;
    MotherShip motherShip;

    // Инициализация
    initialPoolSize = 5;
    startAsteroidsCount = 3;
    spawnInterval = 5;
    minSpawnCount = 1;
    maxSpawnCount = 4;
    fullLogInterval = 15;

    // Экземпляры
    emitter = new AsteroidEmitter(initialPoolSize);
    activeAsteroids = new List<Asteroid>();
    random = new Random();
    chroneCounter = 0;
    motherShip = new MotherShip();

    // Начальный спавн астероидов
    for (int asteroidIndex = 0; asteroidIndex < startAsteroidsCount; ++asteroidIndex) {
      activeAsteroids.Add(emitter.Spawn());
    }

    // Передача активных астероидов материнскому кораблю
    motherShip.SetActiveAsteroids(activeAsteroids);

    PrintInfo(chroneCounter, activeAsteroids, motherShip);

    // Основной цикл
    while (true) {
      Console.WriteLine("\nPress [Enter] — next chrone | [R] — summary | [Esc] — exit");

      ConsoleKey pressedKey = Console.ReadKey(true).Key;

      // Выход из программы
      if (pressedKey == ConsoleKey.Escape) {
        break;
      }

      // Следующий хрон
      if (pressedKey == ConsoleKey.Enter) {
        chroneCounter++;

        isSpawnTick = chroneCounter % spawnInterval == 0;

        ChroneManager.MakeChroneTick();

        // Спавн новых астероидов
        if (isSpawnTick) {
          newAsteroidsCount = random.Next(minSpawnCount, maxSpawnCount);

          // Гарантия, что в пуле всегда будет хотя бы 1 астероид, если наступает спавн-тик
          for (int spawnIndex = 0; spawnIndex < newAsteroidsCount; ++spawnIndex) {
            activeAsteroids.Add(emitter.Spawn());
          }
        }

        // Ресайклинг исчерпанных астероидов
        for (int asteroidIndex = activeAsteroids.Count - 1; asteroidIndex >= 0; --asteroidIndex) {
          if (activeAsteroids[asteroidIndex].state == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[asteroidIndex]);
            activeAsteroids.RemoveAt(asteroidIndex);
          }
        }

        // Печать полного ворклога каждые fullLogInterval хронов
        if (chroneCounter % fullLogInterval == 0) {
          motherShip.PrintFullWorklog();
        }

        PrintInfo(chroneCounter, activeAsteroids, motherShip);
      } else if (pressedKey == ConsoleKey.R) {
        motherShip.PrintSummary();
      }
    }
  }

  static void PrintInfo(int chrone, List<Asteroid> activeAsteroids, MotherShip motherShip) {
    Console.Clear();

    Console.WriteLine(
      $"=== Chrone: {chrone} === Active asteroids: {activeAsteroids.Count} === MotherShip active ===\n" +
      $"{new string('-', 70)}\n" +
      $"=== ASTEROIDS ==="
    );

    // Печать информации об астероидах с цветовой индикацией состояния
    for (int asteroidIndex = 0; asteroidIndex < activeAsteroids.Count; ++asteroidIndex) {
      Asteroid asteroid = activeAsteroids[asteroidIndex];

      ConsoleColor asteroidColor;

      if (asteroid.state == AsteroidState.Mining) {
        asteroidColor = ConsoleColor.Cyan;
      } else if (asteroid.currentEchos <= 200) {
        asteroidColor = ConsoleColor.Red;
      } else if (asteroid.currentEchos <= 500) {
        asteroidColor = ConsoleColor.DarkYellow;
      } else {
        asteroidColor = ConsoleColor.Green;
      }

      Console.ForegroundColor = asteroidColor;
      Console.WriteLine(asteroid);
      Console.ResetColor();
    }

    Console.WriteLine($"{new string('-', 70)}\n=== HARVESTER FLEET ===");

    // Печать информации о кораблях с цветовой индикацией состояния
    for (int shipIndex = 0; shipIndex < motherShip.fleet.Count; ++shipIndex) {
      HarvesterShip ship = motherShip.fleet[shipIndex];
      Console.WriteLine(ship);
    }

    motherShip.PrintSummary();
  }
}