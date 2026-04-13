using System;

public class Asteroid : IChroneListener {
  // Публичные поля
  public int currentEchos;
  public int maxEchos;
  public AsteroidState state;
  public int spawnId;
  public int createId;

  // Внутренние параметры
  private int echosDecreasePerChron;

  private static int globalCreateIdCounter;
  private static Random random;

  // Конструктор
  public Asteroid() {
    // Инициализация статических полей (при первом создании)
    if (random == null) {
      random = new Random();
      globalCreateIdCounter = 0;
    }

    createId = ++globalCreateIdCounter;

    maxEchos = random.Next(100, 1001);
    currentEchos = maxEchos;

    state = AsteroidState.Idle;
    spawnId = 0;

    echosDecreasePerChron = 100;
  }

  // Установка SpawnId
  public void SetSpawnId(int spawnId) {
    this.spawnId = spawnId;
  }

  // Сброс состояния
  public void Reset() {
    currentEchos = maxEchos;
    state = AsteroidState.Idle;
  }

  // Обработка хрона
  public void OnChroneTick() {
    if (state == AsteroidState.Idle) {
      currentEchos -= echosDecreasePerChron;

      if (currentEchos < 0) {
        currentEchos = 0;
      }

      if (currentEchos == 0) {
        state = AsteroidState.Depleted;
      }
    }
  }

  // Вывод
  public override string ToString() {
    return $"[CreateId: {createId:D2} | SpawnId: {spawnId:D2}] Echos: {currentEchos}/{maxEchos} | State: {state}";
  }
}