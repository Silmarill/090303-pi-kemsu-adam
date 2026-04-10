using System;
using System.Collections.Generic;
using Core;
using Asteroids;
using Harvester;
using MotherShip;

namespace AsteroidSimulation {
  class Program {
    public static int InitialPoolSize;
    public static int InitialActiveAsteroids;
    public static int ChronIntervalForSpawn;
    public static int FullWorklogChron;

    static Program()
    {
      InitialPoolSize = 10;
      InitialActiveAsteroids = 5;
      ChronIntervalForSpawn = 5;
      FullWorklogChron = 15;
    }

    static void Main(string[] args)
    {
      AsteroidEmitter emitter;
      List<Asteroid> activeAsteroids;
      MotherShip.MotherShip motherShip;
      int chronCounter;
      bool isRunning;
      int asteroidCount;
      Asteroid asteroid;
      int maxNewAsteroidsInclusive;
      int minNewAsteroids;
      int maxNewAsteroids;

      minNewAsteroids = 1;
      maxNewAsteroids = 3;
      emitter = new AsteroidEmitter(InitialPoolSize);
      activeAsteroids = new List<Asteroid>();
      motherShip = new MotherShip.MotherShip();
      chronCounter = 0;
      isRunning = true;
      maxNewAsteroidsInclusive = maxNewAsteroids + minNewAsteroids;

      Console.Title = "Симуляция - Станция Матриарх";
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine("=== СИМУЛЯЦИЯ СТАНЦИИ МАТРИАРХ И ФЛОТА ХАРВЕСТЕРОВ ===\n");
      Console.ResetColor();

      for (asteroidCount = 0; asteroidCount < InitialActiveAsteroids; ++asteroidCount)
      {
        asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
      }

      Console.WriteLine("Нажмите Enter для следующего хрона, R - суммарная добыча, Esc - выход\n");
      motherShip.PrintAllInfo(activeAsteroids, chronCounter);

      while (isRunning)
      {
        ConsoleKeyInfo key;

        key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape)
        {
          isRunning = false;
          break;
        }
        else if (key.Key == ConsoleKey.Enter)
        {
          chronCounter = chronCounter + 1;
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine($"\n========== ХРОН #{chronCounter} ==========");
          Console.ResetColor();

          motherShip.UpdateHarvesters(activeAsteroids);

          activeAsteroids = motherShip.RemoveDepletedAsteroids(activeAsteroids, emitter);

          motherShip.SpawnNewAsteroids(chronCounter, emitter, activeAsteroids);

          Console.Clear();
          motherShip.PrintAllInfo(activeAsteroids, chronCounter);

          if (chronCounter == FullWorklogChron)
          {
            motherShip.PrintFullWorklog();
          }

          if (activeAsteroids.Count == 0)
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n!!! ВНИМАНИЕ: Нет активных астероидов !!!");
            Console.ResetColor();
          }
        }
        else if (key.Key == ConsoleKey.R)
        {
          Console.Clear();
          motherShip.PrintAllInfo(activeAsteroids, chronCounter);
          Console.WriteLine("\nНажмите любую клавишу для продолжения...");
          Console.ReadKey(true);
          Console.Clear();
          motherShip.PrintAllInfo(activeAsteroids, chronCounter);
        }
      }

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("\n=== СИМУЛЯЦИЯ ЗАВЕРШЕНА ===");
      Console.ResetColor();
    }
  }
}