using AsteroidZoneSimulation.Core;
using System;

namespace AsteroidZoneSimulation.Models {
  public class HarvesterShip : IChroneListener {
    private int _nextID = 1;
    private int _jobCounter = 0;

    public int ID;
    public string Name;
    public int AsteroidsMined;
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;
    public HarvesterState State;
    public Asteroid CurrentAsteroid;
    public MotherShip HomeStation;

    public HarvesterShip(string name, int cargoCapacity, int biteSize, MotherShip station) {
      ID = ++_nextID;
      Name = name;
      CargoCapacity = cargoCapacity;
      CargoCurrent = 0;
      BiteSize = biteSize;
      AsteroidsMined = 0;
      State = HarvesterState.Idle;
      CurrentAsteroid = null;
      HomeStation = station;

      ChroneManager.AddListener(this);
    }

    public void OnChroneTick() {
      bool asteroidDepleted, cargoFull;
            
      if (State == HarvesterState.Mining && CurrentAsteroid != null) {
        int mined = CurrentAsteroid.ReduceEchos(BiteSize);
        CargoCurrent += mined;

        asteroidDepleted = CurrentAsteroid.State == AsteroidState.Depleted;
        cargoFull = CargoCurrent >= CargoCapacity;

        if (asteroidDepleted || cargoFull) {
          HomeStation.FinishHarvest(this);
        }
      }
    }

    public void StartMining(Asteroid asteroid) {
      CurrentAsteroid = asteroid;
      CurrentAsteroid.State = AsteroidState.Mining;
      State = HarvesterState.Mining;
    }

    public Report CreateReport() {
      ++_jobCounter;
      return new Report(_jobCounter, CurrentAsteroid.SpawnID, CargoCurrent);
    }

    public void Unload() {
      if (CargoCurrent > 0) {
        ++AsteroidsMined;
      }

      CargoCurrent = 0;
    }

    public void ResetAfterHarvest() {
      if (CurrentAsteroid != null && CurrentAsteroid.State == AsteroidState.Mining) {
        if (CurrentAsteroid.State != AsteroidState.Depleted) {
          CurrentAsteroid.State = AsteroidState.Idle;
        }
      }

      CurrentAsteroid = null;
      State = HarvesterState.Idle;
    }

    public void PrintInfo() {
      string cargoInfo, asteroidInfo;

      if (State == HarvesterState.Mining) {
        cargoInfo = $"Груз:{CargoCurrent}/{CargoCapacity}";
      } else {
        cargoInfo = "Груз: пуст";
      }

      if (State == HarvesterState.Mining && CurrentAsteroid != null) {
        asteroidInfo = $" | Добывает астероид (SpawnID): {CurrentAsteroid.SpawnID}";
      } else {
        asteroidInfo = "";
      }

      Console.WriteLine($"  {Name} (ID: {ID}) | {State} | {cargoInfo} | Добыто астероидов: {AsteroidsMined}{asteroidInfo}");
    }
  }
}
