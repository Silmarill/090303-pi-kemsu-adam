using System.Collections.Generic;

namespace Project_Adam {
  public class AsteroidEmitter {
    public Queue<Asteroid> _available = new Queue<Asteroid>();
    public int _totalSpawnedCount = 0;

    public AsteroidEmitter(int initialSize) {
      for (int asteroidIndex = 0; asteroidIndex < initialSize; ++asteroidIndex) {
        Asteroid newAsteroid = new Asteroid();
        _available.Enqueue(newAsteroid);
      }
    }

    public Asteroid Spawn() {
      Asteroid spawnedAsteroid;

      if (_available.Count == 0) {
        spawnedAsteroid = new Asteroid();
      } else {
        spawnedAsteroid = _available.Dequeue();
      }

      spawnedAsteroid.SpawnID = ++_totalSpawnedCount;

      return spawnedAsteroid;
    }

    public void Recycle(Asteroid asteroidToRecycle) {
      asteroidToRecycle.Reset();
      _available.Enqueue(asteroidToRecycle);
    }
  }
}

