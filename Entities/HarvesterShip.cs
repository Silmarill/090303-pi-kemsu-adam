using System;

public class HarvesterShip : IChroneListener {
  // Основные данные харвестера
  public readonly int ID;
  public readonly string Name;
  public int AsteroidsMined;

  // Параметры добычи
  public int CargoCapacity;
  public int BiteSize;

  // Текущее состояние
  public int CargoCurrent;
  public HarvesterState State;

  // Приватные поля для внутренней логики
  private Asteroid _currentTarget;
  private int _jobCounter;

  // Конструктор для инициализации харвестера
  public HarvesterShip(int id, string name) {
    ID = id;
    Name = name;

    AsteroidsMined = 0;
    CargoCurrent = 0;
    State = HarvesterState.Idle;

    // Значения по умолчанию
    CargoCapacity = 500;
    BiteSize = 50;

    _currentTarget = null;
    _jobCounter = 0;
  }

  // Метод, вызываемый при каждом тике хронометра
  public void OnChroneTick() {
    if (State == HarvesterState.Mining && _currentTarget != null) {
      Mine(_currentTarget);
    }
  }

  // Логика добычи астероида
  public void Mine(Asteroid asteroid) {
    if (asteroid == null || asteroid.State != AsteroidState.Mining || asteroid.CurrentEchos <= 0) {
      FinishMining();
      return;
    }

    // Вычисления кол-ва эхов можно добыть за один укус, не превышая текущий запас астероида и вместимость груза
    int bite = Math.Min(BiteSize, asteroid.CurrentEchos);
    asteroid.CurrentEchos -= bite;
    CargoCurrent += bite;

    // Проверка, не закончился ли астероид или не переполнился ли груз
    if (asteroid.CurrentEchos <= 0) {
      asteroid.CurrentEchos = 0;
      asteroid.State = AsteroidState.Depleted;
      FinishMining(true);
    }
    else if (CargoCurrent >= CargoCapacity) {
      FinishMining();
    }
  }

  // Завершение добычи, разгрузка и подготовка к следующей задаче
  private void FinishMining(bool asteroidDepleted = false) {
    if (_currentTarget != null) {
      if (CargoCurrent > 0) {
        AsteroidsMined++;
      }

      // Если астероид был полностью добыт, он уже помечен как Depleted, иначе возвращает его в Idle
      _currentTarget = null;
    }

    // Разгрузка на станции
    CargoCurrent = 0;
    State = HarvesterState.Idle;
  }

  // Метод для назначения цели харвестеру
  public bool TryAssignTarget(Asteroid asteroid) {
    if (State != HarvesterState.Idle || asteroid.State != AsteroidState.Idle) {
      return false;
    }

    _currentTarget = asteroid;
    asteroid.State = AsteroidState.Mining;
    State = HarvesterState.Mining;
    _jobCounter++;
    return true;
  }

  // Переопределение метода ToString для удобного отображения информации о харвестере
  public override string ToString() {
    return $"Harvester [{ID:D2}] {Name} | Status: {State} | Cargo: {CargoCurrent}/{CargoCapacity} | Mined: {AsteroidsMined}";
  }
}