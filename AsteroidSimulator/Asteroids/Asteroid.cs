using System;
using AsteroidSimulator.Core;

namespace AsteroidSimulator.Asteroids;

public class Asteroid : IChronListener {
  private static int _nextCreateId = 1;
  private static int _nextSpawnId = 1;
  private static Random _random = new Random();

  public int currentEchos;    // Текущее количество ресурса Echos
  public int maxEchos;        // Максимальное количество ресурса
  public AsteroidState state;
  public int spawnId;
  public int createId;

  private const int MinEchos = 100;
  private const int MaxEchosValue = 1000;

  public Asteroid() {
    createId = _nextCreateId++;
    maxEchos = _random.Next(MinEchos, MaxEchosValue + 1);
    currentEchos = maxEchos;
    state = AsteroidState.Idle;
    spawnId = 0;
  }

  public void Reset() {
    currentEchos = maxEchos;
    state = AsteroidState.Idle;
  }

  public void OnChronTick() {
    // Деградация отключена - стабилизация зоны MotherShip
  }

  public void SetSpawnId() {
    spawnId = _nextSpawnId++;
  }

  public int Mine(int biteSize) {
    if (state != AsteroidState.Idle && state != AsteroidState.Mining) {
      return 0;
    }

    int amountToMine = biteSize;
    if (amountToMine > currentEchos) {
      amountToMine = currentEchos;
    }

    currentEchos -= amountToMine;

    if (currentEchos == 0) {
      state = AsteroidState.Depleted;
    } else {
      state = AsteroidState.Mining;
    }

    return amountToMine;
  }

  public void SetIdle() {
    if (state == AsteroidState.Mining) {
      state = AsteroidState.Idle;
    }
  }
}