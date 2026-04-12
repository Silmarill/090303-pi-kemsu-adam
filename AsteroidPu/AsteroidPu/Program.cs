using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidPu {
  internal class Program {

    public static void PrintInfo(List<Asteroid> list) {
      Console.Clear();
      Console.WriteLine("Info about all asteroids:");
      for (int indexI = 0; indexI < list.Count; ++indexI) {
        Console.Write($"Asteroid {indexI}, CurrentEchos: {list[indexI].CurrentEchos}\n" +
                      $"Asteroid Create ID: {list[indexI].CreateID}\n" +
                      $"Asteroid Spawn ID: {list[indexI].SpawnID}");

      }
      Console.WriteLine("\nEnter esc for exit or enter to contine");
    }

    static void Main(string[] args) {

      int countChrons = 0,
        countAsteroidItems = 5,
        countSpawnID = 0;
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

          if (countChrons % countAsteroidItems == 0) {
            int createdAsteroid = random.Next(1, 3);
            for (int indexI = 0; indexI < createdAsteroid; ++indexI) {
              Asteroid newAsteroid = asteroidItems.Spawn();
              //Unique serial number
              ++countSpawnID;
              newAsteroid.SpawnID = countSpawnID;
              activeAsteroid.Add(newAsteroid);
            }
          }

          for (int indexI = activeAsteroid.Count - 1; indexI >= 0; --indexI) {
            if (activeAsteroid[indexI].State == AsteroidState.Depleted) {
              asteroidItems.Recycle(activeAsteroid[indexI]);
              activeAsteroid.RemoveAt(indexI);
            }
          }

          PrintInfo(activeAsteroid);
        }
      }
    }
  }
}
