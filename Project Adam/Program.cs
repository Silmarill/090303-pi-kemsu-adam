using System;
using System.Collections.Generic;
using AsteroidSimulator.Managers;
using AsteroidSimulator.Models;

namespace AsteroidSimulator {
  internal static class Program {
    private static void Main(string[] args) {
      ConsoleKeyInfo menuKey;

      while (true) {
        Console.Clear();
        Console.WriteLine("Asteroid simulator — выберите лабораторную:");
        Console.WriteLine("  1 — Этап 5: базовая симуляция");
        Console.WriteLine("  2 — Этап 6 «Матриарх»");
        Console.WriteLine("  Esc — выход из программы");
        menuKey = Console.ReadKey(true);

        if (menuKey.Key == ConsoleKey.Escape) {
          return;
        }

        if (menuKey.Key == ConsoleKey.D1 || menuKey.Key == ConsoleKey.NumPad1) {
          RunLabBasicSimulation();
        } else if (menuKey.Key == ConsoleKey.D2 || menuKey.Key == ConsoleKey.NumPad2) {
          RunLabMatriarchSimulation();
        }
      }
    }

    private static void RunLabBasicSimulation() {
      List<Asteroid> activeAsteroids;
      AsteroidEmitter pool;
      Random randomGenerator;

      ChronManager.ClearListeners();
      Asteroid.ResetCounters();
      SimulationClock.CurrentChron = 0;

      activeAsteroids = new List<Asteroid>();
      randomGenerator = new Random();
      pool = new AsteroidEmitter(SimulationConstants.PoolInitialSize, activeAsteroids, randomGenerator, subscribeSpawnedAsteroidsToChron: true);

      BootstrapInitialAsteroids(activeAsteroids, pool, registerAsteroidsForChronTicks: true);
      pool.RegisterWithChronManager();

      RunChronLoop(activeAsteroids, pool, null);
    }

    private static void RunLabMatriarchSimulation() {
      List<Asteroid> activeAsteroids;
      AsteroidEmitter pool;
      Random randomGenerator;
      MotherShip motherShip;

      ChronManager.ClearListeners();
      Asteroid.ResetCounters();
      SimulationClock.CurrentChron = 0;

      activeAsteroids = new List<Asteroid>();
      randomGenerator = new Random();
      pool = new AsteroidEmitter(SimulationConstants.PoolInitialSize, activeAsteroids, randomGenerator, subscribeSpawnedAsteroidsToChron: false);

      BootstrapInitialAsteroids(activeAsteroids, pool, registerAsteroidsForChronTicks: false);

      motherShip = new MotherShip(5, activeAsteroids, randomGenerator);
      motherShip.RegisterWithChronManager();
      pool.RegisterWithChronManager();

      RunChronLoop(activeAsteroids, pool, motherShip);
    }

    private static void BootstrapInitialAsteroids(List<Asteroid> activeAsteroids, AsteroidEmitter pool, bool registerAsteroidsForChronTicks) {
      int index;

      for (index = 0; index < SimulationConstants.InitialActiveAsteroids; ++index) {
        Asteroid spawned;
        spawned = pool.Spawn();
        activeAsteroids.Add(spawned);
        if (registerAsteroidsForChronTicks) {
          ChronManager.AddListener(spawned);
        }
      }
    }

    private static void RunChronLoop(List<Asteroid> activeAsteroids, AsteroidEmitter pool, MotherShip motherShip) {
      int completedChrons;
      bool advanceChron;

      completedChrons = 0;

      while (true) {
        advanceChron = false;
        Console.Clear();
        Console.WriteLine("===== Завершено хронов: " + completedChrons + " =====\n");

        Console.WriteLine("Активные астероиды: " + activeAsteroids.Count);
        PrintAsteroidList(activeAsteroids);

        if (motherShip != null) {
          Console.WriteLine("\nФлот харвестеров (пояс стабилизирован — астероиды не «сыпятся» сами):");
          PrintHarvesterFleet(motherShip);
          Console.WriteLine("\nСуммарная добыча по журналу (каждый хрон):");
          PrintCumulativeMined(motherShip);
        }

        Console.WriteLine("\nEnter — следующий хрон | R — суммарная добыча по харвестерам | Esc — в меню");

        ConsoleKeyInfo pressedKey;
        pressedKey = Console.ReadKey(true);

        if (pressedKey.Key == ConsoleKey.Escape) {
          ChronManager.ClearListeners();
          return;
        }

        if (pressedKey.Key == ConsoleKey.R) {
          if (motherShip != null) {
            motherShip.PrintTotalMinedByHarvester();
          } else {
            Console.WriteLine("(суммарная добыча — только в режиме «Матриарх»)");
          }

          Console.WriteLine("\nНажмите любую клавишу…");
          Console.ReadKey(true);
          continue;
        }

        if (pressedKey.Key == ConsoleKey.Enter) {
          advanceChron = true;
        }

        if (!advanceChron) {
          continue;
        }

        completedChrons = completedChrons + 1;
        SimulationClock.CurrentChron = completedChrons;
        ChronManager.MakeChronTick();
        CleanupDepletedAsteroids(activeAsteroids, pool, motherShip);

        if (motherShip != null && completedChrons % SimulationConstants.WorklogPrintIntervalChrons == 0) {
          Console.Clear();
          Console.WriteLine("===== Хрон " + completedChrons + " — полный Worklog (по методичке) =====\n");
          motherShip.PrintFullWorklog();
          Console.WriteLine("\nНажмите любую клавишу…");
          Console.ReadKey(true);
        }
      }
    }

    private static void PrintCumulativeMined(MotherShip motherShip) {
      Dictionary<string, int> snapshot;
      snapshot = motherShip.GetLifetimeMinedSnapshot();

      foreach (KeyValuePair<string, int> entry in snapshot) {
        Console.WriteLine("  " + entry.Key + ": " + entry.Value);
      }
    }

    private static void PrintAsteroidList(List<Asteroid> activeAsteroids) {
      int index;

      for (index = 0; index < activeAsteroids.Count; ++index) {
        Console.WriteLine("  " + activeAsteroids[index].ToString());
      }
    }

    private static void PrintHarvesterFleet(MotherShip motherShip) {
      int index;

      for (index = 0; index < motherShip.Fleet.Count; ++index) {
        Console.WriteLine("  " + motherShip.Fleet[index].ToString());
      }
    }

    private static void CleanupDepletedAsteroids(List<Asteroid> activeAsteroids, AsteroidEmitter pool, MotherShip motherShip) {
      List<Asteroid> depletedBuffer;
      int index;

      depletedBuffer = new List<Asteroid>();

      for (index = 0; index < activeAsteroids.Count; ++index) {
        if (activeAsteroids[index].State == AsteroidState.Depleted) {
          depletedBuffer.Add(activeAsteroids[index]);
        }
      }

      for (index = 0; index < depletedBuffer.Count; ++index) {
        Asteroid deadAsteroid;
        deadAsteroid = depletedBuffer[index];

        if (motherShip != null) {
          motherShip.NotifyAsteroidLeavingBelt(deadAsteroid);
        }

        ChronManager.RemoveListener(deadAsteroid);
        activeAsteroids.Remove(deadAsteroid);
        pool.Recycle(deadAsteroid);
      }
    }
  }
}
