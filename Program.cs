using AsteroidsLab.Asteroids;
using AsteroidsLab.Fleet;
using AsteroidsLab.Managers;

namespace AsteroidsLab;

internal static class Program
{
  private static void Main()
  {
    MotherShip motherShip;
    AsteroidEmitter asteroidEmitter;
    int chronCounter;
    bool exitRequested;
    ConsoleKeyInfo pressedKey;
    int worklogEveryNthChron;
    int initialIndex;
    Asteroid spawnedAsteroid;
    int harvesterFleetSize;
    int harvesterCargoCapacity;
    int harvesterBiteSize;
    int asteroidPoolInitialSize;
    int asteroidSpawnIntervalChrons;
    int asteroidSpawnCountMinInclusive;
    int asteroidSpawnCountMaxInclusive;
    int simulationStartAsteroidCount;

    worklogEveryNthChron = 15;

    harvesterFleetSize = 5;
    harvesterCargoCapacity = 500;
    harvesterBiteSize = 50;
    asteroidPoolInitialSize = 5;
    asteroidSpawnIntervalChrons = 5;
    asteroidSpawnCountMinInclusive = 4;
    asteroidSpawnCountMaxInclusive = 5;
    simulationStartAsteroidCount = 3;

    motherShip = new MotherShip(harvesterFleetSize, harvesterCargoCapacity, harvesterBiteSize);
    asteroidEmitter = new AsteroidEmitter(
      asteroidPoolInitialSize,
      motherShip,
      asteroidSpawnIntervalChrons,
      asteroidSpawnCountMinInclusive,
      asteroidSpawnCountMaxInclusive);
    motherShip.SetEmitter(asteroidEmitter);

    for (initialIndex = 0; initialIndex < simulationStartAsteroidCount; ++initialIndex)
    {
      spawnedAsteroid = asteroidEmitter.Spawn();
      motherShip.AddAsteroid(spawnedAsteroid);
    }

    chronCounter = 0;
    exitRequested = false;

    while (!exitRequested)
    {
      Console.Clear();
      Console.WriteLine("Current chrone: " + chronCounter);
      Console.WriteLine("Active asteroids: " + motherShip.GetActiveAsteroidsCount());
      Console.WriteLine("Pool available: " + asteroidEmitter.GetAvailableCount());
      Console.WriteLine();

      motherShip.PrintAsteroidsInfo();

      Console.WriteLine();
      Console.WriteLine("--- Harvesters ---");
      motherShip.PrintHarvestersInfo();

      Console.WriteLine();
      motherShip.PrintTotalMined();

      Console.WriteLine();
      Console.WriteLine("Enter = next chron | Esc = exit | R = totals");

      pressedKey = Console.ReadKey(intercept: true);
      if (pressedKey.Key == ConsoleKey.Escape)
      {
        exitRequested = true;
        continue;
      }

      if (pressedKey.Key == ConsoleKey.R)
      {
        Console.WriteLine();
        motherShip.PrintTotalMined();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(intercept: true);
        continue;
      }

      if (pressedKey.Key != ConsoleKey.Enter)
      {
        continue;
      }

      ++chronCounter;
      ChroneManager.MakeChroneTick();
      motherShip.AssignIdleHarvesters();

      if (chronCounter % worklogEveryNthChron == 0)
      {
        Console.Clear();
        motherShip.PrintFullWorklog();
        Console.WriteLine("Press any key to return to simulation...");
        Console.ReadKey(intercept: true);
      }
    }

    Console.WriteLine("Done.");
  }
}
