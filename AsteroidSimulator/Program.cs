using System;
using System.Collections.Generic;
using AsteroidSimulator.Asteroids;
using AsteroidSimulator.Core;

namespace AsteroidSimulator;

class Program {
  private static AsteroidEmitter _asteroidEmitter;
  private static List<Asteroid> _activeAsteroids;
  private static int _chronCounter;

  static void Main(string[] args) {
    Console.Title = "Asteroid Simulator";

    InitializeSimulation();
    RunSimulationLoop();
  }

  static void InitializeSimulation() {
    _asteroidEmitter = new AsteroidEmitter(5);
    _activeAsteroids = new List<Asteroid>();
    _chronCounter = 0;

    for (int i = 0; i < 3; i++) {
      _activeAsteroids.Add(_asteroidEmitter.Spawn());
    }

    Console.WriteLine("========================================\n         СИМУЛЯТОР АСТЕРОИДОВ\n========================================\n");
    DisplayAsteroidsInfo();
    Console.WriteLine("\n----------------------------------------\nКоманды:\n  ENTER - Следующий хрон\n  ESC   - Выход\n----------------------------------------\n");
  }

  static void RunSimulationLoop() {
    while (true) {
      Console.Write($"Хрон #{_chronCounter + 1} готов. Нажмите ENTER для продолжения, ESC для выхода: ");

      ConsoleKeyInfo keyInfo = Console.ReadKey(true);

      if (keyInfo.Key == ConsoleKey.Escape) {
        Console.WriteLine("\n\nДо свидания!");
        break;
      }

      if (keyInfo.Key == ConsoleKey.Enter) {
        ProcessChronTick();
      }
    }
  }

  static void ProcessChronTick() {
    _chronCounter++;

    Console.Clear();

    string output = $"========== ХРОН #{_chronCounter} ==========\n\n";

    foreach (var asteroid in _activeAsteroids) {
      asteroid.OnChronTick();
    }

    if (_chronCounter % 5 == 0) {
      Random random = new Random();
      int newCount = random.Next(1, 4);
      output += $"[СОБЫТИЕ] Появилось {newCount} новых астероидов!\n\n";

      for (int i = 0; i < newCount; i++) {
        _activeAsteroids.Add(_asteroidEmitter.Spawn());
      }
    }

    List<Asteroid> depleted = new List<Asteroid>();
    foreach (var asteroid in _activeAsteroids) {
      if (asteroid.State == AsteroidState.Depleted) {
        depleted.Add(asteroid);
      }
    }

    foreach (var asteroid in depleted) {
      _activeAsteroids.Remove(asteroid);
      _asteroidEmitter.Recycle(asteroid);
      output += $"[УТИЛИЗАЦИЯ] Астероид #{asteroid.CreateID} истощён\n";
    }

    if (depleted.Count > 0) {
      output += "\n";
    }

    output += "--- АКТИВНЫЕ АСТЕРОИДЫ ---\n";
    if (_activeAsteroids.Count == 0) {
      output += "  Нет активных астероидов\n";
    } else {
      for (int i = 0; i < _activeAsteroids.Count; i++) {
        var ast = _activeAsteroids[i];
        output += $"  {i + 1}. ID:{ast.CreateID,3} | Спавн:{ast.SpawnID,3} | {ast.CurrentEchos,4}/{ast.MaxEchos,4} | {ast.State}\n";
      }
      output += $"  Всего: {_activeAsteroids.Count} активных астероидов\n";
    }

    output += $"\n--- СТАТИСТИКА ПУЛА ---\n";
    output += $"  Доступно: {_asteroidEmitter.AvailableCount}\n";
    output += $"  Всего создано: {_asteroidEmitter.TotalCreated}\n";
    output += $"  Активных: {_activeAsteroids.Count}\n\n";
    output += "========================================\n";

    Console.WriteLine(output);
  }

  static void DisplayAsteroidsInfo() {
    string output = "--- АКТИВНЫЕ АСТЕРОИДЫ ---\n";

    if (_activeAsteroids.Count == 0) {
      output += "  Нет активных астероидов\n";
    } else {
      for (int i = 0; i < _activeAsteroids.Count; i++) {
        var ast = _activeAsteroids[i];
        output += $"  {i + 1}. ID:{ast.CreateID,3} | Спавн:{ast.SpawnID,3} | {ast.CurrentEchos,4}/{ast.MaxEchos,4} | {ast.State}\n";
      }
      output += $"  Всего: {_activeAsteroids.Count} активных астероидов\n";
    }

    Console.Write(output);
  }
}