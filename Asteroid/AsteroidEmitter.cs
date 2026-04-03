using System;
using System.Collections.Generic;

namespace Asteroid {
  public class AsteroidEmitter {
    private Queue<Asteroid> _pool = new Queue<Asteroid>();
    private int _totalSpawned = 0;

    public AsteroidEmitter(int initialSize) {
      for (int index = 0; index < initialSize; ++index) {
        _pool.Enqueue(new Asteroid());
      }
    }

    public Asteroid Spawn() {
      Asteroid asteroid;

      if (_pool.Count == 0) {
        asteroid = new Asteroid();
        Console.WriteLine("  [Pool expanded]");
      } 
      
      else {
        asteroid = _pool.Dequeue();
      }

      asteroid.SetSpawnId(++_totalSpawned);

      return asteroid;
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _pool.Enqueue(asteroid);
    }

    public int AvailableCount() {
      return _pool.Count;
    }
  }
}
