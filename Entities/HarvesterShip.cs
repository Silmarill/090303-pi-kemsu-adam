using System;

public class HarvesterShip : IChroneListener {
  public readonly int id;
  public readonly string name;
  public int asteroidsMined;

  public int cargoCapacity;
  public int biteSize;

  public int cargoCurrent;
  public HarvesterState state;

  private Asteroid currentTarget;
  private int jobCounter;

  // ВАЖНО: ссылка на станцию
  private MotherShip motherShip;

  public HarvesterShip(int id, string name) {
    this.id = id;
    this.name = name;

    asteroidsMined = 0;
    cargoCurrent = 0;
    state = HarvesterState.Idle;

    cargoCapacity = 500;
    biteSize = 50;

    currentTarget = null;
    jobCounter = 0;
  }

  // подключение станции
  public void SetMotherShip(MotherShip ship) {
    motherShip = ship;
  }

  public void OnChroneTick() {
    if (state == HarvesterState.Mining && currentTarget != null) {
      Mine(currentTarget);
    }
  }

  public void Mine(Asteroid asteroid) {
    if (asteroid == null || asteroid.state != AsteroidState.Mining || asteroid.currentEchos <= 0) {
      FinishMining();
      return;
    }

    int biteAmount = Math.Min(biteSize, asteroid.currentEchos);

    asteroid.currentEchos -= biteAmount;
    cargoCurrent += biteAmount;

    if (asteroid.currentEchos <= 0) {
      asteroid.currentEchos = 0;
      asteroid.state = AsteroidState.Depleted;
      FinishMining();
    } else if (cargoCurrent >= cargoCapacity) {
      FinishMining();
    }
  }

  private void FinishMining() {
    if (currentTarget != null) {
      if (cargoCurrent > 0) {
        asteroidsMined++;

        // ВАЖНО: создаём отчёт
        motherShip?.DeliverReport(currentTarget, currentTarget.spawnId, cargoCurrent);
      }

      currentTarget = null;
    }

    cargoCurrent = 0;
    state = HarvesterState.Idle;
  }

  public bool TryAssignTarget(Asteroid asteroid) {
    if (state != HarvesterState.Idle || asteroid.state != AsteroidState.Idle) {
      return false;
    }

    currentTarget = asteroid;

    asteroid.state = AsteroidState.Mining;
    state = HarvesterState.Mining;

    jobCounter++;

    return true;
  }

  public override string ToString() {
    return $"Harvester [{id:D2}] {name} | Status: {state} | Cargo: {cargoCurrent}/{cargoCapacity} | Mined: {asteroidsMined}";
  }
}