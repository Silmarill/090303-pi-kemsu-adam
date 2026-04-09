using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroids {
  public class AsteroidEmitter {
    private Queue<Asteroid> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize) // ← без void
    {
      for (int i = 0; i < initialSize; ++i) {
        _available.Enqueue(new Asteroid());
      }
    }

    public Asteroid Spawn() {
      if (_available.Count == 0) {
        return new Asteroid();
      }

      var asteroid = _available.Dequeue();
      asteroid.Reset();
      return asteroid;
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }
  }
}