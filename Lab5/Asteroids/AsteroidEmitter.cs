using System;
using System.Collections.Generic;

public class AsteroidEmitter {
  private Queue<Asteroid> _available = new Queue<Asteroid>();

  public AsteroidEmitter(int initialSize) {
    for (int index = 0; index < initialSize; ++index) {
      Asteroid asteroid = new Asteroid();
      _available.Enqueue(asteroid);
    }
  }

  public Asteroid Spawn() {
    Asteroid asteroid;
    if (_available.Count == 0) {
      Console.WriteLine("  [Pool] Warning: pool is empty, creating new Asteroid.");
      asteroid = new Asteroid();
    } else {
      asteroid = _available.Dequeue();
    }
    asteroid.MarkSpawned();
    return asteroid;
  }

  public void Recycle(Asteroid asteroid) {
    asteroid.Reset();
    _available.Enqueue(asteroid);
  }
}
