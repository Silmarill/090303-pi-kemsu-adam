using System;
using System.Collections.Generic;

public class AsteroidEmitter {
  // Пул доступных астероидов
  private Queue<Asteroid> _available;

  // Счетчик для генерации уникальных номеров спавна
  private int _spawnCounter;

  // Инициализация пула астероидов
  public AsteroidEmitter(int initialSize) {
    _available = new Queue<Asteroid>();
    _spawnCounter = 0;

    int poolIndex;

    for (poolIndex = 0; poolIndex < initialSize; ++poolIndex) {
      Asteroid asteroid = new Asteroid();
      _available.Enqueue(asteroid);
    }
  }

  // Метод для получения астероида из пула
  public Asteroid Spawn() {
    Asteroid asteroid;

    /*
     * Если пул пуст, создаем новый астероид. В реальной игре можно было бы ограничить максимальное количество астероидов и не создавать новые,
     * а просто не спавнить, но для демонстрации работы пула я решил позволить создавать новые экземпляры, если пул закончится.
    */
    if (_available.Count == 0) {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(">> Warning: Asteroid pool is empty. Creating new instance!");
      Console.ResetColor();

      asteroid = new Asteroid();
    } else {
      asteroid = _available.Dequeue();
    }

    asteroid.SetSpawnId(++_spawnCounter);

    ChroneManager.AddListener(asteroid);

    return asteroid;
  }

  // Возврат астероида в пул
  public void Recycle(Asteroid asteroid) {
    ChroneManager.RemoveListener(asteroid);

    asteroid.Reset();

    _available.Enqueue(asteroid);
  }
}