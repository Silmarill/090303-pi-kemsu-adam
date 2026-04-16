using System;
using System.Collections.Generic;
using System.Linq;

internal class Program {
  static void Main(string[] args) {
    AsteroidEmitter asteroidEmitter = new AsteroidEmitter(5);
    MotherShip motherShip = new MotherShip(5, 500, 50);
    motherShip.SetEmitter(asteroidEmitter);
    int chroneCounter = 0;
    Random random = new Random();

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
        motherShip.AssignIdleHarvesters();
        ChroneManager.MakeChroneTick();
        ++chroneCounter;

        if (chroneCounter > 0 && chroneCounter % 5 == 0) {
          int count = random.Next(4, 6);
          for (int spawnedCount = 0; spawnedCount < count; ++spawnedCount) {
            Asteroid newAsteroid = asteroidEmitter.Spawn();
            motherShip.AddAsteroid(newAsteroid);
            ChroneManager.AddListener(newAsteroid);
          }
        }
      }
    }
  }
 }
