using System;
using System.Linq;
using System.Collections.Generic;

namespace AsteroidZoneSimulation.Models {
  public class MotherShip {
    private List<Asteroid> _activeAsteroids = new List<Asteroid>();
    private AsteroidEmitter _asteroidEmitter;

    public List<HarvesterShip> Fleet;
    public Dictionary<string, List<Report>> Worklog;

    public MotherShip(int harvesterCount, int cargoCapacity, int biteSize) {
      Fleet = new List<HarvesterShip>();
      Worklog = new Dictionary<string, List<Report>>();

      for (int harvesterIndex = 1; harvesterIndex <= harvesterCount; ++harvesterIndex) {
        string name = $"Харвестер-{harvesterIndex}";
        HarvesterShip harvester = new HarvesterShip(name, cargoCapacity, biteSize, this);
        Fleet.Add(harvester);
        Worklog[name] = new List<Report>();
      }

      Console.WriteLine($"[Станция] Матриарх создана. Флот: {harvesterCount} харвестеров");
    }

    public void SetEmitter(AsteroidEmitter emitter) {
      _asteroidEmitter = emitter;
    }

    public void AddAsteroid(Asteroid asteroid) {
      _activeAsteroids.Add(asteroid);
      Console.WriteLine($"[Станция] Добавлен астероид (SpawnID): {asteroid.SpawnID} (всего: {_activeAsteroids.Count})");
    }

    public void RemoveAsteroid(Asteroid asteroid) {
      _activeAsteroids.Remove(asteroid);

      if (_asteroidEmitter != null) {
        _asteroidEmitter.Recycle(asteroid);
      }
    }

    public Asteroid GetIdleAsteroid() {
      return _activeAsteroids.FirstOrDefault(asteroid => asteroid.State == AsteroidState.Idle);
    }

    public void AssignIdleHarvesters() {
      var idleHarvesters = Fleet.Where(harvester => harvester.State == HarvesterState.Idle).ToList();

      foreach (var harvester in idleHarvesters) {
        Asteroid targetAsteroid = GetIdleAsteroid();

        if (targetAsteroid != null) {
          harvester.StartMining(targetAsteroid);
          Console.WriteLine($"[Станция] {harvester.Name} начал добычу астероида (SpawnID): {targetAsteroid.SpawnID}");
        }
      }
    }

    public void FinishHarvest(HarvesterShip harvester) {
      Console.WriteLine($"\n[Станция] {harvester.Name} завершил добычу!");

      Report report = harvester.CreateReport();
      Worklog[harvester.Name].Add(report);

      Console.WriteLine($"  Добыто ресурса: {report.AmountMined} ед. с астероида (SpawnID):{report.AsteroidSpawnID}");

      harvester.Unload();

      Asteroid asteroid = harvester.CurrentAsteroid;
      if (asteroid != null) {
        if (asteroid.State == AsteroidState.Depleted) {
          Console.WriteLine($"  Астероид (SpawnID): {asteroid.SpawnID} истощён! Возвращается в пул");
          RemoveAsteroid(asteroid);
        } else {
          asteroid.State = AsteroidState.Idle;
          Console.WriteLine($"  Астероид (SpawnID): {asteroid.SpawnID} освобождён (осталось {asteroid.CurrentEchos} ед.)");
        }
      }

      harvester.ResetAfterHarvest();
      Console.WriteLine($"  {harvester.Name} разгружен на станции и готов к новой добыче\n");
    }

    public void PrintAsteroidsInfo() {
      Console.WriteLine($"\n--- АКТИВНЫЕ АСТЕРОИДЫ ({_activeAsteroids.Count}) ---");

      if (_activeAsteroids.Count == 0) {
        Console.WriteLine("  Нет активных астероидов");
      } else {
        foreach (var asteroid in _activeAsteroids) {
          asteroid.DisplayAsteroidInformation();
        }
      }
    }

    public void PrintHarvestersInfo() {
      Console.WriteLine($"\n--- ФЛОТ ХАРВЕСТЕРОВ ({Fleet.Count}) ---");

      foreach (var harvester in Fleet) {
        harvester.PrintInfo();
      }
    }

    public void PrintTotalMined() {
      Console.WriteLine("\n--- СУММАРНАЯ ДОБЫЧА ПО ХАРВЕСТЕРАМ ---");

      foreach (var harvester in Fleet) {
        int totalMined = Worklog[harvester.Name].Sum(result => result.AmountMined);
        Console.WriteLine($"  {harvester.Name}: {totalMined} ед. ресурса (астероидов: {harvester.AsteroidsMined})");
      }
    }

    public void PrintFullWorklog() {
      Console.WriteLine("\n---------------------------------------------------------------" +
                        "\n                    ПОЛНЫЙ ЖУРНАЛ РАБОТ (WORKLOG)" +
                        "\n---------------------------------------------------------------");

      foreach (var harvester in Fleet) {
        Console.WriteLine($"\n{harvester.Name}:");

        var reports = Worklog[harvester.Name];
        if (reports.Count == 0) {
          Console.WriteLine("  Нет завершённых заданий");
        } else {
          foreach (var report in reports) {
            report.Print();
          }
        }
      }

      Console.WriteLine("---------------------------------------------------------------\n");
    }

    public int GetActiveAsteroidsCount() {
      return _activeAsteroids.Count;
    }
  }
}