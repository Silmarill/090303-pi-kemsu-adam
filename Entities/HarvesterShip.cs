using System;

public class HarvesterShip : IChroneListener {
  // Основные данные
  public readonly int id;
  public readonly string name;
  public int asteroidsMined;

  // Параметры добычи
  public int cargoCapacity;
  public int biteSize;

  // Состояние
  public int cargoCurrent;
  public HarvesterState state;

  // Внутренняя логика
  private Asteroid currentTarget;
  private int jobCounter;

  // Конструктор
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

  // Обработка хрона
  public void OnChroneTick() {
    if (state == HarvesterState.Mining && currentTarget != null) {
      Mine(currentTarget);
    }
  }

  // Добыча астероида
  public void Mine(Asteroid asteroid) {

    // Проверка валидности цели
    if (asteroid == null || asteroid.State != AsteroidState.Mining || asteroid.CurrentEchos <= 0) {
      FinishMining(false);
      return;
    }

    // Расчет добычи за шаг
    int biteAmount = Math.Min(biteSize, asteroid.CurrentEchos);

    asteroid.CurrentEchos -= biteAmount;
    cargoCurrent += biteAmount;

    // Проверка завершения добычи
    if (asteroid.CurrentEchos <= 0) {
      asteroid.CurrentEchos = 0;
      asteroid.State = AsteroidState.Depleted;

      FinishMining(true);
    }
    else if (cargoCurrent >= cargoCapacity) {
      FinishMining(false);
    }
  }

  // Завершение добычи (освобождение цели, обновление статистики и состояния)
  private void FinishMining(bool isAsteroidDepleted) {
    if (currentTarget != null) {
      if (cargoCurrent > 0) {
        asteroidsMined++;
      }

      currentTarget = null;
    }

    cargoCurrent = 0;
    state = HarvesterState.Idle;
  }

  // Назначение цели
  public bool TryAssignTarget(Asteroid asteroid) {
    if (state != HarvesterState.Idle || asteroid.State != AsteroidState.Idle) {
      return false;
    }

    currentTarget = asteroid;

    asteroid.State = AsteroidState.Mining;
    state = HarvesterState.Mining;

    jobCounter++;

    return true;
  }

  // Вывод информации о харвестере
  public override string ToString() {
    return $"Harvester [{id:D2}] {name} | Status: {state} | Cargo: {cargoCurrent}/{cargoCapacity} | Mined: {asteroidsMined}";
  }
}