using AsteroidSimulation.Managers;
using AsteroidSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation
{
  class Program {
    private static AsteroidEmitter _asteroidEmitter;
    private static List<Asteroid> _activeAsteroids;
    private static int _chronCounter;
    private static readonly Random _random = new Random();

    private static int InitialPoolSize = 5;                    
    private static int MinAsteroidsToSpawn = 1;       
    private static int MaxAsteroidsToSpawn = 3;
    private static int SpawnInterval = 5;

    static void Main(string[] args) {
      Console.Title = "Asteroid Simulation - Observer & Object Pool";
      Console.CursorVisible = false;

      // Инициализация
      _asteroidEmitter = new AsteroidEmitter(InitialPoolSize);
      _activeAsteroids = new List<Asteroid>();
      _chronCounter = 0;

      // Спавн первых трёх астероидов
      Console.WriteLine("\n=== ИНИЦИАЛИЗАЦИЯ ===");
      int asteroidsToSpawnAtStart = 3;

      for (int spawnIndex = 0; spawnIndex < asteroidsToSpawnAtStart; ++spawnIndex) {
        SpawnNewAsteroid();
      }

      Console.WriteLine("\n=== НАЧАЛО СИМУЛЯЦИИ ===");
      Console.WriteLine("Нажмите Enter для следующего хрона, Esc для выхода\n");

      while (true) {
        PrintAsteroidsInfo();
        PrintChronInfo();

        Console.Write("> Ожидание действия... ");
        ConsoleKeyInfo key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape) {
          Console.WriteLine("\n\nЗавершение программы...");
          break;
        } else if (key.Key == ConsoleKey.Enter) {
          ProcessChronTick();
        } else {
          Console.WriteLine("\nНеверная клавиша. Используйте Enter или Esc.");
        }
      }

      Console.WriteLine("Программа завершена.");
    }

    private static void ProcessChronTick() {
      ++_chronCounter;
      Console.Clear();
      Console.WriteLine($"\n========== ХРОН #{_chronCounter} ==========");

      // 1. Уведомляем всех слушателей (активные астероиды уже подписаны)
      ChroneManager.MakeChroneTick();

      // 2. Каждые 5 хронов спавним 1-3 новых астероида
      const int chronIntervalForSpawning = 5;

      if (_chronCounter % chronIntervalForSpawning == 0) {
        int newAsteroidsCount = _random.Next(MinAsteroidsToSpawn, MaxAsteroidsToSpawn);
        Console.WriteLine($"\n[Событие] Каждые 5 хронов: спавн {newAsteroidsCount} новых астероидов");
        for (int asteroidIndex = 0; asteroidIndex < newAsteroidsCount; ++asteroidIndex) {
          SpawnNewAsteroid();
        }
      }

      // 3. Находим истощённые астероиды и возвращаем их в пул
      List<Asteroid> depletedAsteroids = _activeAsteroids.Where(a => a.State == AsteroidState.Depleted).ToList();

      foreach (var asteroid in depletedAsteroids) {
        Console.WriteLine($"\n[Деградация] Астероид CreateID:{asteroid.CreateID} истощён!");

        // Отписываем от хронов перед возвратом в пул
        ChroneManager.RemoveListener(asteroid);

        // Возвращаем в пул
        _asteroidEmitter.Recycle(asteroid);

        // Удаляем из активного списка
        _activeAsteroids.Remove(asteroid);
      }

      if (depletedAsteroids.Count > 0) {
        Console.WriteLine($"\n[Пул] Возвращено астероидов: {depletedAsteroids.Count}");
        Console.WriteLine($"[Пул] Доступно в пуле: {_asteroidEmitter.AvailableCount}");
      }

      Console.WriteLine($"\nАктивных астероидов: {_activeAsteroids.Count}");
    }

    private static void SpawnNewAsteroid() {
      Asteroid newAsteroid = _asteroidEmitter.Spawn();
      _activeAsteroids.Add(newAsteroid);
      ChroneManager.AddListener(newAsteroid);
      Console.WriteLine($"[Спавн] Новый астероид: {newAsteroid}");
    }

    private static void PrintAsteroidsInfo() {
      Console.WriteLine("\n--- АКТИВНЫЕ АСТЕРОИДЫ ---");
      if (_activeAsteroids.Count == 0) {
        Console.WriteLine("(нет активных астероидов)");
      } else {
        for (int asteroidIndex = 0; asteroidIndex < _activeAsteroids.Count; ++asteroidIndex) {
          var asteroid = _activeAsteroids[asteroidIndex];
          string status = asteroid.State == AsteroidState.Depleted ? "ИСТОЩЁН" : "АКТИВЕН";
          Console.WriteLine($"{asteroidIndex + 1}. CreateID:{asteroid.CreateID,-4} | SpawnID:{asteroid.SpawnID,-4} | " +
                            $"Echos: {asteroid.CurrentEchos,-4}/{asteroid.MaxEchos,-4} | {status}");
        }
      }
      Console.WriteLine("-----------------------------");
    }

    private static void PrintChronInfo() {
      Console.WriteLine($"\n--- СТАТИСТИКА ---");
      Console.WriteLine($"Текущий хрон: {_chronCounter}");
      Console.WriteLine($"Активных астероидов: {_activeAsteroids.Count}");
      Console.WriteLine($"Доступно в пуле: {_asteroidEmitter.AvailableCount}");
      Console.WriteLine($"Следующий спавн через: {5 - (_chronCounter % SpawnInterval)} хронов");
      Console.WriteLine("-------------------\n");
    }
  }
}
