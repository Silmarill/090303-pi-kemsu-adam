using System;
using System.Collections.Generic;
using System.Linq;

internal class Program {
  static void Main(string[] args) {
    MotherShip motherShip = new MotherShip(5, 500, 50);
    int chroneCounter = 0;
    Random random = new Random();
    AsteroidEmitter asteroidEmitter = new AsteroidEmitter(5, motherShip, 5, 4, 5);
    motherShip.SetEmitter(asteroidEmitter);

    Asteroid asteroid1 = asteroidEmitter.Spawn();
    motherShip.AddAsteroid(asteroid1);
    ChroneManager.AddListener(asteroid1);
    Asteroid asteroid2 = asteroidEmitter.Spawn();
    motherShip.AddAsteroid(asteroid2);
    ChroneManager.AddListener(asteroid2);
    Asteroid asteroid3 = asteroidEmitter.Spawn();
    motherShip.AddAsteroid(asteroid3);
    ChroneManager.AddListener(asteroid3);

    while (true) {
      Console.WriteLine($"=== Chron {chroneCounter} ===");
      Console.WriteLine();
      motherShip.PrintAsteroidsInfo();
      Console.WriteLine();
      motherShip.PrintHarvestersInfo();
      Console.WriteLine();
      motherShip.PrintTotalMined();
      Console.WriteLine();

      if (chroneCounter > 0 && chroneCounter % 15 == 0) {
        motherShip.PrintFullWorklog();
      }

      ConsoleKeyInfo key = Console.ReadKey(true);

      if (key.Key == ConsoleKey.Escape) {
        break;
      } else if (key.Key == ConsoleKey.R) {
        motherShip.PrintTotalMined();
        Console.ReadKey(true);
      } else if (key.Key == ConsoleKey.Enter) {
        ChroneManager.MakeChroneTick();
        motherShip.AssignIdleHarvesters();
        ++chroneCounter;
      }
    }
  }
}
