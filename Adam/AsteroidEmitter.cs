using Adam;
using System;
using System.Collections.Generic;

namespace Asteroids {
  public class AsteroidEmitter {
    private Queue<Asteroid> _availableAsteroids = new Queue<Asteroid>();
    private int _totalCreated;

    public int AvailableCount {
      get {
        return _availableAsteroids.Count;
      }
    }

    public int TotalCreated {
      get {
        return _totalCreated;
      }
    }

    public AsteroidEmitter(int initialPoolSize) {
      for (int asteroidIndex = 0; asteroidIndex < initialPoolSize; asteroidIndex++) {
        Asteroid newAsteroid = new Asteroid();
        _availableAsteroids.Enqueue(newAsteroid);
        _totalCreated++;
      }
    }

    public Asteroid Spawn() {
      Asteroid asteroid;

      if (_availableAsteroids.Count == 0) {
        Console.WriteLine("Warning: pool is empty, creating new asteroid");
        asteroid = new Asteroid();
        _totalCreated++;
      } else {
        asteroid = _availableAsteroids.Dequeue();
      }

      asteroid.Reset();
      ChronoManager.AddListener(asteroid);
      return asteroid;
    }

    public void Recycle(Asteroid usedAsteroid) {
      if (usedAsteroid.State != AsteroidState.Depleted) {
        Console.WriteLine($"Warning: recycling non-depleted asteroid #{usedAsteroid.CreateID}");
      }
      ChronoManager.RemoveListener(usedAsteroid);
      usedAsteroid.Reset();
      _availableAsteroids.Enqueue(usedAsteroid);
    }
  }
} 