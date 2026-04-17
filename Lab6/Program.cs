using System;

class Program {
  static void Main() {
    MotherShip motherShip = new MotherShip(5, 500, 50);
    AsteroidEmitter asteroidEmitter = new AsteroidEmitter(5, motherShip, 5, 4, 5);
    motherShip.SetEmitter(asteroidEmitter);

    for (int index = 0; index < 3; ++index) {
      Asteroid asteroid = asteroidEmitter.Spawn();
      motherShip.AddAsteroid(asteroid);
    }

    int chronCounter = 0;

    Console.Clear();
    PrintStatus(motherShip, chronCounter);

    while (true) {
      ConsoleKeyInfo key = Console.ReadKey();

      if (key.Key == ConsoleKey.Escape) {
        Console.WriteLine("\nSimulation stopped.");
        break;
      }

      if (key.Key == ConsoleKey.R) {
        Console.Clear();
        motherShip.PrintTotalMined();
        Console.WriteLine("\nEnter — next chron  |  R — summary  |  Esc — exit");
        continue;
      }

      if (key.Key != ConsoleKey.Enter) {
        continue;
      }

      ++chronCounter;
      ChroneManager.MakeChroneTick();
      motherShip.AssignIdleHarvesters();

      Console.Clear();
      PrintStatus(motherShip, chronCounter);

      if (chronCounter % 15 == 0) {
        motherShip.PrintFullWorklog();
      }
    }
  }

  static void PrintStatus(MotherShip motherShip, int chron) {
    Console.WriteLine("=== Chron: " + chron + " ===\n");
    motherShip.PrintAsteroidsInfo();
    Console.WriteLine();
    motherShip.PrintHarvestersInfo();
    Console.WriteLine();
    motherShip.PrintTotalMined();
    Console.WriteLine("\nEnter — next chron  |  R — summary  |  Esc — exit");
  }
}
