using System;

namespace AsteroidSimulator.Models {
  public class HarvesterShip {
    public int Id { get; }
    public string Name { get; }
    public int AsteroidsMined { get; private set; }
    public int CargoCapacity { get; }
    public int CargoCurrent { get; private set; }
    public int BiteSize { get; }
    public HarvesterState State { get; private set; }
    public Asteroid AssignedAsteroid { get; private set; }

    private MotherShip _home;

    public HarvesterShip(int id, string name, int cargoCapacity, int biteSize) {
      Id = id;
      Name = name;
      CargoCapacity = cargoCapacity;
      BiteSize = biteSize;
      CargoCurrent = 0;
      AsteroidsMined = 0;
      State = HarvesterState.Idle;
      AssignedAsteroid = null;
      _home = null;
    }

    internal void BindMotherShip(MotherShip home) {
      _home = home;
    }

    internal void RegisterAsteroidFullyProcessed() {
      AsteroidsMined = AsteroidsMined + 1;
    }

    public void AssignToAsteroid(Asteroid asteroid) {
      if (State != HarvesterState.Idle) {
        return;
      }

      if (asteroid == null || asteroid.State != AsteroidState.Idle) {
        return;
      }

      AssignedAsteroid = asteroid;
      asteroid.StartMining();
      State = HarvesterState.Mining;
    }

    /// <summary>Добыча по методичке: явная ссылка на астероид.</summary>
    public int Mine(Asteroid asteroid) {
      int zero;
      int mined;

      zero = 0;

      if (_home == null) {
        return zero;
      }

      if (State != HarvesterState.Mining || asteroid == null || AssignedAsteroid != asteroid) {
        return zero;
      }

      mined = asteroid.Mine(BiteSize);
      CargoCurrent = CargoCurrent + mined;

      if (CargoCurrent >= CargoCapacity || asteroid.State == AsteroidState.Depleted) {
        CompleteUnloadAtStation();
      }

      return mined;
    }

    private void CompleteUnloadAtStation() {
      int haul;
      int spawnId;
      bool asteroidFullyConsumed;

      haul = CargoCurrent;
      spawnId = 0;
      asteroidFullyConsumed = false;

      if (AssignedAsteroid != null) {
        spawnId = AssignedAsteroid.SpawnId;
        asteroidFullyConsumed = AssignedAsteroid.State == AsteroidState.Depleted;
        AssignedAsteroid.StopMining();
        AssignedAsteroid = null;
      }

      if (haul > 0) {
        _home.AppendHarvesterJobReport(this, spawnId, haul, asteroidFullyConsumed);
      }

      CargoCurrent = 0;
      State = HarvesterState.Idle;
    }

    public void ForcedRetreatFromLostAsteroid(MotherShip station, Asteroid asteroid) {
      int haul;
      bool fullyConsumed;

      if (AssignedAsteroid != asteroid) {
        return;
      }

      haul = CargoCurrent;
      fullyConsumed = asteroid.State == AsteroidState.Depleted;

      if (AssignedAsteroid != null) {
        AssignedAsteroid.StopMining();
        AssignedAsteroid = null;
      }

      if (haul > 0) {
        station.AppendHarvesterJobReport(this, asteroid.SpawnId, haul, fullyConsumed);
      }

      CargoCurrent = 0;
      State = HarvesterState.Idle;
    }

    public override string ToString() {
      string target;
      if (AssignedAsteroid == null) {
        target = "none";
      } else {
        target = "#" + AssignedAsteroid.SpawnId;
      }

      return "ID " + Id + " " + Name + " | " + State + " | cargo " + CargoCurrent + "/" + CargoCapacity + " | asteroids mined " + AsteroidsMined + " | target " + target;
    }
  }
}
