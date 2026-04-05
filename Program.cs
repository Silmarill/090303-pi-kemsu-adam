using System;
using System.Collections.Generic;

class Program {
  static void Main(string[] args) {
    AsteroidEmitter emitter = new AsteroidEmitter(5);
    List<Asteroid> activeAsteroids = new List<Asteroid>();
    int chroneCounter = 0;
    Random random = new Random();

    // Нулевой хрон: спавн первых 3х астероидов
    for (int i = 0; i < 3; i++) {
      activeAsteroids.Add(emitter.Spawn());
    }

    PrintInfo(chroneCounter, activeAsteroids);

    while (true) {
      Console.WriteLine("\nНажмите [Enter] для следующего хрона или [Esc] для выхода...");
      var key = Console.ReadKey(true).Key;

      if (key == ConsoleKey.Escape) {
        break;
      }

      if (key == ConsoleKey.Enter) {
        chroneCounter++;

        // Оповещение всех подписанных объектов о такте времени (паттерн Наблюдателя)
        ChroneManager.MakeChroneTick();

        // Спавн новых астероидов каждые 5 хронов
        if (chroneCounter % 5 == 0) {

          // От 1 до 3
          int newAsteroidsCount = random.Next(1, 4);

          for (int i = 0; i < newAsteroidsCount; i++) {
            activeAsteroids.Add(emitter.Spawn());
          }
        }

        // Поиск и утилизация истощенных астероидов (реверс)
        for (int i = activeAsteroids.Count - 1; i >= 0; i--) {
          if (activeAsteroids[i].State == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[i]);
            activeAsteroids.RemoveAt(i);
          }
        }

        // Отрисовка
        PrintInfo(chroneCounter, activeAsteroids);
      }
    }
  }

  // Метод для отображения информации о текущем состоянии астероидов
  static void PrintInfo(int chrone, List<Asteroid> activeAsteroids) {
    Console.Clear();
    Console.WriteLine($"=== Хрон: {chrone} ===");
    Console.WriteLine($"Активных астероидов: {activeAsteroids.Count}");
    Console.WriteLine(new string('-', 50));

    if (activeAsteroids.Count == 0) {
      Console.WriteLine("Нет активных астероидов.");
    } else {
      foreach (var ast in activeAsteroids) {
        // Подсвечиваем цветом для красоты
        if (ast.CurrentEchos <= 200) {
          Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (ast.CurrentEchos <= 500) {
          Console.ForegroundColor = ConsoleColor.DarkYellow;
        }
        else {
          Console.ForegroundColor = ConsoleColor.Green;
        }

        Console.WriteLine(ast.ToString());
        Console.ResetColor();
      }
    }
    Console.WriteLine(new string('-', 50));
  }
}