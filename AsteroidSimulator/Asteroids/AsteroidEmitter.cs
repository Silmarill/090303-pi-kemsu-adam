using System;
using System.Collections.Generic;

namespace AsteroidSimulator.Asteroids;

public class AsteroidEmitter {
  private Queue<Asteroid> _available;
  private int _totalCreated;
  private const int InitialPoolSize = 5;

  public int TotalCreated {
    get { return _totalCreated; }
  }

  public int AvailableCount {
    get { return _available.Count; }
  }

  public AsteroidEmitter() {
    _available = new Queue<Asteroid>();
    _totalCreated = 0;

    for (int i = 0; i < InitialPoolSize; i++) {
      Asteroid asteroid = new Asteroid();
      _available.Enqueue(asteroid);
      _totalCreated = _totalCreated + 1;
    }
  }

  public Asteroid Spawn() {
    Asteroid asteroid;

    if (_available.Count == 0) {
      asteroid = new Asteroid();
      _totalCreated = _totalCreated + 1;
      Console.WriteLine("[Предупреждение] Пул пуст! Создан новый астероид (всего создано: " + _totalCreated + ")");
    } else {
      asteroid = _available.Dequeue();
    }

    asteroid.SetSpawnId();
    return asteroid;
  }

  public void Recycle(Asteroid asteroid) {
    asteroid.Reset();
    _available.Enqueue(asteroid);
  }
}