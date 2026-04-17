using System;
using System.Collections.Generic;

public class MotherShip {
  public List<HarvesterShip> fleet;
  public Dictionary<string, List<Report>> worklog;

  private List<Asteroid> _activeAsteroids;
  private AsteroidEmitter? _asteroidEmitter;

  public MotherShip(int harvesterCount, int cargoCapacity, int biteSize) {
    fleet = new List<HarvesterShip>();
    worklog = new Dictionary<string, List<Report>>();
    _activeAsteroids = new List<Asteroid>();

    for (int index = 0; index < harvesterCount; ++index) {
      HarvesterShip harvester = new HarvesterShip(
        "Harvester-" + (index + 1), cargoCapacity, biteSize, this
      );
      fleet.Add(harvester);
      ChroneManager.AddListener(harvester);
      worklog[harvester.name] = new List<Report>();
    }
  }

  public void SetEmitter(AsteroidEmitter emitter) {
    _asteroidEmitter = emitter;
  }

  public void AddAsteroid(Asteroid asteroid) {
    _activeAsteroids.Add(asteroid);
  }

  public void RemoveAsteroid(Asteroid asteroid) {
    _activeAsteroids.Remove(asteroid);
    _asteroidEmitter!.Recycle(asteroid);
  }

  public Asteroid? GetIdleAsteroid() {
    foreach (Asteroid asteroid in _activeAsteroids) {
      if (asteroid.state == AsteroidState.Idle) {
        return asteroid;
      }
    }
    return null;
  }

  public void AssignIdleHarvesters() {
    foreach (HarvesterShip harvester in fleet) {
      if (harvester.state != HarvesterState.Idle) {
        continue;
      }
      Asteroid? idleAsteroid = GetIdleAsteroid();
      if (idleAsteroid == null) {
        break;
      }
      harvester.StartMining(idleAsteroid);
    }
  }

  public void FinishHarvest(HarvesterShip harvester) {
    Report report = harvester.CreateReport();
    worklog[harvester.name].Add(report);
    ++harvester.asteroidsMined;

    bool asteroidDepleted = harvester.currentAsteroid!.state == AsteroidState.Depleted;
    if (asteroidDepleted) {
      RemoveAsteroid(harvester.currentAsteroid);
    } else {
      harvester.currentAsteroid.state = AsteroidState.Idle;
    }

    harvester.cargoCurrent = 0;
    harvester.currentAsteroid = null;
    harvester.state = HarvesterState.Idle;
  }

  public void PrintAsteroidsInfo() {
    Console.WriteLine("--- Asteroids (" + _activeAsteroids.Count + ") ---");
    if (_activeAsteroids.Count == 0) {
      Console.WriteLine("  (none)");
      return;
    }
    foreach (Asteroid asteroid in _activeAsteroids) {
      asteroid.PrintInfo();
    }
  }

  public void PrintHarvestersInfo() {
    Console.WriteLine("--- Harvesters ---");
    foreach (HarvesterShip harvester in fleet) {
      harvester.PrintInfo();
    }
  }

  public void PrintTotalMined() {
    Console.WriteLine("--- Total Mined ---");
    foreach (HarvesterShip harvester in fleet) {
      int total = 0;
      foreach (Report report in worklog[harvester.name]) {
        total += report.amountMined;
      }
      Console.WriteLine(
        "  " + harvester.name +
        ": " + total + " echos" +
        " (" + harvester.asteroidsMined + " asteroids)"
      );
    }
  }

  public void PrintFullWorklog() {
    Console.WriteLine("=== FULL WORKLOG ===");
    foreach (HarvesterShip harvester in fleet) {
      Console.WriteLine("  [" + harvester.name + "]");
      List<Report> reports = worklog[harvester.name];
      if (reports.Count == 0) {
        Console.WriteLine("    (no reports)");
        continue;
      }
      foreach (Report report in reports) {
        report.Print();
      }
    }
  }

  public int GetActiveAsteroidsCount() {
    return _activeAsteroids.Count;
  }
}
