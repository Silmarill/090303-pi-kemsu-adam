
using System;

namespace AsteroidSimulator.Models {
  public class HarvesterShip {
    public static int NextId;

    public int Id;
    public string Name;
    public int AsteroidsMined;
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;
    public HarvesterState State;
    public Asteroid CurrentAsteroid;

    public HarvesterShip(string shipName) {
      int stepOne;
      int defaultCargo;
      int defaultBite;

      stepOne = 1;
      defaultCargo = 500;
      defaultBite = 50;

      this.Id = NextId + stepOne;
      NextId = this.Id;
      this.Name = shipName;
      this.CargoCapacity = defaultCargo;
      this.BiteSize = defaultBite;
      this.CargoCurrent = 0;
      this.AsteroidsMined = 0;
      this.State = HarvesterState.Idle;
      this.CurrentAsteroid = null;
    }

    public bool IsIdle() {
      return this.State == HarvesterState.Idle;
    }

    public bool AssignAsteroid(Asteroid target) {
      if (this.State != HarvesterState.Idle) {
        return false;
      }

      if (target == null) {
        return false;
      }

      if (target.State != AsteroidState.Idle) {
        return false;
      }

      this.CurrentAsteroid = target;
      this.CurrentAsteroid.StartMining();
      this.State = HarvesterState.Mining;
      return true;
    }

    public void MineTick() {
      int mined;

      if (this.State != HarvesterState.Mining) {
        return;
      }

      if (this.CurrentAsteroid == null) {
        this.FinishMining();
        return;
      }

      if (this.CurrentAsteroid.State == AsteroidState.Depleted) {
        this.FinishMining();
        return;
      }

      mined = this.CurrentAsteroid.Mine(this.BiteSize);
      this.CargoCurrent = this.CargoCurrent + mined;

      if (this.CurrentAsteroid.State == AsteroidState.Depleted) {
        this.FinishMining();
        return;
      }

      if (this.CargoCurrent >= this.CargoCapacity) {
        this.FinishMining();
        return;
      }
    }

    private void FinishMining() {
      if (this.CurrentAsteroid != null) {
        this.CurrentAsteroid.StopMining();
        this.CurrentAsteroid = null;
      }

      this.State = HarvesterState.Idle;
    }

    public Report Unload(int jobNumber) {
      Report newReport;
      int stepOne;
      int asteroidSpawnIdentifier;
      int zero;

      stepOne = 1;
      zero = 0;

      if (this.CurrentAsteroid != null) {
        asteroidSpawnIdentifier = this.CurrentAsteroid.SpawnId;
      } else {
        asteroidSpawnIdentifier = zero;
      }

      newReport = new Report();
      newReport.JobNumber = jobNumber;
      newReport.AsteroidSpawnID = asteroidSpawnIdentifier;
      newReport.AmountMined = this.CargoCurrent;

      this.AsteroidsMined = this.AsteroidsMined + stepOne;
      this.CargoCurrent = zero;

      return newReport;
    }

    public override string ToString() {
      string result;

      result = this.Name + " (ID:" + this.Id + ") | " + this.State + " | Cargo:" + this.CargoCurrent + "/" + this.CargoCapacity + " | Mined:" + this.AsteroidsMined;
      return result;
    }
  }
}