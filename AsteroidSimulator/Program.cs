using System;
using System.Collections.Generic;
using AsteroidSimulator.Asteroids;

namespace AsteroidSimulator;

class Program {
  private static AsteroidEmitter _asteroidEmitter;
  private static List<Asteroid> _activeAsteroids;
  private static int _chronCounter;

  private const int SpawnInterval = 5;
  private const int MinNewAsteroids = 1;
  private const int MaxNewAsteroids = 3;
  private const int InitialAsteroids = 3;

  static void Main(string[] args) {
    Console.Title = "Asteroid Simulator";

    InitializeSimulation();
    RunSimulationLoop();
  }

  static void InitializeSimulation() {
    _asteroidEmitter = new AsteroidEmitter();
    _activeAsteroids = new List<Asteroid>();
    _chronCounter = 0;

    for (int i = 0; i < InitialAsteroids; i++) {
      _activeAsteroids.Add(_asteroidEmitter.Spawn());
    }

    string header = "========================================\n" +
                   "         СИМУЛЯТОР АСТЕРОИДОВ\n" +
                   "========================================\n\n";

    Console.Write(header);

    DisplayAsteroidsInfo();

    string controls = "\n----------------------------------------\n" +
                     "Команды:\n" +
                     "  ENTER - Следующий хрон\n" +
                     "  ESC   - Выход\n" +
                     "----------------------------------------\n";

    Console.Write(controls);
  }

  static void RunSimulationLoop() {
    while (true) {
      Console.Write("Хрон #" + (_chronCounter + 1) + " готов. Нажмите ENTER для продолжения, ESC для выхода: ");

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
    _chronCounter = _chronCounter + 1;

    Console.Clear();

    string output = "========== ХРОН #" + _chronCounter + " ==========\n\n";

    foreach (Asteroid asteroid in _activeAsteroids) {
      asteroid.OnChronTick();
    }

    if (_chronCounter % SpawnInterval == 0) {
      Random random = new Random();
      int newCount = random.Next(MinNewAsteroids, MaxNewAsteroids + 1);
      output = output + "[СОБЫТИЕ] Появилось " + newCount + " новых астероидов!\n\n";

      for (int i = 0; i < newCount; i++) {
        _activeAsteroids.Add(_asteroidEmitter.Spawn());
      }
    }

    List<Asteroid> depleted = new List<Asteroid>();
    foreach (Asteroid asteroid in _activeAsteroids) {
      if (asteroid.State == AsteroidState.Depleted) {
        depleted.Add(asteroid);
      }
    }

    foreach (Asteroid asteroid in depleted) {
      _activeAsteroids.Remove(asteroid);
      _asteroidEmitter.Recycle(asteroid);
      output = output + "[УТИЛИЗАЦИЯ] Астероид #" + asteroid.CreateID + " (SpawnID:" + asteroid.SpawnID + ") истощён и возвращён в пул\n";
    }

    if (depleted.Count > 0) {
      output = output + "\n";
    }

    output = output + "--- АКТИВНЫЕ АСТЕРОИДЫ ---\n";

    if (_activeAsteroids.Count == 0) {
      output = output + "  Нет активных астероидов\n";
    } else {
      for (int index = 0; index < _activeAsteroids.Count; index++) {
        Asteroid asteroid = _activeAsteroids[index];
        output = output + $"  {index + 1,2}. ID:{asteroid.CreateID,8} | Спавн:{asteroid.SpawnID,8} | {asteroid.CurrentEchos,8}/{asteroid.MaxEchos,8} | {asteroid.State}\n";
      }
      output = output + "  Всего: " + _activeAsteroids.Count + " активных астероидов\n";
    }

    output = output + "\n--- СТАТИСТИКА ПУЛА ---\n";
    output = output + "  Доступно: " + _asteroidEmitter.AvailableCount + "\n";
    output = output + "  Всего создано: " + _asteroidEmitter.TotalCreated + "\n";
    output = output + "  Активных: " + _activeAsteroids.Count + "\n\n";
    output = output + "========================================\n";

    Console.Write(output);
  }

  static void DisplayAsteroidsInfo() {
    string output = "--- АКТИВНЫЕ АСТЕРОИДЫ ---\n";

    if (_activeAsteroids.Count == 0) {
      output = output + "  Нет активных астероидов\n";
    } else {
      for (int index = 0; index < _activeAsteroids.Count; index++) {
        Asteroid asteroid = _activeAsteroids[index];
        output = output + $"  {index + 1,2}. ID:{asteroid.CreateID,8} | Спавн:{asteroid.SpawnID,8} | {asteroid.CurrentEchos,8}/{asteroid.MaxEchos,8} | {asteroid.State}\n";
      }
      output = output + "  Всего: " + _activeAsteroids.Count + " активных астероидов\n";
    }

    Console.Write(output);
  }
}