using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp16 {
  public class MotherShip {
    public List<HarvesterShip> Fleet;
    public Dictionary<string, List<Report>> Worklog;

    private AsteroidEmitter _asteroidEmitter;
    private List<Asteroid> _activeAsteroids;
    private int _chronCounter;

    private const int FleetSize = 5;
    private const int WorklogDisplayInterval = 15;
    private const int NewAsteroidSpawnInterval = 5;

    private string[] HarvesterNames = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };

    public MotherShip(AsteroidEmitter asteroidEmitter, List<Asteroid> activeAsteroids) {
      _asteroidEmitter = asteroidEmitter;
      _activeAsteroids = activeAsteroids;
      _chronCounter = 0;

      Fleet = new List<HarvesterShip>();

      for (int harvesterIndex = 0; harvesterIndex < FleetSize; ++harvesterIndex) {
        Fleet.Add(new HarvesterShip(HarvesterNames[harvesterIndex]));
      }

      Worklog = new Dictionary<string, List<Report>>();

      foreach (HarvesterShip currentHarvester in Fleet) {
        Worklog[currentHarvester.Name] = new List<Report>();
      }
    }

    public void ProcessChron() {
      ++_chronCounter;  // Префиксный инкремент

      AssignMiningTasks();
      ProcessMining();

      DisplayHarvestersInfo();
      DisplayTotalMined();

      if (_chronCounter % WorklogDisplayInterval == 0) {
        DisplayFullWorklog();
      }
    }

    private void AssignMiningTasks() {
      List<HarvesterShip> idleHarvesters = new List<HarvesterShip>();
      foreach (HarvesterShip currentHarvester in Fleet) {
        if (currentHarvester.State == HarvesterState.Idle) {
          idleHarvesters.Add(currentHarvester);
        }
      }

      List<Asteroid> availableAsteroids = new List<Asteroid>();
      foreach (Asteroid currentAsteroid in _activeAsteroids) {
        if (currentAsteroid.State == AsteroidState.Idle && currentAsteroid.CurrentEchos > 0) {
          availableAsteroids.Add(currentAsteroid);
        }
      }

      foreach (HarvesterShip idleHarvester in idleHarvesters) {
        if (availableAsteroids.Count == 0) {
          break;
        }

        Asteroid targetAsteroid = availableAsteroids[0];
        availableAsteroids.RemoveAt(0);

        int jobNumber = Worklog[idleHarvester.Name].Count + 1;
        idleHarvester.StartMining(targetAsteroid, jobNumber);

        Console.WriteLine("  " + idleHarvester.Name + " начал добычу астероида #" + targetAsteroid.SpawnID + " (Задание #" + jobNumber + ")");
      }
    }

    private void ProcessMining() {
      List<HarvesterShip> activeHarvesters = new List<HarvesterShip>();
      foreach (HarvesterShip currentHarvester in Fleet) {
        if (currentHarvester.State == HarvesterState.Mining) {
          activeHarvesters.Add(currentHarvester);
        }
      }

      foreach (HarvesterShip miningHarvester in activeHarvesters) {
        bool isFinished = miningHarvester.ContinueMining();

        if (isFinished) {
          Report completedReport = miningHarvester.FinishMining();
          if (completedReport != null) {
            Worklog[miningHarvester.Name].Add(completedReport);
            Console.WriteLine("  " + miningHarvester.Name + " завершил добычу! Добыто: " + completedReport.AmountMined + " ед. Echos");
          }
        }
      }
    }

    private void DisplayHarvestersInfo() {
      Console.WriteLine("\n=== Состояние флота ===");
      foreach (HarvesterShip currentHarvester in Fleet) {
        Console.WriteLine(currentHarvester.GetHarvesterInfo());
      }
    }

    private void DisplayTotalMined() {
      Console.WriteLine("\n=== Суммарная добыча ===");
      foreach (HarvesterShip currentHarvester in Fleet) {
        int totalMined = 0;
        foreach (Report currentReport in Worklog[currentHarvester.Name]) {
          totalMined += currentReport.AmountMined;
        }
        Console.WriteLine(currentHarvester.Name + ": " + totalMined + " ед. Echos (астероидов: " + currentHarvester.AsteroidMined + ")");
      }
    }

    private void DisplayFullWorklog() {
      Console.WriteLine("\n" + new string('=', 60));
      Console.WriteLine("ПОЛНЫЙ ЖУРНАЛ РАБОТ (Хрон #" + _chronCounter + ")");
      Console.WriteLine(new string('=', 60));

      foreach (HarvesterShip currentHarvester in Fleet) {
        Console.WriteLine("\n" + currentHarvester.Name + ":");
        if (Worklog[currentHarvester.Name].Count == 0) {
          Console.WriteLine("  Нет завершённых заданий");
        } else {
          foreach (Report currentReport in Worklog[currentHarvester.Name]) {
            Console.WriteLine("  " + currentReport.GetReportString());
          }
          int totalMined = 0;
          foreach (Report currentReport in Worklog[currentHarvester.Name]) {
            totalMined += currentReport.AmountMined;
          }
          Console.WriteLine("  Итого: " + totalMined + " ед. Echos");
        }
      }

      Console.WriteLine(new string('=', 60));
    }

    public int GetTotalMined() {
      int grandTotal = 0;
      foreach (HarvesterShip currentHarvester in Fleet) {
        foreach (Report currentReport in Worklog[currentHarvester.Name]) {
          grandTotal += currentReport.AmountMined;
        }
      }
      return grandTotal;
    }
  }
}