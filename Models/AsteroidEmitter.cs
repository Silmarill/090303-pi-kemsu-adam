using System.Collections.Generic;

namespace AsteroidZoneSimulation.Models {
  public class AsteroidEmitter {
    private Queue<Asteroid> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize) {
      for (int asteroidCount = 0; asteroidCount < initialSize; ++asteroidCount) {
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

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }

    public int GetAvailableCount() {
      return _available.Count;
    }
  }
}
