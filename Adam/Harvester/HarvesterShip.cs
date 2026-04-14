using AsteroidSimulation.Models;
using System;

namespace Adam.Models {
  public class HarvesterShip {
    public int id;
    public string name;
    public int asteroidsMined;
    public double cargoCapacity;
    public double cargoCurrent;
    public double biteSize;
    public HarvesterState state;
    public Asteroid currentAsteroid;
    public int jobsCompleted;

    private static int nextId;

    public HarvesterShip(string shipName, double capacity, double bite) {
      id = nextId;
      ++nextId;
      name = shipName;
      cargoCapacity = capacity;
      biteSize = bite;
      cargoCurrent = 0;
      asteroidsMined = 0;
      jobsCompleted = 0;
      state = HarvesterState.Idle;
      currentAsteroid = null;
    }

    public bool TryStartMining(Asteroid asteroid) {
      bool started;

      if (state != HarvesterState.Idle) {
        return false;
      }

      if (asteroid == null) {
        return false;
      }

      if (asteroid.state != AsteroidState.Idle) {
        return false;
      }

      if (asteroid.isBeingMined == true) {
        return false;
      }

      started = asteroid.TryStartMining();
      if (started == true) {
        state = HarvesterState.Mining;
        currentAsteroid = asteroid;
        return true;
      }

      return false;
    }

    public double MineCurrentAsteroid() {
      double minedAmount;

      if (state != HarvesterState.Mining) {
        return 0;
      }

      if (currentAsteroid == null) {
        return 0;
      }

      minedAmount = currentAsteroid.Mine(biteSize);
      cargoCurrent = cargoCurrent + minedAmount;

      if (currentAsteroid.state == AsteroidState.Depleted) {
        currentAsteroid.ReleaseFromMining();
        currentAsteroid.Recycle();
        currentAsteroid = null;
        state = HarvesterState.Idle;
        asteroidsMined = asteroidsMined + 1;
        return minedAmount;
      }

      if (cargoCurrent >= cargoCapacity) {
        currentAsteroid.ReleaseFromMining();
        currentAsteroid = null;
        state = HarvesterState.Idle;
        asteroidsMined = asteroidsMined + 1;
        return minedAmount;
      }

      return minedAmount;
    }

    public Report UnloadAndCreateReport(int jobNumber, int asteroidSpawnId) {
      double amountToReport;
      Report newReport;

      amountToReport = cargoCurrent;
      cargoCurrent = 0;
      jobsCompleted = jobsCompleted + 1;

      newReport = new Report(jobNumber, asteroidSpawnId, amountToReport, name);
      return newReport;
    }

    public void ResetForNewJob() {
      cargoCurrent = 0;
      state = HarvesterState.Idle;
      currentAsteroid = null;
    }

    public void PrintInfo() {
      string status;

      if (state == HarvesterState.Idle) {
        status = "Idle";
      } else {
        status = "Mining (Asteroid #" + currentAsteroid.spawnId + ")";
      }

      Console.WriteLine("Harvester #" + id + " | " + name + " | " + status + " | Cargo: " + cargoCurrent + "/" + cargoCapacity + " | Asteroids Mined: " + asteroidsMined + " | Jobs: " + jobsCompleted);
    }
  }
}