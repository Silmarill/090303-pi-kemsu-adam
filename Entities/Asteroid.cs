using System;

public class Asteroid : IChroneListener {
  // Публичные поля
  public int currentEchos;
  public int maxEchos;
  public AsteroidState state;
  public int spawnId;
  public int createId;

  // Использовал на 1 этапе для фиксированной деградации астероидов. Dead code на момент 2 этапа.
  // private const int echosDecreasePerChron = 100;

  // Глобальный счетчик для генерации уникальных CreateId
  private static int globalCreateIdCounter = 0;
  // Генератор случайных чисел для создания астероидов с разным количеством эхосов
  private static Random random = new Random();

  // Конструктор
  public Asteroid() {
    createId = ++globalCreateIdCounter;

    maxEchos = random.Next(100, 1001);
    currentEchos = maxEchos;

    state = AsteroidState.Idle;
    spawnId = 0;
  }

  // Метод для установки SpawnId при спавне астероида
  public void SetSpawnId(int spawnId) {
    this.spawnId = spawnId;
  }

  // Метод для сброса астероида в начальное состояние
  public void Reset() {
    currentEchos = maxEchos;
    state = AsteroidState.Idle;
  }

  // Метод для обработки события ChroneTick
  public void OnChroneTick() {
    // стабилизационное поле MotherShip — астероиды не деградируют
  }

  // Переопределение метода ToString для удобного отображения информации об астероиде
  public override string ToString() {
    return $"[CreateId: {createId:D2} | SpawnId: {spawnId:D2}] Echos: {currentEchos}/{maxEchos} | State: {state}";
  }
}