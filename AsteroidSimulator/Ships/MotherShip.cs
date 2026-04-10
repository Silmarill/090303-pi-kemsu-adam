using System;
using System.Collections.Generic;
using AsteroidSimulator.Asteroids;
using AsteroidSimulator.Reports;

namespace AsteroidSimulator.Ships;

public class MotherShip {
  public List<HarvesterShip> fleet;
  public Dictionary<string, List<Report>> worklog;
  public bool isActive;

  private const int FleetSize = 5;

  public MotherShip() {
    fleet = new List<HarvesterShip>();
    worklog = new Dictionary<string, List<Report>>();
    isActive = true;

    for (int i = 0; i < FleetSize; i++) {
      HarvesterShip harvester = new HarvesterShip("Harvester");
      fleet.Add(harvester);
      worklog[harvester.name] = new List<Report>();
    }
  }

  public void StabilizeZone() {
    isActive = true;
  }

  public void UpdateHarvesters(List<Asteroid> activeAsteroids) {
    foreach (HarvesterShip harvester in fleet) {
      if (harvester.state == HarvesterState.Mining) {
        bool continueMining = harvester.Mine();

        if (continueMining == false) {
          FinishHarvesting(harvester);
        }
      }
    }

    foreach (HarvesterShip harvester in fleet) {
      if (harvester.state == HarvesterState.Idle) {
        Asteroid targetAsteroid = FindAvailableAsteroid(activeAsteroids);

        if (targetAsteroid != null) {
          harvester.StartMining(targetAsteroid);
        }
      }
    }
  }

  private Asteroid FindAvailableAsteroid(List<Asteroid> asteroids) {
    foreach (Asteroid asteroid in asteroids) {
      if (asteroid.state == AsteroidState.Idle) {
        return asteroid;
      }
    }
    return null;
  }

  private void FinishHarvesting(HarvesterShip harvester) {
    if (harvester.currentAsteroid != null) {
      int asteroidSpawnId = harvester.currentAsteroid.spawnId;
      int amountMined = harvester.cargoCurrent;

      Report report = harvester.CreateReport(asteroidSpawnId, amountMined);
      worklog[harvester.name].Add(report);

      harvester.IncrementAsteroidsMined();
      harvester.Unload();
    } else {
      harvester.Unload();
    }
  }

  public int GetTotalMinedForHarvester(string harvesterName) {
    if (worklog.ContainsKey(harvesterName) == false) {
      return 0;
    }

    int total = 0;
    foreach (Report report in worklog[harvesterName]) {
      total = total + report.AmountMined;
    }
    return total;
  }

  public void PrintWorklog() {
    string output = "\n================== ПОЛНЫЙ WORKLOG ==================\n";

    foreach (HarvesterShip harvester in fleet) {
      output = output + "\n📋 " + harvester.name + ":\n";

      if (worklog.ContainsKey(harvester.name) == false || worklog[harvester.name].Count == 0) {
        output = output + "   Нет отчётов\n";
      } else {
        foreach (Report report in worklog[harvester.name]) {
          output = output + "   Задание #" + report.JobNumber + " | Астероид #" + report.AsteroidSpawnId + " | Добыто: " + report.AmountMined + "\n";
        }
        output = output + "   ИТОГО: " + GetTotalMinedForHarvester(harvester.name) + " ресурсов\n";
      }
    }
    output = output + "===================================================\n";

    Console.Write(output);
  }

  public void PrintHarvesterStatus() {
    string output = "\n================== ФЛОТ ХАРВЕСТЕРОВ ==================\n";

    foreach (HarvesterShip harvester in fleet) {
      string statusSymbol = (harvester.state == HarvesterState.Idle) ? "⚓" : "⛏";
      string cargoInfo = harvester.cargoCurrent + "/" + harvester.cargoCapacity;
      string asteroidInfo = "";

      if (harvester.currentAsteroid != null) {
        asteroidInfo = " → астероид #" + harvester.currentAsteroid.spawnId;
      }

      output = output + "   " + statusSymbol + " " + harvester.name;
      output = output + " | " + harvester.state.ToString();
      output = output + " | Груз: " + cargoInfo;
      output = output + " | Добыто: " + harvester.asteroidsMined;
      output = output + asteroidInfo + "\n";
    }

    output = output + "\n================== СУММАРНАЯ ДОБЫЧА ==================\n";

    foreach (HarvesterShip harvester in fleet) {
      int total = GetTotalMinedForHarvester(harvester.name);
      output = output + "   " + harvester.name + ": " + total + " ресурсов\n";
    }

    output = output + "===================================================\n";

    Console.Write(output);
  }
}