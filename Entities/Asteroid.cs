using System;

public class Asteroid : IChroneListener {
  // Публичные поля
  public int CurrentEchos;
  public int MaxEchos;
  public AsteroidState State;
  public int SpawnID;
  public int CreateID;

  // Величина уменьшения ресурса за один хрон
  private const int ECHOS_DECREASE_PER_CHRON = 100;

  private static int _globalCreateIdCounter = 0;
  private static Random _random = new Random();

  // Конструктор, который инициализирует астероид с уникальным CreateID, случайным количеством Echos и начальным состоянием Idle
  public Asteroid() {
    CreateID = ++_globalCreateIdCounter;

    MaxEchos = _random.Next(100, 1001);
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
    SpawnID = 0;
  }

  // Метод для установки SpawnID, который может быть вызван эмиттером при спавне астероида
  public void SetSpawnID(int spawnId) {
    SpawnID = spawnId;
  }

  // Метод для сброса астероида в начальное состояние, который может быть вызван эмиттером при ресете астероида
  public void Reset() {
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  // Метод, который вызывается хроном каждый тик, уменьшая количество Echos, если астероид в состоянии Idle
  public void OnChroneTick() {
    // Только Idle теряет ресурс
    if (State == AsteroidState.Idle) {
      CurrentEchos -= ECHOS_DECREASE_PER_CHRON;
      if (CurrentEchos < 0) {
        CurrentEchos = 0;
      }
      if (CurrentEchos == 0) {
        State = AsteroidState.Depleted;
      }
    }
  }

  // Переопределение метода ToString для удобного отображения информации об астероиде
  public override string ToString() {
    return $"[CreateID: {CreateID:D2} | SpawnID: {SpawnID:D2}] Echos: {CurrentEchos}/{MaxEchos} | State: {State}";
  }
}