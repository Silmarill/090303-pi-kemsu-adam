using System;
using System.Collections.Generic;

namespace AsteroidSimulator.Models {
  public class AsteroidEmitter {
    public Queue<Asteroid> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize)
    {
      for (int asteroidIndex = 0; asteroidIndex < initialSize; ++asteroidIndex)
      {
        _available.Enqueue(new Asteroid());
      }
    }

    public Asteroid Spawn()
    {
      if (_available.Count == 0)
      {
        Console.WriteLine("WARNING: Pool is empty! Creating new asteroid on the fly.");
        return new Asteroid();
      }

      return _available.Dequeue();
    }

    public void Recycle(Asteroid asteroid)
    {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }

    public int PoolSize => _available.Count;
  }
}