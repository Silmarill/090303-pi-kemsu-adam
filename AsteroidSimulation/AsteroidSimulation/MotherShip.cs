using System;
using System.Collections.Generic;

namespace AsteroidSimulation {
  public class MotherShip {
    private const int DefaultHarvesterCount = 5;
    private const int DefaultCargoCapacity = 500;
    private const int DefaultBiteSize = 50;

    private List<HarvesterShip> _fleet = new List<HarvesterShip>();
    private Dictionary<string, List<Report>> _workLog = new Dictionary<string, List<Report>>();
    private List<Asteroid> _activeAsteroidItems = new List<Asteroid>();
    private AsteroidEmitter _asteroidEmitter;

    public List<HarvesterShip> Fleet {
      get { return _fleet; }
    }

    public Dictionary<string, List<Report>> WorkLog {
      get { return _workLog; }
    }

    public MotherShip() : this(DefaultHarvesterCount, DefaultCargoCapacity, DefaultBiteSize) {
    }

    public MotherShip(int harvesterCount, int cargoCapacity, int biteSize) {
      for (int harvesterIndex = 0; harvesterIndex < harvesterCount; ++harvesterIndex) {
        string harvesterName = "Harvester" + (harvesterIndex + 1);
        HarvesterShip newHarvester = new HarvesterShip(harvesterName, cargoCapacity, biteSize, this);
        _fleet.Add(newHarvester);
        _workLog[harvesterName] = new List<Report>();
      }
    }

    public void SetEmitter(AsteroidEmitter emitter) {
      _asteroidEmitter = emitter;
    }

    public void AddAsteroid(Asteroid asteroid) {
      _activeAsteroidItems.Add(asteroid);
    }

    public void RemoveAsteroid(Asteroid asteroid) {
      _activeAsteroidItems.Remove(asteroid);
      _asteroidEmitter.Recycle(asteroid);
    }

    public Asteroid GetIdleAsteroid() {
      for (int asteroidIndex = 0; asteroidIndex < _activeAsteroidItems.Count; ++asteroidIndex) {
        if (_activeAsteroidItems[asteroidIndex].State == AsteroidState.Idle) {
          return _activeAsteroidItems[asteroidIndex];
        }
      }
      return null;
    }

    public void FinishHarvest(HarvesterShip harvester) {
      if (harvester.currentAsteroid != null) {
        int newJobNumber = _workLog[harvester.GetName()].Count + 1;
        Report newReport = harvester.CreateReport(newJobNumber);
        _workLog[harvester.GetName()].Add(newReport);

        if (harvester.currentAsteroid.State == AsteroidState.Depleted) {
          RemoveAsteroid(harvester.currentAsteroid);
        }
      }
      harvester.Unload();
    }

    public void PrintWorkLog() {
      Console.WriteLine("\n========== WORKLOG ==========");
      foreach (var workEntry in _workLog) {
        Console.WriteLine("\n" + workEntry.Key + ":");
        if (workEntry.Value.Count == 0) {
          Console.WriteLine("  No reports yet");
        }
        else {
          foreach (var report in workEntry.Value) {
            Console.WriteLine("  " + report.ToString());
          }
        }
      }
      Console.WriteLine("=============================\n");
    }

    public void PrintTotalMined() {
      Console.WriteLine("\n========== TOTAL MINED ==========");
      foreach (var workEntry in _workLog) {
        int totalMined = 0;
        foreach (var report in workEntry.Value) {
          totalMined += report.AmountMined;
        }
        Console.WriteLine(workEntry.Key + ": " + totalMined + " units");
      }
      Console.WriteLine("==================================\n");
    }

    public void UpdateHarvesters() {
      for (int harvesterIndex = 0; harvesterIndex < _fleet.Count; ++harvesterIndex) {
        if (_fleet[harvesterIndex].GetState() == HarvesterState.Idle) {
          Asteroid targetAsteroid = GetIdleAsteroid();
          if (targetAsteroid != null) {
            _fleet[harvesterIndex].StartMining(targetAsteroid);
          }
        }
      }
    }
  }
}