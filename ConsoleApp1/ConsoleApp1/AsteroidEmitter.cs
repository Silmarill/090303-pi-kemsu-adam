using System;
using System.Collections.Generic;

public class AsteroidEmitter {
  private Queue<Asteroid> _available = new Queue<Asteroid>();
  public AsteroidEmitter(int initialSize) {
    for (int idx = 0; idx < initialSize; idx++) {
      Asteroid asteroid = new Asteroid();
      _available.Enqueue(asteroid);
    }
  }

  public Asteroid Spawn() {
    if (_available.Count == 0) {
      return new Asteroid();
    }
    return _available.Dequeue();
  }

  public void Recycle(AsteroidEmitter asteroid) {
    asteroid.Reset();
    _available.Enqueue(asteroid);
  }
}