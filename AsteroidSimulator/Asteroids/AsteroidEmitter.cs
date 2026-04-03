using System;
using System.Collections.Generic;

namespace AsteroidSimulator.Asteroids;

public class AsteroidEmitter {
  private Queue<Asteroid> _available = new Queue<Asteroid>();
  private int _totalCreated = 0;

  public int TotalCreated => _totalCreated;
  public int AvailableCount => _available.Count;

  public AsteroidEmitter(int initialSize) {
    for (int i = 0; i < initialSize; ++i) {
      Asteroid asteroid = new Asteroid();
      _available.Enqueue(asteroid);
      _totalCreated++;
    }
  }

  public Asteroid Spawn() {
    Asteroid asteroid;

    if (_available.Count == 0) {
      asteroid = new Asteroid();
      _totalCreated++;
      Console.WriteLine($"[Предупреждение] Пул пуст! Создан новый астероид (всего создано: {_totalCreated})");
    } else {
      asteroid = _available.Dequeue();
    }

    asteroid.SetSpawnID();
    return asteroid;
  }

  public void Recycle(Asteroid asteroid) {
    asteroid.Reset();
    _available.Enqueue(asteroid);
  }
}