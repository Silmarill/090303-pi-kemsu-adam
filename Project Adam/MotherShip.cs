using System;
using System.Collections.Generic;
using AsteroidSimulator;
using AsteroidSimulator.Interfaces;
using AsteroidSimulator.Managers;

namespace AsteroidSimulator.Models {
  public class MotherShip : IChronListener {
    private readonly List<Asteroid> _belt;
    private readonly Random _random;
    private readonly Dictionary<string, int> _lifetimeMinedByHarvester;

    public List<HarvesterShip> Fleet { get; }
    public Dictionary<string, List<Report>> Worklog { get; }

    public MotherShip(int harvesterCount, List<Asteroid> belt, Random random) {
      string[] names;
      int shipIndex;
      int maxHarvesterSlots;

      names = new string[] { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };
      maxHarvesterSlots = names.Length;

      _belt = belt;
      _random = random;
      Fleet = new List<HarvesterShip>();
      Worklog = new Dictionary<string, List<Report>>();
      _lifetimeMinedByHarvester = new Dictionary<string, int>();

      for (shipIndex = 0; shipIndex < harvesterCount && shipIndex < maxHarvesterSlots; ++shipIndex) {
        HarvesterShip ship;
        string harvesterName;
        int harvesterId;

        harvesterName = names[shipIndex];
        harvesterId = shipIndex + 1;
        ship = new HarvesterShip(
          harvesterId,
          harvesterName,
          SimulationConstants.HarvesterCargoCapacity,
          SimulationConstants.HarvesterBiteSize);
        ship.BindMotherShip(this);
        Fleet.Add(ship);
        Worklog[harvesterName] = new List<Report>();
        _lifetimeMinedByHarvester[harvesterName] = 0;
      }
    }

    public void RegisterWithChronManager() {
      ChronManager.AddListener(this);
    }

    public void AppendHarvesterJobReport(HarvesterShip ship, int asteroidSpawnId, int amountMined, bool asteroidFullyConsumed) {
      int jobNumber;
      Report report;

      if (amountMined <= 0) {
        return;
      }

      jobNumber = Worklog[ship.Name].Count + 1;
      report = new Report(jobNumber, asteroidSpawnId, amountMined);
      Worklog[ship.Name].Add(report);
      _lifetimeMinedByHarvester[ship.Name] = _lifetimeMinedByHarvester[ship.Name] + amountMined;

      if (asteroidFullyConsumed) {
        ship.RegisterAsteroidFullyProcessed();
      }
    }

    public void NotifyAsteroidLeavingBelt(Asteroid asteroid) {
      int fleetIndex;

      for (fleetIndex = 0; fleetIndex < Fleet.Count; ++fleetIndex) {
        Fleet[fleetIndex].ForcedRetreatFromLostAsteroid(this, asteroid);
      }
    }

    public Dictionary<string, int> GetLifetimeMinedSnapshot() {
      return new Dictionary<string, int>(_lifetimeMinedByHarvester);
    }

    public void OnChronTick() {
      AssignIdleHarvestersToBelt();
      ProcessMiningForFleet();
    }

    private void ProcessMiningForFleet() {
      int fleetIndex;

      for (fleetIndex = 0; fleetIndex < Fleet.Count; ++fleetIndex) {
        HarvesterShip ship;
        Asteroid target;

        ship = Fleet[fleetIndex];
        target = ship.AssignedAsteroid;

        if (ship.State == HarvesterState.Mining && target != null) {
          ship.Mine(target);
        }
      }
    }

    private void AssignIdleHarvestersToBelt() {
      int fleetIndex;

      for (fleetIndex = 0; fleetIndex < Fleet.Count; ++fleetIndex) {
        HarvesterShip ship;
        Asteroid candidate;

        ship = Fleet[fleetIndex];

        if (ship.State != HarvesterState.Idle) {
          continue;
        }

        candidate = PickRandomUnassignedIdleAsteroid();
        if (candidate != null) {
          ship.AssignToAsteroid(candidate);
        }
      }
    }

    private Asteroid PickRandomUnassignedIdleAsteroid() {
      List<Asteroid> candidates;
      int beltIndex;
      int fleetIndex;
      int pickIndex;

      candidates = new List<Asteroid>();

      for (beltIndex = 0; beltIndex < _belt.Count; ++beltIndex) {
        Asteroid rock;
        bool alreadyTaken;

        rock = _belt[beltIndex];
        if (rock.State != AsteroidState.Idle) {
          continue;
        }

        alreadyTaken = false;
        for (fleetIndex = 0; fleetIndex < Fleet.Count; ++fleetIndex) {
          if (Fleet[fleetIndex].AssignedAsteroid == rock) {
            alreadyTaken = true;
            break;
          }
        }

        if (!alreadyTaken) {
          candidates.Add(rock);
        }
      }

      if (candidates.Count == 0) {
        return null;
      }

      pickIndex = _random.Next(candidates.Count);
      return candidates[pickIndex];
    }

    public void PrintFullWorklog() {
      foreach (KeyValuePair<string, List<Report>> entry in Worklog) {
        int reportIndex;

        Console.WriteLine("Harvester " + entry.Key + " — полный журнал (" + entry.Value.Count + "):");
        for (reportIndex = 0; reportIndex < entry.Value.Count; ++reportIndex) {
          Console.WriteLine("  " + entry.Value[reportIndex]);
        }
      }
    }

    public void PrintTotalMinedByHarvester() {
      Console.WriteLine("--- Суммарная добыча (Echos) по харвестерам ---");
      foreach (KeyValuePair<string, int> entry in _lifetimeMinedByHarvester) {
        Console.WriteLine("  " + entry.Key + ": " + entry.Value);
      }
    }
  }
}
