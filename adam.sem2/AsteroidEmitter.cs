using System;
using System.Collections.Generic;

namespace AsteroidSimulator.Models {
  public class AsteroidEmitter {
    public Queue<Asteroid> Available;

    public AsteroidEmitter(int initialSize)
    {
      int asteroidIndex;
      Asteroid newAsteroid;

      this.Available = new Queue<Asteroid>();

      for (asteroidIndex = 0; asteroidIndex < initialSize; ++asteroidIndex)
      {
        newAsteroid = new Asteroid();
        this.Available.Enqueue(newAsteroid);
      }
    }

    public Asteroid Spawn()
    {
      Asteroid asteroidFromPool;

      if (this.Available.Count == 0)
      {
        Console.WriteLine("WARNING: Pool is empty! Creating new asteroid on the fly.");
        asteroidFromPool = new Asteroid();
        return asteroidFromPool;
      }

      asteroidFromPool = this.Available.Dequeue();
      return asteroidFromPool;
    }

    public void Recycle(Asteroid asteroidToRecycle)
    {
      asteroidToRecycle.Reset();
      this.Available.Enqueue(asteroidToRecycle);
    }

    public int GetPoolSize() {
      int poolSize;
      poolSize = this.Available.Count;
      return poolSize;
    }
  }
}