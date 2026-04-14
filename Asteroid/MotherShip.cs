using System;
using System.Collections.Generic;

namespace Asteroid {
  public class MotherShip {
    private List<HarvesterShip> _fleet;
    private Dictionary<string, List<Report>> _worklog;
    private AsteroidEmitter _asteroidEmitter;
    private List<Asteroid> _activeAsteroids;

    public MotherShip(AsteroidEmitter asteroidEmitter, List<Asteroid> activeAsteroids) {
      _asteroidEmitter = asteroidEmitter;
      _activeAsteroids = activeAsteroids;
      _fleet = new List<HarvesterShip>();
      _worklog = new Dictionary<string, List<Report>>();

      int fleetSize;
      fleetSize = 5;

      for (int shipIndex = 0; shipIndex < fleetSize; ++shipIndex) {
        HarvesterShip newShip;
        newShip = new HarvesterShip(this);

        _fleet.Add(newShip);
        ChronManager.AddListener(newShip);
      }
    }

    public List<HarvesterShip> GetFleet() {
      return _fleet;
    }

    public Asteroid GetAvailableAsteroid() {
      foreach (Asteroid asteroid in _activeAsteroids) {
        if (asteroid.state == AsteroidState.Idle) {
          return asteroid;
        }
      }

      return null;
    }

    public void RecycleAsteroid(Asteroid asteroid) {
      _asteroidEmitter.Recycle(asteroid);
      _activeAsteroids.Remove(asteroid);
    }

    public void AddReport(string harvesterName, Report report) {
      if (!_worklog.ContainsKey(harvesterName)) {
        _worklog[harvesterName] = new List<Report>();
      }

      _worklog[harvesterName].Add(report);
    }

    public int GetHarvesterTotalMined(string harvesterName) {
      int total;
      total = 0;

      if (_worklog.ContainsKey(harvesterName)) {
        foreach (Report report in _worklog[harvesterName]) {
          total = total + report.amountMined;
        }
      }

      return total;
    }

    public void PrintHarvesterTotals() {
      Console.WriteLine("\n--- HARVESTER TOTALS ---");

      foreach (HarvesterShip ship in _fleet) {
        int totalMined;
        totalMined = GetHarvesterTotalMined(ship.name);

        Console.WriteLine($"  {ship.name,-14}: {totalMined,5} units total");
      }
    }

    public void PrintFullWorklog() {
      Console.WriteLine("\n=== FULL WORKLOG ===");

      foreach (KeyValuePair<string, List<Report>> entry in _worklog) {
        Console.WriteLine($"\n{entry.Key}:");

        foreach (Report report in entry.Value) {
          Console.WriteLine($"    {report}");
        }
      }
    }

    public void PrintFleetStatus() {
      Console.WriteLine("\n--- HARVESTER FLEET ---");

      foreach (HarvesterShip ship in _fleet) {
        Console.WriteLine($"  {ship}");
      }
    }
  }
}
