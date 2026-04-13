using System;
using System.Collections.Generic;

namespace AsteroidPu {
  internal class Program {

    static void Main(string[] args) {

      int countChrons = 0,
        countAsteroidItems = 5,
        countSpawnID = 0,
        minCorrectNum = 0;
      AsteroidEmitter asteroidItems = new AsteroidEmitter(countAsteroidItems);
      List<Asteroid> activeAsteroid = new List<Asteroid>();
      bool isRun = true;
      Random random = new Random();

      Console.WriteLine("Enter esc for exit or enter to contine");

      while (isRun) {
        ConsoleKeyInfo keyInfo = Console.ReadKey();

        if (keyInfo.Key == ConsoleKey.Escape) {
          isRun = false;
        } else if(keyInfo.Key == ConsoleKey.Enter) {
            ++countChrons;

          foreach(Asteroid asteroid in activeAsteroid) {
            asteroid.OnChoneTick();
          }

          if (countChrons % countAsteroidItems == minCorrectNum) {
            int createdAsteroid = random.Next(1, 3);
            for (int indexI = 0; indexI < createdAsteroid; ++indexI) {
              Asteroid newAsteroid = asteroidItems.Spawn();
              //Unique serial number
              ++countSpawnID;
              newAsteroid.SpawnID = countSpawnID;
              activeAsteroid.Add(newAsteroid);
            }
          }

          for (int indexI = activeAsteroid.Count - 1; indexI >= minCorrectNum; --indexI) {
            if (activeAsteroid[indexI].State == AsteroidState.Depleted) {
              asteroidItems.Recycle(activeAsteroid[indexI]);
              activeAsteroid.RemoveAt(indexI);
            }
          }

          PrintInfo
        }
      }
    }
  }
}
