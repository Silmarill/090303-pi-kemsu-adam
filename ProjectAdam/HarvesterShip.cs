using ProjectAdam;
using System;

namespace AsteroidSimulator.Models {
  public class HarvesterShip {
    public static int NextId = 0;
    private const int DefaultCargoCapacity = 500;
    private const int DefaultBiteSize = 50;

    public int Id;
    public string Name;
    public int AsteroidsMined;
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;
    public HarvesterState State;
    public Asteroid CurrentAsteroid;

    public HarvesterShip(string shipName) {
      this.Id = NextId + 1;
      NextId = this.Id;
      this.Name = shipName;
      this.CargoCapacity = DefaultCargoCapacity;
      this.BiteSize = DefaultBiteSize;
      this.CargoCurrent = 0;
      this.AsteroidsMined = 0;
      this.State = HarvesterState.Idle;
      this.CurrentAsteroid = null;
    }

    public bool IsIdle() {
      return this.State == HarvesterState.Idle;
    }

    public bool AssignAsteroid(Asteroid target) {
      if (this.State != HarvesterState.Idle) return false;
      if (target == null) return false;
      if (target.State != AsteroidState.Idle) return false;
      if (target.IsBeingMined) return false;

      this.CurrentAsteroid = target;
      this.CurrentAsteroid.StartMining();
      this.State = HarvesterState.Mining;
      return true;
    }

    public void MineTick() {
      if (this.State != HarvesterState.Mining) return;
      if (this.CurrentAsteroid == null) {
        this.FinishMining();
        return;
      }

      if (this.CurrentAsteroid.State == AsteroidState.Depleted) {
        this.FinishMining();
        return;
      }

      int mined = this.CurrentAsteroid.Mine(this.BiteSize);
      this.CargoCurrent += mined;

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
      Report newReport = new Report();
      newReport.JobNumber = jobNumber;
      newReport.AsteroidSpawnID = this.CurrentAsteroid != null ? this.CurrentAsteroid.SpawnID : 0;
      newReport.AmountMined = this.CargoCurrent;

      this.AsteroidsMined++;
      this.CargoCurrent = 0;

      return newReport;
    }

    public override string ToString() {
      return $"{this.Name} (ID:{this.Id}) | {this.State} | Cargo:{this.CargoCurrent}/{this.CargoCapacity} | Asteroids:{this.AsteroidsMined}";
    }
  }
}