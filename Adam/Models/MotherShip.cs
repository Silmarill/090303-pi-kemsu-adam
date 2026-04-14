using AsteroidSimulation.Models;
using System;
using System.Collections.Generic;

namespace Adam.Models {
  public class MotherShip {
    public List<HarvesterShip> fleet;
    public Dictionary<string, List<Report>> worklog;
    public int chronCounter;

    public int chronIntervalForFullWorklog;
    public int defaultCargoCapacity;
    public int defaultBiteSize;
    public int initialFleetSize;

    public MotherShip() {
      chronIntervalForFullWorklog = 15;
      defaultCargoCapacity = 500;
      defaultBiteSize = 50;
      initialFleetSize = 5;
      fleet = new List<HarvesterShip>();
      worklog = new Dictionary<string, List<Report>>();
      chronCounter = 0;
      InitializeFleet();
    }

    HarvesterShip newShip;

    private void InitializeFleet() {
      string[] shipNames = { "Vindicator", "Reaper", "Marauder", "HarvesterOne", "Collector" };

      for (int shipIndex = 0; shipIndex < initialFleetSize; ++shipIndex) {
        newShip = new HarvesterShip(shipNames[shipIndex], defaultCargoCapacity, defaultBiteSize);
        fleet.Add(newShip);
        worklog.Add(newShip.name, new List<Report>());
      }
    }

    public void ProcessChronTick(List<Asteroid> activeAsteroids) {
      ++chronCounter;

      ProcessIdleHarvesters(activeAsteroids);
      ProcessMiningHarvesters();

      Console.WriteLine("\n=== ASTEROIDS STATE ===");
      foreach (Asteroid asteroid in activeAsteroids) {
        asteroid.PrintInfo();
      }

      Console.WriteLine("\n=== HARVESTERS STATE ===");
      foreach (HarvesterShip harvester in fleet) {
        harvester.PrintInfo();
      }

      ShowTotalMinedPerHarvester();

      if (chronCounter % chronIntervalForFullWorklog == 0) {
        ShowFullWorklog();
      }
    }

    Asteroid targetAsteroid;

    private void ProcessIdleHarvesters(List<Asteroid> activeAsteroids) {
      foreach (HarvesterShip harvester in fleet) {
        if (harvester.state == HarvesterState.Idle) {
          targetAsteroid = FindAvailableAsteroid(activeAsteroids);
          if (targetAsteroid != null) {
            harvester.TryStartMining(targetAsteroid);
          }
        }
      }
    }

    private Asteroid FindAvailableAsteroid(List<Asteroid> asteroidsList) {
      int asteroidSpawnId;
      int newJobNumber;
      Report completedReport;

      foreach (Asteroid asteroid in asteroidsList) {
        if (asteroid.state == AsteroidState.Idle && asteroid.isBeingMined == false) {
          return asteroid;
        }
      }
      return null;
    }

    private void ProcessMiningHarvesters() {
      List<HarvesterShip> harvestersReadyToUnload = new List<HarvesterShip>();

      foreach (HarvesterShip harvester in fleet) {
        if (harvester.state == HarvesterState.Mining) {
          harvester.MineCurrentAsteroid();

          if (harvester.state == HarvesterState.Idle && harvester.cargoCurrent > 0) {
            harvestersReadyToUnload.Add(harvester);
          }
        }
      }

      foreach (HarvesterShip harvester in harvestersReadyToUnload) {
        int asteroidSpawnId;
        int newJobNumber;
        int nextJobIncrement;
        int invalidAsteroidId;
        Report completedReport;

        nextJobIncrement = 1;
        invalidAsteroidId = -1;
        asteroidSpawnId = (harvester.currentAsteroid != null) ? harvester.currentAsteroid.spawnId : invalidAsteroidId;
        newJobNumber = worklog[harvester.name].Count + nextJobIncrement;
        completedReport = harvester.UnloadAndCreateReport(newJobNumber, asteroidSpawnId);
        worklog[harvester.name].Add(completedReport);
        harvester.ResetForNewJob();
      }
    }

    private void ShowTotalMinedPerHarvester() {
      double totalMinedAmount;

      totalMinedAmount = 0;
      Console.WriteLine("\n=== TOTAL MINED PER HARVESTER ===");

      foreach (HarvesterShip harvester in fleet) {
        foreach (Report report in worklog[harvester.name]) {
          totalMinedAmount += report.amountMined;
        }
        Console.WriteLine(harvester.name + ": " + totalMinedAmount + " units");
      }
    }

    private void ShowFullWorklog() {
      Console.WriteLine("\n=== FULL WORKLOG (CHRON #" + chronCounter + ") ===");

      foreach (KeyValuePair<string, List<Report>> entry in worklog) {
        Console.WriteLine("\n--- " + entry.Key + " ---");
        if (entry.Value.Count == 0) {
          Console.WriteLine("No reports yet.");
        } else {
          foreach (Report report in entry.Value) {
            report.PrintReport();
          }
        }
      }
    }

    public Dictionary<string, List<Report>> GetWorklog() {
      return worklog;
    }
  }
}