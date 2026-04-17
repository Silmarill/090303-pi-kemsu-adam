using System;

public class HarvesterShip : IChronListener {
  private static int _idCounter = 0;
  private int _jobCounter;

  public int id;
  public string name;
  public int asteroidsMined;
  public int cargoCapacity;
  public int cargoCurrent;
  public int biteSize;
  public HarvesterState state;
  public Asteroid? currentAsteroid;
  public MotherShip homeStation;

  public HarvesterShip(string name, int cargoCapacity, int biteSize, MotherShip station) {
    id = ++_idCounter;
    this.name = name;
    this.cargoCapacity = cargoCapacity;
    this.biteSize = biteSize;
    homeStation = station;
    state = HarvesterState.Idle;
    cargoCurrent = 0;
    asteroidsMined = 0;
    _jobCounter = 0;
    currentAsteroid = null;
  }

  public void OnChronTick() {
    if (state != HarvesterState.Mining || currentAsteroid == null) {
      return;
    }

    int bite = biteSize;
    if (bite > currentAsteroid.currentEchos) {
      bite = currentAsteroid.currentEchos;
    }
    currentAsteroid.currentEchos -= bite;
    cargoCurrent += bite;

    if (currentAsteroid.currentEchos == 0) {
      currentAsteroid.state = AsteroidState.Depleted;
    }

    if (currentAsteroid.state == AsteroidState.Depleted || cargoCurrent >= cargoCapacity) {
      homeStation.FinishHarvest(this);
    }
  }

  public void StartMining(Asteroid asteroid) {
    currentAsteroid = asteroid;
    asteroid.state = AsteroidState.Mining;
    state = HarvesterState.Mining;
  }

  public Report CreateReport() {
    ++_jobCounter;
    return new Report(_jobCounter, currentAsteroid!.spawnID, cargoCurrent);
  }

  public void PrintInfo() {
    string targetInfo = currentAsteroid != null
      ? "Asteroid Spawn #" + currentAsteroid.spawnID
      : "none";
    Console.WriteLine(
      "  [" + name + "]" +
      "  State: " + state +
      "  Cargo: " + cargoCurrent + "/" + cargoCapacity +
      "  Mined: " + asteroidsMined +
      "  Target: " + targetInfo
    );
  }
}
