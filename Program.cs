using AsteroidsLab.Managers;
using AsteroidsLab.Asteroids;

namespace AsteroidsLab;

internal static class Program
{
  private static void Main()
  {
    AsteroidEmitter asteroidEmitter;
    List<Asteroid> activeAsteroids;
    Random random;
    int chronCounter;
    bool exitRequested;
    int initialIndex;
    Asteroid spawnedAsteroid;
    int asteroidIndex;
    ConsoleKeyInfo pressedKey;
    int spawnBatchCount;
    int spawnIndex;
    Asteroid newAsteroid;
    Asteroid asteroid;
    int initialPoolSize;
    int initialActiveAsteroidCount;
    int spawnEveryNthChron;
    int extraSpawnCountMinInclusive;
    int extraSpawnCountMaxExclusive;
    int reverseCleanupStartIndex;
    int activeAsteroidCountForCleanup;
    int lastIndexOffsetFromCount;

    initialPoolSize = 5;
    initialActiveAsteroidCount = 3;
    spawnEveryNthChron = 5;
    extraSpawnCountMinInclusive = 1;
    extraSpawnCountMaxExclusive = 4;

    asteroidEmitter = new AsteroidEmitter(initialPoolSize);
    activeAsteroids = new List<Asteroid>();
    random = new Random();
    chronCounter = 0;

    for (initialIndex = 0; initialIndex < initialActiveAsteroidCount; ++initialIndex)
    {
      spawnedAsteroid = asteroidEmitter.Spawn();
      activeAsteroids.Add(spawnedAsteroid);
      ChroneManager.AddListener(spawnedAsteroid);
    }

    exitRequested = false;
    while (!exitRequested)
    {
      Console.Clear();
      Console.WriteLine("Current chrone: " + chronCounter);
      Console.WriteLine("Active asteroids: " + activeAsteroids.Count);
      Console.WriteLine();

      for (asteroidIndex = 0; asteroidIndex < activeAsteroids.Count; ++asteroidIndex)
      {
        activeAsteroids[asteroidIndex].PrintInfo();
      }

      Console.WriteLine();
      Console.WriteLine("Enter = next chrone, Esc = exit.");

      pressedKey = Console.ReadKey(intercept: true);
      if (pressedKey.Key == ConsoleKey.Escape)
      {
        exitRequested = true;
        continue;
      }

      if (pressedKey.Key != ConsoleKey.Enter)
      {
        continue;
      }

      ++chronCounter;
      ChroneManager.MakeChroneTick();

      if (chronCounter % spawnEveryNthChron == 0)
      {
        spawnBatchCount = random.Next(extraSpawnCountMinInclusive, extraSpawnCountMaxExclusive);
        for (spawnIndex = 0; spawnIndex < spawnBatchCount; ++spawnIndex)
        {
          newAsteroid = asteroidEmitter.Spawn();
          activeAsteroids.Add(newAsteroid);
          ChroneManager.AddListener(newAsteroid);
        }
      }

      lastIndexOffsetFromCount = 1;
      activeAsteroidCountForCleanup = activeAsteroids.Count;
      reverseCleanupStartIndex = activeAsteroidCountForCleanup - lastIndexOffsetFromCount;

      for (asteroidIndex = reverseCleanupStartIndex; asteroidIndex >= 0; --asteroidIndex)
      {
        asteroid = activeAsteroids[asteroidIndex];
        if (asteroid.State == AsteroidState.Depleted)
        {
          ChroneManager.RemoveListener(asteroid);
          asteroidEmitter.Recycle(asteroid);
          activeAsteroids.RemoveAt(asteroidIndex);
        }
      }
    }

    Console.WriteLine("Done.");
  }
}
