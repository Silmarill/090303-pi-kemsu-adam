using System;
using System.Collections.Generic;

namespace AsteroidSimulator.Models {
  public class MotherShip {
    public List<HarvesterShip> Fleet;
    public Dictionary<string, List<Report>> Worklog;

    private Random randomGenerator;

    public MotherShip(int harvesterCount) {
      int shipIndex;
      int stepOne;
      int zero;
      string[] names;

      stepOne = 1;
      zero = 0;
      names = new string[] { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };

      this.Fleet = new List<HarvesterShip>();
      this.Worklog = new Dictionary<string, List<Report>>();
      this.randomGenerator = new Random();

      for (shipIndex = zero; shipIndex < harvesterCount; shipIndex = shipIndex + stepOne) {
        HarvesterShip newShip;

        newShip = new HarvesterShip(names[shipIndex]);
        this.Fleet.Add(newShip);
        this.Worklog.Add(newShip.Name, new List<Report>());
      }
    }

    public void OnChronTick(List<Asteroid> activeAsteroids) {
      int shipIndex;
      int asteroidIndex;
      int stepOne;
      int zero;

      stepOne = 1;
      zero = 0;

      for (shipIndex = zero; shipIndex < this.Fleet.Count; shipIndex = shipIndex + stepOne) {
        HarvesterShip currentShip;

        currentShip = this.Fleet[shipIndex];

        if (currentShip.State == HarvesterState.Mining) {
          currentShip.MineTick();
        }
      }

      for (shipIndex = zero; shipIndex < this.Fleet.Count; shipIndex = shipIndex + stepOne) {
        HarvesterShip currentShip;

        currentShip = this.Fleet[shipIndex];

        if (currentShip.State == HarvesterState.Idle) {
          if (currentShip.CargoCurrent > zero) {
            int jobNumber;
            Report unloadReport;

            jobNumber = this.Worklog[currentShip.Name].Count + stepOne;
            unloadReport = currentShip.Unload(jobNumber);
            this.Worklog[currentShip.Name].Add(unloadReport);
          }
        }
      }

      for (shipIndex = zero; shipIndex < this.Fleet.Count; shipIndex = shipIndex + stepOne) {
        HarvesterShip currentShip;
        List<Asteroid> freeAsteroids;

        currentShip = this.Fleet[shipIndex];
        freeAsteroids = new List<Asteroid>();

        if (currentShip.IsIdle()) {
          for (asteroidIndex = zero; asteroidIndex < activeAsteroids.Count; asteroidIndex = asteroidIndex + stepOne) {
            Asteroid currentAsteroid;

            currentAsteroid = activeAsteroids[asteroidIndex];

            if (currentAsteroid.State == AsteroidState.Idle) {
              freeAsteroids.Add(currentAsteroid);
            }
          }

          if (freeAsteroids.Count > zero) {
            int targetIndex;
            Asteroid target;

            targetIndex = this.randomGenerator.Next(zero, freeAsteroids.Count);
            target = freeAsteroids[targetIndex];
            currentShip.AssignAsteroid(target);
          }
        }
      }
    }

    public Dictionary<string, int> GetTotalMined() {
      int shipIndex;
      int stepOne;
      int zero;
      Dictionary<string, int> result;

      stepOne = 1;
      zero = 0;
      result = new Dictionary<string, int>();

      for (shipIndex = zero; shipIndex < this.Fleet.Count; shipIndex = shipIndex + stepOne) {
        HarvesterShip currentShip;
        int total;
        int reportIndex;

        currentShip = this.Fleet[shipIndex];
        total = zero;

        for (reportIndex = zero; reportIndex < this.Worklog[currentShip.Name].Count; reportIndex = reportIndex + stepOne) {
          Report currentReport;

          currentReport = this.Worklog[currentShip.Name][reportIndex];
          total = total + currentReport.AmountMined;
        }

        result.Add(currentShip.Name, total);
      }

      return result;
    }

    public void PrintWorklog() {
      int shipIndex;
      int stepOne;
      int zero;

      stepOne = 1;
      zero = 0;

      Console.WriteLine("\n=== FULL WORKLOG ===");

      for (shipIndex = zero; shipIndex < this.Fleet.Count; shipIndex = shipIndex + stepOne) {
        HarvesterShip currentShip;
        int reportIndex;

        currentShip = this.Fleet[shipIndex];

        Console.WriteLine("\n" + currentShip.Name + " (ID:" + currentShip.Id + "):");

        if (this.Worklog[currentShip.Name].Count == zero) {
          Console.WriteLine("  No jobs yet");
        } else {
          for (reportIndex = zero; reportIndex < this.Worklog[currentShip.Name].Count; reportIndex = reportIndex + stepOne) {
            Report currentReport;

            currentReport = this.Worklog[currentShip.Name][reportIndex];
            Console.WriteLine("  " + currentReport.ToString());
          }
        }
      }

      Console.WriteLine("===================\n");
    }
  }
}