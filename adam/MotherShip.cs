using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MotherShip {
  public List<HarvesterShip> Fleet;
  public Dictionary<string, List<Report>> Worklog;
  private List<Asteroid> _activeAsteroids;
  private AsteroidEmitter _asteroidEmitter;
  public bool IsStabilizing;
  public MotherShip(int harvesterCount, int cargoCapacity, int biteSize) {
    Fleet = new List<HarvesterShip>();
    Worklog = new Dictionary<string, List<Report>>();
    _activeAsteroids = new List<Asteroid>();
    IsStabilizing = true;

    for (int harvesterNumber = 1; harvesterNumber <= harvesterCount; ++harvesterNumber) {
      string name = $"Harvester-{harvesterNumber}";
      HarvesterShip harvester = new HarvesterShip(name, cargoCapacity, biteSize, this);
      Fleet.Add(harvester);
      Worklog[name] = new List<Report>();
      ChroneManager.AddListener(harvester);
    }
  }

  public void SetEmitter(AsteroidEmitter asteroidEmitter) {
    _asteroidEmitter = asteroidEmitter;
  }

  public void AddAsteroid(Asteroid asteroid) {
    _activeAsteroids.Add(asteroid);
  }

  public void RemoveAsteroid(Asteroid asteroid) {
    _activeAsteroids.Remove(asteroid);
    _asteroidEmitter.Recycle(asteroid);
    ChroneManager.RemoveListener(asteroid);
  }

  public Asteroid GetIdleAsteroid() {
    foreach (Asteroid asteroid in _activeAsteroids) {
      if (asteroid.State == AsteroidState.Idle) {
        return asteroid;
      }
    }
    return null;
  }

  public void AssignIdleHarvesters() {
    foreach (HarvesterShip harvester in Fleet) {
      if (harvester.State == HarvesterState.Idle) {
        Asteroid freeAsteroid = GetIdleAsteroid();
        if (freeAsteroid != null) {
          harvester.StartMining(freeAsteroid);
        }
      }
    }
  }

  public void FinishHarvest(HarvesterShip harvester) {
    Report report = harvester.CreateReport();
    Worklog[harvester.Name].Add(report);

    if (harvester.CurrentAsteroid != null) {
      if (harvester.CurrentAsteroid.State == AsteroidState.Depleted) {
        RemoveAsteroid(harvester.CurrentAsteroid);
      } else {
        harvester.CurrentAsteroid.State = AsteroidState.Idle;
      }
    }

    harvester.CargoCurrent = 0;
    harvester.AsteroidsMined = harvester.AsteroidsMined + 1;
    harvester.State = HarvesterState.Idle;
    harvester.CurrentAsteroid = null;
  }

  public void PrintAsteroidsInfo() {
    Console.WriteLine("Active asteroids:");
    foreach (Asteroid asteroid in _activeAsteroids) {
      asteroid.PrintInfo();
    }
  }

  public void PrintHarvestersInfo() {
    Console.WriteLine("Harvesters:");
    foreach (HarvesterShip harvester in Fleet) {
      harvester.PrintInfo();
    } 
  }

  public void PrintTotalMined() {
    Console.WriteLine("Total mined:");
    foreach (HarvesterShip harvester in Fleet) {
      int totalAmount = 0;
      foreach (Report report in Worklog[harvester.Name]) {
        totalAmount += report.AmountMined;
      }
      Console.WriteLine($"{harvester.Name}: {totalAmount} resources.");
    }
  }

  public void PrintFullWorklog() {
    Console.WriteLine("Full worklog:");
    foreach (KeyValuePair<string, List<Report>> harvesterEntry in Worklog) {
      Console.WriteLine($"{harvesterEntry.Key}:");
      foreach (Report report in harvesterEntry.Value) {
        report.Print();
      }
      Console.WriteLine();
    }
  }

  public int GetActiveAsteroidsCount() {
    return _activeAsteroids.Count;
  }

  public void SetStabilizing(bool active) {
    IsStabilizing = active;
  }
}
