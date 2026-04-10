using System;
using System.Collections.Generic;

namespace AsteroidPu {
  public class AsteroidEmitter {
    private Queue<Asteroid> _available = new Queue<Asteroid>();
    int numForCompare = 0;

    public AsteroidEmitter(int initialSize) {
      for (int indexI = 0; indexI < initialSize; ++indexI) {
        Asteroid asteroid = new Asteroid();
        _available.Enqueue(asteroid);
      }
    }

    public Asteroid Spawn() {
      if (_available.Count == numForCompare) {
        return new Asteroid();
      }
      return _available.Dequeue();
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }
  }
}