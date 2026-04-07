using System;
using System.Collections.Generic;
using System.Linq;
using AsteroidZoneSimulation.Models;
using AsteroidZoneSimulation.Core;

namespace AsteroidZoneSimulation {
  class Program {

    public static List<Asteroid> activeAsteroids = new List<Asteroid>();
    public static AsteroidEmitter emitter;
    public static int chroneCounter = 0;
    public static Random random = new Random();

    static void Main() {
      Console.Title = "Проект Адам. Астероидная зона";

      int initialNumberOfAsteroids = 3;
        
      emitter = new AsteroidEmitter(5);
        
      for (int spawnIndex = 0; spawnIndex < initialNumberOfAsteroids; ++spawnIndex) {
        SpawnNewAsteroid();
      }
        
      Console.WriteLine($"\n[Старт] Обнаружено активных астероидов: {activeAsteroids.Count}");
      DisplayAsteroidsInfo();
        
      while (true) {
        Console.WriteLine("\n=======================================" +
                          $"\n    ░▒▓█ХРОН #{chroneCounter}█▓▒░" +
                          "\n=======================================" +
                          "\n\nНажмите ENTER для следующего хрона или ESC для выхода");
            
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
        if (keyInfo.Key == ConsoleKey.Escape) {
          Console.WriteLine("\nПрограмма завершает работу...Приятного дня, капитан!");
          break;
        } else if (keyInfo.Key == ConsoleKey.Enter) {
          ProcessChrone();
        } else {
          Console.WriteLine("Введена неверная клавиша! Используйте ENTER или ESC");
        }
      }
    }

    static void ProcessChrone() {
      ++chroneCounter;
        
      Console.Clear();
      Console.WriteLine("\n=======================================" +
                        $"\n    ░▒▓█ХРОН #{chroneCounter}█▓▒░" +
                        "\n=======================================");
        
      foreach (var asteroid in activeAsteroids) {
        asteroid.OnChroneTick();
      }
        
      if (chroneCounter % 5 == 0) {
        int newAsteroidsCount = random.Next(1, 4);
        Console.WriteLine($"\nОбнаружено {newAsteroidsCount} новых астероидов!\n");
            
        for (int spawnIndex = 0; spawnIndex < newAsteroidsCount; ++spawnIndex) {
          SpawnNewAsteroid();
        }
      }
        
      List<Asteroid> depletedAsteroids = activeAsteroids.Where(asteroid => asteroid.State == AsteroidState.Depleted).ToList();
        
      if (depletedAsteroids.Any()) {
        Console.WriteLine($"\nНайдено {depletedAsteroids.Count} истощённых астероидов");
        foreach (var asteroid in depletedAsteroids) {
          activeAsteroids.Remove(asteroid);
          emitter.Recycle(asteroid);
        }
      }
        
      Console.WriteLine($"\n.* АКТИВНЫЕ АСТЕРОИДЫ ({activeAsteroids.Count}) *.\n");
      DisplayAsteroidsInfo();
        
      Console.WriteLine($"\nСвободных астероидов в пуле: {emitter.GetAvailableCount()}" +
                        $"Всего спавнов: {activeAsteroids.Max(asteroid => asteroid.SpawnID)}");
    }
    
    static void SpawnNewAsteroid() {
      Asteroid asteroid = emitter.Spawn();
      activeAsteroids.Add(asteroid);
      Console.WriteLine($"[Спавн] Астероид {asteroid} появился!");
    }
    
    static void DisplayAsteroidsInfo() {
      if (activeAsteroids.Count == 0) {
        Console.WriteLine("Нет активных астероидов. Нашей жизни ничего не угрожает");
        return;
      }
        
      for (int activeAsteroidIndex = 0; activeAsteroidIndex < activeAsteroids.Count; ++activeAsteroidIndex) {
        Console.WriteLine($"{activeAsteroidIndex + 1}. {activeAsteroids[activeAsteroidIndex].GetAsteroidInformation()}");
      }
    }
  }
}
