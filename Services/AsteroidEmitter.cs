using System;
using System.Collections.Generic;

public class AsteroidEmitter {
  private Queue<Asteroid> _available = new Queue<Asteroid>();

  // Счетчик для генерации уникальных номеров спавна
  private int _spawnCounter = 0;

  // Инициализация пула астероидов
  public AsteroidEmitter(int initialSize) {
    int poolIndex;

    // Заполнение пула начальными астероидами
    for (poolIndex = 0; poolIndex < initialSize; ++poolIndex) {
      Asteroid newAsteroid;

      newAsteroid = new Asteroid();
      _available.Enqueue(newAsteroid);
    }
  }

  // Метод для получения астероида из пула
  public Asteroid Spawn() {
    Asteroid asteroid;
    bool isPoolEmpty;

    isPoolEmpty = (_available.Count == 0);

    // Если пул пуст, создаётся новый астероид и выдаем предупреждение
    if (isPoolEmpty) {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(">> Предупреждение: Пул пуст. Создан новый объект Asteroid!");
      Console.ResetColor();

      asteroid = new Asteroid();
    }
    else {
      asteroid = _available.Dequeue();
    }

    // Присвоение уникального номера спавна
    asteroid.SetSpawnId(++_spawnCounter);

    // Подписка на уведомления о тактах времени
    ChroneManager.AddListener(asteroid);

    return asteroid;
  }

  public void Recycle(Asteroid asteroid) {
    // Отписка от уведомлений и возврат в очередь
    ChroneManager.RemoveListener(asteroid);

    asteroid.Reset();
    _available.Enqueue(asteroid);
  }
}