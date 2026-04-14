using System;
using System.Collections.Generic;

namespace Asteroid {
  public class AsteroidEmitter {
    private Queue<Asteroid> _availablePool;
    private int _totalSpawnCount;
    private int _initialPoolSize;

    public AsteroidEmitter(int initialSize) {
      _initialPoolSize = initialSize;
      _availablePool = new Queue<Asteroid>();
      _totalSpawnCount = 0;

      for (int asteroidIndex = 0; asteroidIndex < initialSize; ++asteroidIndex) {
        Asteroid newAsteroid;
        newAsteroid = new Asteroid();
        _availablePool.Enqueue(newAsteroid);
      }
    }

    public Asteroid Spawn() {
      Asteroid asteroid;

      int increasingCount;
      increasingCount = 1;

      if (_availablePool.Count == 0) {
        asteroid = new Asteroid();
        Console.WriteLine("  [Pool warning: empty, creating new asteroid]");
      } 
      
      else {
        asteroid = _availablePool.Dequeue();
      }

      _totalSpawnCount = _totalSpawnCount + increasingCount;
      asteroid.SetSpawnId(_totalSpawnCount);

      return asteroid;
    }

    public void Recycle(Asteroid asteroid) {
      if (asteroid == null) {
        return;
      }

      asteroid.Reset();
      _availablePool.Enqueue(asteroid);
    }

    public int GetAvailableCount() {
      return _availablePool.Count;
    }

    public int GetInitialPoolSize() {
      return _initialPoolSize;
    }
  }
}
