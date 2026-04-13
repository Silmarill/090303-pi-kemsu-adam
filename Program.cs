using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AsteroidZoneSimulation.Models;
using AsteroidZoneSimulation.Core;

namespace AsteroidZoneSimulation {
  class Program {

    private static MotherShip _motherShip;
    private static AsteroidEmitter _asteroidEmitter;
    public static int chroneCounter = 0;

    static void Main() {
      Console.Title = "Проект Адам. Астероидная зона";

      _motherShip = new MotherShip(5, 500, 50);
       
      //Пул 5, спавн каждые 5 хронов, от 1 до 3 астероидов
      _asteroidEmitter = new AsteroidEmitter(5, _motherShip, 5, 1, 3);
      _motherShip.SetEmitter(_asteroidEmitter);

      int initialNumberOfAsteroids = 3;
        
      for (int spawnIndex = 0; spawnIndex < initialNumberOfAsteroids; ++spawnIndex) {
        Asteroid asteroid = _asteroidEmitter.Spawn();
        _motherShip.AddAsteroid(asteroid);
      }

      _motherShip.AssignIdleHarvesters();
        
      while (true) {
        Console.WriteLine("\n=======================================" +
                          $"\n        ░▒▓█ХРОН #{chroneCounter}█▓▒░" +
                          "\n=======================================");

        _motherShip.PrintAsteroidsInfo();
        _motherShip.PrintHarvestersInfo();
        _motherShip.PrintTotalMined();
            
        Console.WriteLine($"\n[Пул] Свободных астероидов: {_asteroidEmitter.GetAvailableCount()}" +
                          "\n\n----------------------------------------------------------------" +
                          "\n  ENTER - следующий хрон | R - суммарная добыча | ESC - выход" +
                          "\n----------------------------------------------------------------");

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
        if (keyInfo.Key == ConsoleKey.Escape) {
          Console.WriteLine("\nПрограмма завершает работу...Приятного дня, капитан!");
          break;
        } else if (keyInfo.Key == ConsoleKey.Enter) {
          ProcessChrone();
        } else if (keyInfo.Key == ConsoleKey.R) {
          _motherShip.PrintTotalMined();
          Console.WriteLine("\nНажмите любую клавишу для продолжения...");
          Console.ReadKey(true);
        } else {
          Console.WriteLine("Введена неверная клавиша! Используйте ENTER или ESC");
        }
      }
    }

    static void ProcessChrone() {
      ++chroneCounter;
        
      Console.Clear();
      Console.WriteLine("\n=======================================" +
                        $"\n        ░▒▓█ХРОН #{chroneCounter}█▓▒░" +
                        "\n=======================================");
        
      ChroneManager.MakeChroneTick();

      _motherShip.AssignIdleHarvesters();
        
      if (chroneCounter % 15 == 0) {
        _motherShip.PrintFullWorklog();
      }

      Thread.Sleep(30);
    }
  }
}
