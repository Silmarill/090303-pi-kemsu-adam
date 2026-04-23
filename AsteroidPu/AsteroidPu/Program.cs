using AsteroidPu.Chrones;
using AsteroidPu.Ship;
using System;
using System.Collections.Generic;

namespace AsteroidPu {
  internal class Program {

    static void Main(string[] args) {

      int countChrons = 0,
        countAsteroidItems = 5,
        numForWorklog = 15,
        countHarvesterItems = 5,
        harvesterCargoCurrent = 500,
        harvesterBite = 50,
        countSpawnID = 0,
        minCorrectNum = 0;
      MotherShip motherShip = new MotherShip(countHarvesterItems, harvesterCargoCurrent, harvesterBite);
      AsteroidEmitter asteroidItems = new AsteroidEmitter(countAsteroidItems);
      List<Asteroid> activeAsteroid = new List<Asteroid>();
      bool isRun = true;
      Random random = new Random();

      Console.WriteLine("Enter esc for exit, enter to continue and R for output total production");

      while (isRun) {
        for (int indexI = 0; indexI < activeAsteroid.Count; ++indexI) {
          motherShip.PrintAsteroidItemsInfo();
          motherShip.PrintHarvesterItemsInfo();
          motherShip.PrintTotalMined();
        }
        ConsoleKeyInfo keyInfo = Console.ReadKey();

        if (keyInfo.Key == ConsoleKey.Escape) {
          isRun = false;
        } else if(keyInfo.Key == ConsoleKey.Enter) {
            ++countChrons;
          ChronoManager.MakeChronTick();
          motherShip.AssignIdleHarvesters();
          foreach(Asteroid asteroid in activeAsteroid) {
            asteroid.OnChoneTick();
          }

          if (countChrons % countAsteroidItems == minCorrectNum) {
            int createdAsteroid = random.Next(4, 5);
            for (int indexI = 0; indexI < createdAsteroid; ++indexI) {
              Asteroid newAsteroid = asteroidItems.Spawn();
              motherShip.AddAsteroid(newAsteroid);
              //Unique serial number
              ++countSpawnID;
              newAsteroid.SpawnID = countSpawnID;
              activeAsteroid.Add(newAsteroid);
            }
          }

          if (countChrons % numForWorklog == minCorrectNum) {
            motherShip.PrintFullWorklog();
          }

          for (int indexI = activeAsteroid.Count - 1; indexI >= minCorrectNum; --indexI) {
            if (activeAsteroid[indexI].State == AsteroidState.Depleted) {
              asteroidItems.Recycle(activeAsteroid[indexI]);
              activeAsteroid.RemoveAt(indexI);
            }
          }
        } else if (keyInfo.Key == ConsoleKey.R) {
          motherShip.PrintTotalMined();
        }
      }
    }
  }
}
