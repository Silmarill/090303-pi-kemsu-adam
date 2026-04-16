using Adam;
using System;

namespace Asteroids {
  public class HarvesterShip : IChronListener {
    private static int _nextId = 1;

    public int ID;
    public string Name;
    public int AsteroidsMined;
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;
    public HarvesterState State;
    public Asteroid CurrentAsteroid;
    public MotherShip HomeStation;
    public int JobCounter;

    public HarvesterShip(string name, int cargoCapacity, int biteSize, MotherShip station) {
      ID = _nextId;
      _nextId = _nextId + 1;
      Name = name;
      CargoCapacity = cargoCapacity;
      BiteSize = biteSize;
      State = HarvesterState.Idle;
      HomeStation = station;
      CargoCurrent = 0;
      AsteroidsMined = 0;
      JobCounter = 0;
      CurrentAsteroid = null;
    }

    public void OnChronTick() {
      if (State == HarvesterState.Mining && CurrentAsteroid != null) {
        int biteAmount = BiteSize;

        if (biteAmount > CurrentAsteroid.CurrentEchos) {
          biteAmount = CurrentAsteroid.CurrentEchos;
        }

        if (biteAmount > CargoCapacity - CargoCurrent) {
          biteAmount = CargoCapacity - CargoCurrent;
        }

        if (biteAmount > 0) {
          CurrentAsteroid.CurrentEchos -= biteAmount;
          CargoCurrent += biteAmount;
        }

        if (CurrentAsteroid.CurrentEchos <= 0) {
          CurrentAsteroid.State = AsteroidState.Depleted;
          HomeStation.FinishHarvest(this);
        } else if (CargoCurrent >= CargoCapacity) {
          HomeStation.FinishHarvest(this);
        }
      }
    }

    public void StartMining(Asteroid asteroid) {
      if (State != HarvesterState.Idle) return;
      if (asteroid.State != AsteroidState.Idle) return;

      State = HarvesterState.Mining;
      CurrentAsteroid = asteroid;
      asteroid.State = AsteroidState.Mining;
    }

    public Report CreateReport() {
      JobCounter = JobCounter + 1;
      return new Report(JobCounter, CurrentAsteroid.SpawnID, CargoCurrent);
    }

    public void CompleteMission() {
      if (CurrentAsteroid != null) {
        if (CurrentAsteroid.State == AsteroidState.Depleted) {
          AsteroidsMined = AsteroidsMined + 1;
        } else {
          CurrentAsteroid.State = AsteroidState.Idle;
        }
      }
      CurrentAsteroid = null;
      CargoCurrent = 0;
      State = HarvesterState.Idle;
    }

    public bool IsIdle() {
      return State == HarvesterState.Idle;
    }

    public void PrintInfo() {
      if (State == HarvesterState.Idle) {
        Console.WriteLine($"{Name} (ID:{ID}) Idle, Cargo: {CargoCurrent}/{CargoCapacity}, Mined: {AsteroidsMined}");
      } else {
        Console.WriteLine($"{Name} (ID:{ID}) Mining asteroid {CurrentAsteroid.SpawnID}, Cargo: {CargoCurrent}/{CargoCapacity}, Mined: {AsteroidsMined}");
      }
    }
  }
}