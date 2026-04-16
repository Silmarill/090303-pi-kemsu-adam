using System;
using System.Collections.Generic;

namespace ProjectAdam {
  public class AsteroidEmitter {
    private Queue<Asteroid> _available;
    private int _nextSpawnId;

    public AsteroidEmitter(int initialSize) {
      _available = new Queue<Asteroid>();
      _nextSpawnId = 1;
      for (int poolFillIndex = 0; poolFillIndex < initialSize; ++poolFillIndex) {
        _available.Enqueue(new Asteroid());
      }
    }

    public Asteroid Spawn() {
      Asteroid asteroid;
      if (_available.Count == 0) {
        Console.WriteLine(
            "[AsteroidEmitter] Warning: pool is empty; allocating new Asteroid().");
        asteroid = new Asteroid();
      } else {
        asteroid = _available.Dequeue();
      }

      asteroid.SpawnID = _nextSpawnId;
      ++_nextSpawnId;
      return asteroid;
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }
  }
}