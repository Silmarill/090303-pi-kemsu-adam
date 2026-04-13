using System;
using System.Collections.Generic;

class Program {
  static void Main(string[] args) {
    const int initialPoolSize = 5;
    const int startAsteroidsCount = 3;
    const int spawnInterval = 5;
    const int minSpawn = 1;
    const int maxSpawn = 4;

    // Инициализация
    AsteroidEmitter emitter = new AsteroidEmitter(initialPoolSize);
    List<Asteroid> activeAsteroids = new List<Asteroid>();
    Random random = new Random();
    int chroneCounter = 0;

    // Создание станции-матриарха
    MotherShip motherShip = new MotherShip();

    // Начальное наполнение
    for (int i = 0; i < startAsteroidsCount; i++) {
      activeAsteroids.Add(emitter.Spawn());
    }

    // Для поиска целей
    motherShip.SetActiveAsteroids(activeAsteroids);

    PrintInfo(chroneCounter, activeAsteroids, motherShip);

    // Основной цикл
    while (true) {
      Console.WriteLine("\nНажмите [Enter] — следующий хрон | [R] — суммарная добыча | [Esc] — выход");
      ConsoleKey key = Console.ReadKey(true).Key;

      if (key == ConsoleKey.Escape) {
        break;
      }

      if (key == ConsoleKey.Enter) {
        chroneCounter++;
        bool isSpawnTick = (chroneCounter % spawnInterval == 0);

        ChroneManager.MakeChroneTick();

        if (isSpawnTick) {
          int count = random.Next(minSpawn, maxSpawn);
          for (int i = 0; i < count; i++) {
            activeAsteroids.Add(emitter.Spawn());
          }
        }

        // Удаление Depleted
        for (int i = activeAsteroids.Count - 1; i >= 0; i--) {
          if (activeAsteroids[i].State == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[i]);
            activeAsteroids.RemoveAt(i);
          }
        }

        // Автоматический вывод полного журнала на 15-м хроне
        if (chroneCounter % 15 == 0) {
          motherShip.PrintFullWorklog();
        }

        PrintInfo(chroneCounter, activeAsteroids, motherShip);
      }
      else if (key == ConsoleKey.R) {
        motherShip.PrintSummary();
      }
    }
  }

  static void PrintInfo(int chrone, List<Asteroid> activeAsteroids, MotherShip motherShip) {
    Console.Clear();
    Console.WriteLine($"=== Хрон: {chrone} === Активных астероидов: {activeAsteroids.Count} === Матриарх активен ===");
    Console.WriteLine(new string('-', 70));

    Console.WriteLine("=== АСТЕРОИДЫ ===");
    foreach (var ast in activeAsteroids) {
      ConsoleColor color = ast.State == AsteroidState.Mining ? ConsoleColor.Cyan :
                           ast.CurrentEchos <= 200 ? ConsoleColor.Red :
                           ast.CurrentEchos <= 500 ? ConsoleColor.DarkYellow : ConsoleColor.Green;

      Console.ForegroundColor = color;
      Console.WriteLine(ast);
      Console.ResetColor();
    }

    Console.WriteLine(new string('-', 70));
    Console.WriteLine("=== ФЛОТ ХАРВЕСТЕРОВ ===");

    foreach (var ship in motherShip.Fleet) {
      Console.WriteLine(ship);
    }

    // Краткий вывод на каждый хрон
    motherShip.PrintSummary();
  }
}