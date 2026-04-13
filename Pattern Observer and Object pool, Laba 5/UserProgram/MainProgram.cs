using Pattern_Observer_and_Object_pool__Laba_5.PoolObject;
using System;
using System.Collections.Generic;

namespace Pattern_Observer_and_Object_pool__Laba_5 {

  public class MainProgram {

    static void Main() {
      List<Asteroid> activeAsteroid;
      AsteroidEmitter asteroid = new AsteroidEmitter(5);
      activeAsteroid = new List<Asteroid>();
      int chroneCount;

      chroneCount = 0;

      Asteroid asteroid1 = asteroid.Spawn();
      Asteroid asteroid2 = asteroid.Spawn();
      Asteroid asteroid3 = asteroid.Spawn();

      ChroneManager.AddListener(asteroid1);
      ChroneManager.AddListener(asteroid2);
      ChroneManager.AddListener(asteroid3);

      activeAsteroid.Add(asteroid1);
      activeAsteroid.Add(asteroid2);
      activeAsteroid.Add(asteroid3);

      Console.WriteLine("Press Enter to continue: ");

      while (true) {
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Enter) {
          Console.WriteLine($"Chron {++chroneCount}");
          Console.WriteLine($"asteroid1 Characteristics: Max Echo: {asteroid1.MaxEchos}, Current Echo: {asteroid1.CurrentEchos}, AsteroidState: {asteroid1.State}");
          Console.WriteLine($"asteroid2 Characteristics: Max Echo: {asteroid2.MaxEchos}, Current Echo: {asteroid2.CurrentEchos}, AsteroidState: {asteroid2.State}");
          Console.WriteLine($"asteroid3 Characteristics: Max Echo: {asteroid3.MaxEchos}, Current Echo: {asteroid3.CurrentEchos}, AsteroidState: {asteroid3.State} \n");

          ChroneManager.MakeChroneTick();

          if (asteroid1.CurrentEchos == 0) {
            asteroid1.Reset();
            asteroid.Recycle(asteroid1);
          }

          if (asteroid2.CurrentEchos == 0) {
            asteroid2.Reset();
            asteroid.Recycle(asteroid2);
          }

          if (asteroid3.CurrentEchos == 0) {
            asteroid3.Reset();
            asteroid.Recycle(asteroid3);
          }
        }
      }
    }
  }
}
