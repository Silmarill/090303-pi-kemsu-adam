using System;

public class HarvesterShip : IChroneListener {
  public int ID;
  public string Name;
  public int AsteroidsMined;
  public int CargoCapacity;
  public int CargoCurrent;
  public int BiteSize;
  public HarvesterState State;
  public Asteroid CurrentAsteroid;
  public MotherShip HomeStation;
  public static int nextId;

  public HarvesterShip(string name, int cargoCapacity, int biteSize, MotherShip station) {
    ID = ++nextId;
    Name = name;
    CargoCapacity = cargoCapacity;
    BiteSize = biteSize;
    State = HarvesterState.Idle;
    HomeStation = station;
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

      CurrentAsteroid.CurrentEchos -= biteAmount;
      CargoCurrent += biteAmount;

      if (CurrentAsteroid.CurrentEchos <= 0) {
        CurrentAsteroid.State = AsteroidState.Depleted;
        HomeStation.FinishHarvest(this);
      } else if (CargoCurrent >= CargoCapacity) {
        HomeStation.FinishHarvest(this);
      }
    }
  }
  public void StartMining(Asteroid asteroid) {
    State = HarvesterState.Mining;
    CurrentAsteroid = asteroid;
    asteroid.State = AsteroidState.Mining;
  }


}