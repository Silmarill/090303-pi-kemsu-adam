using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp16 {
  class Program {
    private static int InitialAsteroidPoolSize = 10;
    private static int InitialActiveAsteroidsCount = 5;
    private static int NewAsteroidSpawnInterval = 5;
    private static int MinNewAsteroidsPerSpawn = 1;
    private static int MaxNewAsteroidsPerSpawn = 4;
    private static int WorklogDisplayInterval = 15;

    static void Main() {
      var emitter = new AsteroidEmitter(InitialAsteroidPoolSize);

      List<Asteroid> activeAsteroids = new List<Asteroid>();

      for (int asteroidIndex = 0; asteroidIndex < InitialActiveAsteroidsCount; ++asteroidIndex) {
        var asteroid = emitter.Spawn();
        activeAsteroids.Add(asteroid);
        ChroneManager.AddListener(asteroid);
        Console.WriteLine("Asteroid ID: " + asteroid.CreateID + ", MaxEchos: " + asteroid.MaxEchos + ", CurrentEchos: " + asteroid.CurrentEchos + ", State: " + asteroid.State + ", SpawnID: " + asteroid.SpawnID);
      }

      var motherShip = new MotherShip(emitter, activeAsteroids);
      int chronCount = 0;

      Console.WriteLine("\nНажмите Enter для перехода к следующему хрону, Esc — для выхода.");

      while (true) {
        var key = Console.ReadKey().KeyChar;

        if (key == 27) {
          break;
        }

        if (key == 13) {
          chronCount++;
          ProcessChron(chronCount, emitter, activeAsteroids);
          DisplayAsteroids(activeAsteroids);
          motherShip.ProcessChron();

          DisplayHarvestersInfo(motherShip);
          DisplayTotalMined(motherShip);

          if (chronCount % WorklogDisplayInterval == 0) {
            DisplayFullWorklog(motherShip);
          }
        }
      }
    }

    static void ProcessChron(int chronCount, AsteroidEmitter emitter, List<Asteroid> activeAsteroids) {
      ChroneManager.MakeChroneTick();

      var depletedAsteroids = activeAsteroids.Where(a => a.State == AsteroidState.Depleted).ToList();
      foreach (var asteroid in depletedAsteroids) {
        activeAsteroids.Remove(asteroid);
        emitter.Recycle(asteroid);
        ChroneManager.RemoveListener(asteroid);
        Console.WriteLine("Астероид " + asteroid.CreateID + " исчерпан и возвращён в пул.");
      }

      if (chronCount % NewAsteroidSpawnInterval == 0) {
        var random = new Random();
        var newCount = random.Next(MinNewAsteroidsPerSpawn, MaxNewAsteroidsPerSpawn + 1);
        for (int asteroidIndex = 0; asteroidIndex < newCount; ++asteroidIndex) {
          var newAsteroid = emitter.Spawn();
          activeAsteroids.Add(newAsteroid);
          ChroneManager.AddListener(newAsteroid);
          Console.WriteLine("Спавн нового астероида: ID=" + newAsteroid.CreateID + ", Echos=" + newAsteroid.CurrentEchos);
        }
      }

      Console.WriteLine("Хрон " + chronCount + " обработан.");
    }

    static void DisplayAsteroids(List<Asteroid> activeAsteroids) {
      Console.WriteLine("\nАктивные астероиды:");
      foreach (var asteroid in activeAsteroids) {
        Console.WriteLine("ID: " + asteroid.CreateID + ", Echos: " + asteroid.CurrentEchos + "/" + asteroid.MaxEchos + ", Состояние: " + asteroid.State + ", SpawnID: " + asteroid.SpawnID);
      }

      Console.WriteLine();
    }

    static void DisplayHarvestersInfo(MotherShip motherShip) {
      Console.WriteLine("\n=== Состояние харвестеров ===");
      foreach (var harvester in motherShip.Fleet) {
        Console.WriteLine(harvester.GetHarvesterInfo());
      }
    }

    static void DisplayTotalMined(MotherShip motherShip) {
      Console.WriteLine("\n=== Суммарная добыча ===");
      foreach (var harvester in motherShip.Fleet) {

        int totalMined = 0;

        foreach (var report in motherShip.Worklog[harvester.Name]) {
          totalMined += report.AmountMined;
        }

        Console.WriteLine(harvester.Name + ": " + totalMined + " ед. Echos (астероидов: " + harvester.AsteroidMined + ")");
      }
    }

    static void DisplayFullWorklog(MotherShip motherShip) {
      Console.WriteLine("\nПОЛНЫЙ ЖУРНАЛ РАБОТ");
      foreach (var harvester in motherShip.Fleet) {
        Console.WriteLine("\n" + harvester.Name + ":");
        if (motherShip.Worklog[harvester.Name].Count == 0) {
          Console.WriteLine("  Нет завершённых заданий");
        } else {
          foreach (var report in motherShip.Worklog[harvester.Name]) {
            Console.WriteLine("  " + report.GetReportString());
          }

          int totalMined = 0;

          foreach (var report in motherShip.Worklog[harvester.Name]) {
            totalMined += report.AmountMined;
          }

          Console.WriteLine("  Итого: " + totalMined + " ед. Echos");
        }
      }
    }
  }
}