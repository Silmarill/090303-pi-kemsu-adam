using System;
using System.Collections.Generic;

namespace AsteroidSimulator.Models {
  public class AsteroidEmitter {
    public Queue<Asteroid> Available;

    public AsteroidEmitter(int initialSize) {
      int asteroidIndex;
      int stepOne;
      int zero;

      stepOne = 1;
      zero = 0;

      this.Available = new Queue<Asteroid>();

      for (asteroidIndex = zero; asteroidIndex < initialSize; asteroidIndex = asteroidIndex + stepOne) {
        Asteroid newAsteroid;

        newAsteroid = new Asteroid();
        this.Available.Enqueue(newAsteroid);
      }
    }

    public Asteroid Spawn() {
      int zero;

      zero = 0;

      if (this.Available.Count == zero) {
        return new Asteroid();
      }

      return this.Available.Dequeue();
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      this.Available.Enqueue(asteroid);
    }

    public int GetPoolSize() {
      return this.Available.Count;
    }
  }
}