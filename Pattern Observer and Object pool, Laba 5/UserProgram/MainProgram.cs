using Pattern_Observer_and_Object_pool__Laba_5.HarvesterShipRealisation;
using Pattern_Observer_and_Object_pool__Laba_5.PoolObject;
using System;
using System.Collections.Generic;

namespace Pattern_Observer_and_Object_pool__Laba_5 {

  public class MainProgram {
    static Random rnd = new Random();

    static void Main() {
      List<Asteroid> activeAsteroid;
      AsteroidEmitter asteroid = new AsteroidEmitter(5);
      activeAsteroid = new List<Asteroid>();
      HarvesterShip harvester = new HarvesterShip("First", 500, 50);
      MotherShip motherShip = new MotherShip();
      int chroneCount;
      int asteroidRandom;

      asteroidRandom = rnd.Next(1,4);
      chroneCount = 0;

      HarvesterShip harvester1 = asteroid.SpawnHarvester();
      HarvesterShip harvester2 = asteroid.SpawnHarvester();
      HarvesterShip harvester3 = asteroid.SpawnHarvester();
      HarvesterShip harvester4 = asteroid.SpawnHarvester();
      HarvesterShip harvester5 = asteroid.SpawnHarvester();

      ChroneManager.AddListener(harvester1);
      ChroneManager.AddListener(harvester2);
      ChroneManager.AddListener(harvester3);
      ChroneManager.AddListener(harvester4);
      ChroneManager.AddListener(harvester5);

      motherShip.Fleet.Add(harvester1);
      motherShip.Fleet.Add(harvester2);
      motherShip.Fleet.Add(harvester3);
      motherShip.Fleet.Add(harvester4);
      motherShip.Fleet.Add(harvester5);


      Asteroid asteroid1 = asteroid.Spawn();
      Asteroid asteroid2 = asteroid.Spawn();
      Asteroid asteroid3 = asteroid.Spawn();

      ChroneManager.AddListener(asteroid1);
      ChroneManager.AddListener(asteroid2);
      ChroneManager.AddListener(asteroid3);

      activeAsteroid.Add(asteroid1);
      activeAsteroid.Add(asteroid2);
      activeAsteroid.Add(asteroid3);

      Console.Write("Press Enter to continue: ");

      while (true) {
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Enter) {
          Console.WriteLine($"Chron {++chroneCount}");
          ChroneManager.MakeChroneTick();

          for (int count = 0; count < activeAsteroid.Count; ++count) {
            Console.WriteLine($"{activeAsteroid[count].CreateID} asteroid Characteristics: Max Echo: {activeAsteroid[count].MaxEchos}, Current Echo: {activeAsteroid[count].CurrentEchos}, AsteroidState: {activeAsteroid[count].State}");
            Console.WriteLine($"Name: {harvester.name}, CargoCapacity: {harvester.cargoCapacity}, BiteSize: {harvester.biteSize}, State: {harvester.state}\n");
          }

          harvester.StartMining(asteroid1);
          harvester.StartMining(asteroid2);
          harvester.StartMining(asteroid3);

          for (int count = activeAsteroid.Count - 1; count != 0; --count)
            if (activeAsteroid[count].CurrentEchos == 0) {
              ChroneManager.RemoveListener(activeAsteroid[count]);
              asteroid.Recycle(activeAsteroid[count]);
              activeAsteroid.Remove(activeAsteroid[count]);
            }
            
          if (chroneCount % 5 == 0) {
              for (int count = 0; count < asteroidRandom; ++count) {
                Asteroid asteroidNew = asteroid.Spawn();
                ChroneManager.AddListener(asteroidNew);
                activeAsteroid.Add(asteroidNew);
              }
          }
          Console.WriteLine();
        }

        else if (key.Key == ConsoleKey.Escape) {
          return;
        }
      }
    }
  }
}