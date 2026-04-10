using Pattern_Observer_and_Object_pool__Laba_5.PoolObject;
using System;
using System.Collections.Generic;

namespace Pattern_Observer_and_Object_pool__Laba_5 {

  public class MainProgram {

    static void Main() {
      AsteroidEmitter emitter = new AsteroidEmitter(5);

      List<Asteroid> activeAsteroids = new List<Asteroid>();

      Asteroid firstAsteroid = emitter.Spawn();
      Asteroid secondAsteroid = emitter.Spawn();
      Asteroid thirdAsteroid = emitter.Spawn();

      activeAsteroids.Add(firstAsteroid);
      activeAsteroids.Add(secondAsteroid);
      activeAsteroids.Add(thirdAsteroid);

      ChroneManager.AddListener(firstAsteroid);
      ChroneManager.AddListener(secondAsteroid);
      ChroneManager.AddListener(thirdAsteroid);

      int tickCounter = 0;

      Random random = new Random();

      while (true) {
        Console.Clear();
        Console.WriteLine($"CHRONE #{tickCounter}");
        Console.WriteLine($"Active asteroid: {activeAsteroids.Count}");
        Console.WriteLine();

        for (int counter = 0; counter < activeAsteroids.Count; counter++) {
          var asteroid = activeAsteroids[counter];
          Console.WriteLine($"Asteroid {counter + 1}: resource {asteroid.CurrentEchos}/{asteroid.MaxEchos}, status: {asteroid.State}");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter for the next chrono");
        Console.WriteLine("Press Esc to exit");

        ConsoleKeyInfo key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape) {
          break;
        }

        if (key.Key == ConsoleKey.Enter) {
          tickCounter++;

          ChroneManager.MakeChroneTick();

          if (tickCounter % 5 == 0) {
            int newAsteroidsCount = random.Next(1, 4);
            Console.WriteLine($"\n5th chron! Spawn {newAsteroidsCount} new asteroids\r\n");

            for (int count = 0; count < newAsteroidsCount; ++count) {
              Asteroid newAsteroid = emitter.Spawn();
              activeAsteroids.Add(newAsteroid);
              ChroneManager.AddListener(newAsteroid);
              Console.WriteLine($" A new asteroid has been created! Resource\r\n: {newAsteroid.CurrentEchos}/{newAsteroid.MaxEchos}");
            }
          }

          for (int count = activeAsteroids.Count - 1; count >= 0; --count) {
            Asteroid asteroid = activeAsteroids[count];

            if (asteroid.State == Asteroid.AsteroidState.Depleted) {
              Console.WriteLine($"  The asteroid is depleted! Return to the pool.");
              activeAsteroids.RemoveAt(count);
              emitter.Recycle(asteroid);
            }
          }

          Console.WriteLine("\nPress any button to continue...");
          Console.ReadKey(true);
        }
      }
      Console.WriteLine("Program over");
    }
  }
}
