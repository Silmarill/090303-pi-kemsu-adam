using System;
using System.Collections.Generic;
using AsteroidSimulator.Asteroids;
using AsteroidSimulator.Ships;

namespace AsteroidSimulator;

class Program {
  private static AsteroidEmitter _asteroidEmitter;
  private static List<Asteroid> _activeAsteroids;
  private static MotherShip _motherShip;
  private static int _chronCounter;

  private const int SpawnInterval = 5;
  private const int WorklogChron = 15;
  private const int MinNewAsteroids = 1;
  private const int MaxNewAsteroids = 3;
  private const int InitialAsteroids = 3;

  static void Main(string[] args) {
    Console.Title = "Asteroid Simulator v2.0";
    Console.OutputEncoding = System.Text.Encoding.UTF8;

    InitializeSimulation();
    RunSimulationLoop();
  }

  static void InitializeSimulation() {
    _asteroidEmitter = new AsteroidEmitter();
    _activeAsteroids = new List<Asteroid>();
    _motherShip = new MotherShip();
    _chronCounter = 0;

    for (int i = 0; i < InitialAsteroids; i++) {
      _activeAsteroids.Add(_asteroidEmitter.Spawn());
    }

    string header = "╔════════════════════════════════════════════════════════════════╗\n" +
                    "║                   СИМУЛЯТОР АСТЕРОИДОВ v2.0                    ║\n" +
                    "║                      С ХАРВЕСТЕРАМИ                             ║\n" +
                    "╚════════════════════════════════════════════════════════════════╝\n\n";

    Console.Write(header);

    DisplayAllInfo();

    string controls = "\n────────────────────────────────────────────────────────────────\n" +
                     "Команды:\n" +
                     "  ENTER - Следующий хрон\n" +
                     "  R     - Показать суммарную добычу\n" +
                     "  ESC   - Выход\n" +
                     "────────────────────────────────────────────────────────────────\n";

    Console.Write(controls);
  }

  static void RunSimulationLoop() {
    while (true) {
      Console.Write("Хрон #" + (_chronCounter + 1) + " готов. Нажмите ENTER/R/ESC: ");

      ConsoleKeyInfo keyInfo = Console.ReadKey(true);

      if (keyInfo.Key == ConsoleKey.Escape) {
        Console.WriteLine("\n\nДо свидания!");
        break;
      }

      if (keyInfo.Key == ConsoleKey.R) {
        ShowMiningTotals();
        continue;
      }

      if (keyInfo.Key == ConsoleKey.Enter) {
        ProcessChronTick();
      }
    }
  }

  static void ShowMiningTotals() {
    Console.Clear();

    string output = "╔════════════════════════════════════════════════════════════════╗\n" +
                   "║                    СУММАРНАЯ ДОБЫЧА                              ║\n" +
                   "╚════════════════════════════════════════════════════════════════╝\n";

    int grandTotal = 0;

    foreach (HarvesterShip harvester in _motherShip.fleet) {
      int total = _motherShip.GetTotalMinedForHarvester(harvester.name);
      grandTotal = grandTotal + total;
      output = output + "\n📊 " + harvester.name + ":\n";
      output = output + "   Добыто астероидов: " + harvester.asteroidsMined + "\n";
      output = output + "   Всего ресурсов: " + total + "\n";
    }

    output = output + "\n🏆 ОБЩАЯ ДОБЫЧА: " + grandTotal + " ресурсов\n";
    output = output + "\nНажмите любую клавишу для продолжения...";

    Console.Write(output);
    Console.ReadKey(true);
    Console.Clear();
    DisplayAllInfo();
  }

  static void ProcessChronTick() {
    _chronCounter = _chronCounter + 1;

    Console.Clear();

    string output = "╔════════════════════════════════════════════════════════════════╗\n" +
                   "║                        ХРОН #" + _chronCounter + "                                  ║\n" +
                   "╚════════════════════════════════════════════════════════════════╝\n\n";

    Console.Write(output);

    _motherShip.StabilizeZone();
    _motherShip.UpdateHarvesters(_activeAsteroids);

    if (_chronCounter % SpawnInterval == 0) {
      Random random = new Random();
      int newCount = random.Next(MinNewAsteroids, MaxNewAsteroids + 1);
      Console.WriteLine("✨ [СОБЫТИЕ] Появилось " + newCount + " новых астероидов!\n");

      for (int i = 0; i < newCount; i++) {
        _activeAsteroids.Add(_asteroidEmitter.Spawn());
      }
    }

    List<Asteroid> depleted = new List<Asteroid>();
    foreach (Asteroid asteroid in _activeAsteroids) {
      if (asteroid.state == AsteroidState.Depleted) {
        depleted.Add(asteroid);
      }
    }

    foreach (Asteroid asteroid in depleted) {
      _activeAsteroids.Remove(asteroid);
      _asteroidEmitter.Recycle(asteroid);
      Console.WriteLine("♻ [УТИЛИЗАЦИЯ] Астероид #" + asteroid.spawnId + " истощён и возвращён в пул");
    }

    if (depleted.Count > 0) {
      Console.WriteLine();
    }

    DisplayAllInfo();

    if (_chronCounter == WorklogChron) {
      _motherShip.PrintWorklog();
    }

    Console.WriteLine("\n════════════════════════════════════════════════════════════════");
  }

  static void DisplayAllInfo() {
    DisplayAsteroidsInfo();
    _motherShip.PrintHarvesterStatus();
  }

  static void DisplayAsteroidsInfo() {
    string output = "╔════════════════════════════════════════════════════════════════╗\n" +
                   "║                      АКТИВНЫЕ АСТЕРОИДЫ                         ║\n" +
                   "╠════════════════════════════════════════════════════════════════╣\n";

    if (_activeAsteroids.Count == 0) {
      output = output + "║                      Нет активных астероидов                    ║\n";
    } else {
      output = output + "║  N  │ SpawnID │ Ресурс     │ Статус                            ║\n";
      output = output + "╠═════╪═════════╪════════════╪═══════════════════════════════════╣\n";

      int maxDisplay = 20;
      for (int index = 0; index < _activeAsteroids.Count && index < maxDisplay; index++) {
        Asteroid asteroid = _activeAsteroids[index];
        string statusText = "";
        if (asteroid.state == AsteroidState.Idle) {
          statusText = "Ожидает";
        } else if (asteroid.state == AsteroidState.Mining) {
          statusText = "В добыче";
        } else {
          statusText = "Истощён";
        }

        string line = "║ " + (index + 1).ToString().PadLeft(3) + " │ " +
                     asteroid.spawnId.ToString().PadLeft(5) + " │ " +
                     asteroid.currentEchos.ToString().PadLeft(4) + "/" + asteroid.maxEchos.ToString().PadLeft(4) + " │ " +
                     statusText.PadRight(28) + " ║\n";
        output = output + line;
      }

      if (_activeAsteroids.Count > maxDisplay) {
        output = output + "║                                     и ещё " + (_activeAsteroids.Count - maxDisplay) + "... ║\n";
      }
    }

    output = output + "╚════════════════════════════════════════════════════════════════╝\n";
    output = output + "Всего астероидов: " + _activeAsteroids.Count + "\n";
    output = output + "Доступно в пуле: " + _asteroidEmitter.AvailableCount + "\n";
    output = output + "Всего создано: " + _asteroidEmitter.TotalCreated + "\n\n";

    Console.Write(output);
  }
}