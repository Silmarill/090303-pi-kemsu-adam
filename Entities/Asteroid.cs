using System;

public class Asteroid : IChroneListener {
  // Статический счетчик для генерации уникальных CreateID
  private static int _globalCreateIdCounter = 0;
  // Статический объект Random для генерации случайного количества эхов
  private static Random _random = new Random();

  public int CurrentEchos { get; private set; }
  public int MaxEchos { get; private set; }
  public AsteroidState State { get; private set; }
  public int SpawnID { get; private set; }
  public int CreateID { get; private set; }

  // Конструктор для создания астероида с рандомным количеством эхов
  public Asteroid() {
    CreateID = ++_globalCreateIdCounter;

    // От 100 до 1000 включительно
    MaxEchos = _random.Next(100, 1001);

    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  // Метод для пула, чтобы задавать сквозной номер спавна
  public void SetSpawnID(int spawnId) {
    SpawnID = spawnId;
  }

  // Метод для сброса астероида в начальное состояние
  public void Reset() {
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  // Вызывается при каждом тике хронометра
  public void OnChroneTick() {
    if (State == AsteroidState.Idle) {
      CurrentEchos -= 100;
      if (CurrentEchos <= 0) {
        CurrentEchos = 0;
        State = AsteroidState.Depleted;
      }
    }
  }

  // Переопределение для удобного вывода
  public override string ToString() {
    return $"[CreateID: {CreateID:D2} | SpawnID: {SpawnID:D2}] Echos: {CurrentEchos,4}/{MaxEchos,4} | State: {State}";
  }
}