using System;
using System.Collections.Generic;

public class MotherShip : IChroneListener {
  public List<HarvesterShip> fleet;
  public Dictionary<string, List<Report>> worklog;

  private const int fleetSize = 5;

  private int globalJobCounter;
  private List<Asteroid> activeAsteroids;

  public MotherShip() {
    fleet = new List<HarvesterShip>();
    worklog = new Dictionary<string, List<Report>>();

    globalJobCounter = 0;
    activeAsteroids = null;

    for (int i = 1; i <= fleetSize; ++i) {
      string name = $"Harvester-{i:D2}";

      HarvesterShip ship = new HarvesterShip(i, name);
      ship.SetMotherShip(this);

      fleet.Add(ship);
      worklog[name] = new List<Report>();

      ChroneManager.AddListener(ship);
    }

    ChroneManager.AddListener(this);
  }

  public void OnChroneTick() {
    AssignTargets();
  }

  private void AssignTargets() {
    if (activeAsteroids == null) {
      return;
    }

    for (int i = 0; i < fleet.Count; ++i) {
      var ship = fleet[i];

      if (ship.state != HarvesterState.Idle) {
        continue;
      }

      for (int j = 0; j < activeAsteroids.Count; ++j) {
        var asteroid = activeAsteroids[j];

        if (asteroid.state == AsteroidState.Idle) {
          if (ship.TryAssignTarget(asteroid)) {
            break;
          }
        }
      }
    }
  }

  public void SetActiveAsteroids(List<Asteroid> activeAsteroids) {
    this.activeAsteroids = activeAsteroids;
  }

  public void DeliverReport(Asteroid asteroid, int spawnId, int amountMined) {
    if (amountMined <= 0) {
      return;
    }

    globalJobCounter++;

    var report = new Report(globalJobCounter, spawnId, amountMined);

    string key = null;

    // ищем владельца по текущему харвестеру (упрощённо по факту работы)
    for (int i = 0; i < fleet.Count; ++i) {
      if (fleet[i].asteroidsMined > 0) {
        key = fleet[i].name;
        break;
      }
    }

    if (key != null && worklog.ContainsKey(key)) {
      worklog[key].Add(report);
    }
  }

  public void PrintFullWorklog() {
    Console.WriteLine("\n=== Full Worklog ===");

    foreach (var kv in worklog) {
      Console.WriteLine($"Harvester: {kv.Key} ({kv.Value.Count})");

      for (int i = 0; i < kv.Value.Count; ++i) {
        Console.WriteLine($"  {kv.Value[i]}");
      }
    }

    Console.WriteLine("====================\n");
  }

  public void PrintSummary() {
    Console.WriteLine("\n=== Summary ===");

    for (int i = 0; i < fleet.Count; ++i) {
      var ship = fleet[i];

      int total = 0;

      if (worklog.ContainsKey(ship.name)) {
        var list = worklog[ship.name];

        for (int j = 0; j < list.Count; ++j) {
          total += list[j].amountMined;
        }
      }

      Console.WriteLine($"{ship.name}: {total} Echos | Asteroids mined: {ship.asteroidsMined}");
    }
  }
}