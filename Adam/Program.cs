using System;
using System.Collections.Generic;
using Core;
using Asteroids;

namespace AsteroidSimulation {
  class Program {
    private static int InitialPoolSize;
    private static int InitialActiveAsteroids;
    private static int EchosDecrement;
    private static int MinNewAsteroids;
    private static int MaxNewAsteroids;
    private static int ChronIntervalForSpawn;
    private static int MinEchosValue;
    private static int MaxEchosValue;

    static Program()
    {
      InitialPoolSize = 5;
      InitialActiveAsteroids = 3;
      EchosDecrement = 100;
      MinNewAsteroids = 1;
      MaxNewAsteroids = 3;
      ChronIntervalForSpawn = 5;
      MinEchosValue = 100;
      MaxEchosValue = 1000;
    }

    static void Main(string[] args)
    {
      AsteroidEmitter emitter;
      List<Asteroid> activeAsteroids;
      int chronCounter;
      bool isRunning;
      int asteroidCount;
      Asteroid asteroid;
      int newAsteroidsCount;
      int newAsteroidIndex;
      List<Asteroid> depletedAsteroids;
      Random random;
      ConsoleKeyInfo key;
      int depletedIndex;
      int maxNewAsteroidsInclusive;

      maxNewAsteroidsInclusive = 1;
      emitter = new AsteroidEmitter(InitialPoolSize);
      activeAsteroids = new List<Asteroid>();
      chronCounter = 0;
      isRunning = true;

      Console.Title = "Симуляция астероидов с Echos";
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine("=== СИМУЛЯЦИЯ АСТЕРОИДОВ И ХРОНОВ ===\n");
      Console.ResetColor();

      for (asteroidCount = 0; asteroidCount < InitialActiveAsteroids; ++asteroidCount)
      {
        asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChroneManager.AddListener(asteroid);
      }

      Console.WriteLine("Нажмите Enter для следующего хрона, Esc - выход\n");
      PrintAsteroidsInfo(activeAsteroids, chronCounter);

      while (isRunning)
      {
        key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape)
        {
          isRunning = false;
          break;
        }
        else if (key.Key == ConsoleKey.Enter)
        {
          ++chronCounter;
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine($"\n========== ХРОН #{chronCounter} ==========");
          Console.ResetColor();

          ChroneManager.MakeChroneTick();

          if (chronCounter % ChronIntervalForSpawn == 0)
          {
            random = new Random();
            newAsteroidsCount = random.Next(MinNewAsteroids, MaxNewAsteroids + maxNewAsteroidsInclusive);
            Console.WriteLine($"\n*** Генерация {newAsteroidsCount} новых астероидов! ***");

            for (newAsteroidIndex = 0; newAsteroidIndex < newAsteroidsCount; ++newAsteroidIndex)
            {
              asteroid = emitter.Spawn();
              activeAsteroids.Add(asteroid);
              ChroneManager.AddListener(asteroid);
            }
          }

          depletedAsteroids = new List<Asteroid>();
          foreach (var currentAsteroid in activeAsteroids)
          {
            if (currentAsteroid.state == AsteroidState.Depleted)
            {
              depletedAsteroids.Add(currentAsteroid);
            }
          }

          for (depletedIndex = 0; depletedIndex < depletedAsteroids.Count; ++depletedIndex)
          {
            Asteroid depleted;

            depleted = depletedAsteroids[depletedIndex];
            activeAsteroids.Remove(depleted);
            ChroneManager.RemoveListener(depleted);
            emitter.Recycle(depleted);
            Console.WriteLine($"  [Удалён] Астероид CreateId:{depleted.createId} достиг 0 и утилизирован");
          }

          Console.Clear();
          PrintAsteroidsInfo(activeAsteroids, chronCounter);

          if (activeAsteroids.Count == 0)
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n!!! ВНИМАНИЕ: Нет активных астероидов !!!");
            Console.ResetColor();
          }
        }
      }

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("\n=== СИМУЛЯЦИЯ ЗАВЕРШЕНА ===");
      Console.ResetColor();
    }

    static void PrintAsteroidsInfo(List<Asteroid> asteroids, int chronCounter)
    {
      int asteroidPosition;
      int displayNumber;

      displayNumber = 1;
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine($"=== ТЕКУЩИЙ ХРОН: {chronCounter} ===");
      Console.WriteLine($"Активных астероидов: {asteroids.Count}");
      Console.ResetColor();

      if (asteroids.Count == 0)
      {
        Console.WriteLine("(Нет активных астероидов)");
      }
      else
      {
        for (asteroidPosition = 0; asteroidPosition < asteroids.Count; ++asteroidPosition)
        {
          Console.Write($"[{asteroidPosition + displayNumber}] ");
          asteroids[asteroidPosition].PrintInfo();
        }
      }

      Console.WriteLine("\n----------------------------------------");
      Console.WriteLine("Нажмите Enter → следующий хрон | Esc → выход");
    }
  }
}