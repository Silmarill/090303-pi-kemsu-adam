using System;
using AsteroidSimulator.Asteroids;
using AsteroidSimulator.Reports;

namespace AsteroidSimulator.Ships;

public class HarvesterShip {
  private static int _nextId = 1;
  private static Random _random = new Random();

  public int id;
  public string name;
  public int asteroidsMined;
  public int cargoCapacity;
  public int cargoCurrent;
  public int biteSize;
  public HarvesterState state;
  public Asteroid currentAsteroid;
  public int currentJobNumber;

  private const int DefaultCargoCapacity = 500;
  private const int DefaultBiteSize = 50;

  public HarvesterShip(string namePrefix) {
    id = _nextId++;
    name = namePrefix + "-" + id.ToString();
    asteroidsMined = 0;
    cargoCapacity = DefaultCargoCapacity;
    cargoCurrent = 0;
    biteSize = DefaultBiteSize;
    state = HarvesterState.Idle;
    currentAsteroid = null;
    currentJobNumber = 0;
  }

  public bool StartMining(Asteroid asteroid) {
    if (state != HarvesterState.Idle) {
      return false;
    }

    if (asteroid.state != AsteroidState.Idle) {
      return false;
    }

    currentAsteroid = asteroid;
    state = HarvesterState.Mining;
    currentJobNumber = currentJobNumber + 1;
    return true;
  }

  public bool Mine() {
    if (state != HarvesterState.Mining) {
      return false;
    }

    if (currentAsteroid == null) {
      state = HarvesterState.Idle;
      return false;
    }

    int mined = currentAsteroid.Mine(biteSize);
    cargoCurrent = cargoCurrent + mined;

    if (currentAsteroid.state == AsteroidState.Depleted) {
      return false;
    }

    if (cargoCurrent >= cargoCapacity) {
      return false;
    }

    return true;
  }

  public void Unload() {
    if (currentAsteroid != null) {
      currentAsteroid.SetIdle();
      currentAsteroid = null;
    }

    cargoCurrent = 0;
    state = HarvesterState.Idle;
  }

  public Report CreateReport(int asteroidSpawnId, int amountMined) {
    Report report = new Report(currentJobNumber, asteroidSpawnId, amountMined);
    return report;
  }

  public void IncrementAsteroidsMined() {
    asteroidsMined = asteroidsMined + 1;
  }
}