using System;
using System.Collections.Generic;
using AsteroidZoneSimulation.Core;

namespace AsteroidZoneSimulation.Models {
  public class AsteroidEmitter : IChroneListener {
    private Queue<Asteroid> _availableAsteroids = new Queue<Asteroid>();
    private MotherShip _motherShip;
    private int _spawnInterval;
    private int _minSpawnAmount, _maxSpawnAmount;
    private static Random _random = new Random();
    private int _chroneCounter = 0;
    private int _nextSpawnID = 1;

    public AsteroidEmitter(int initialPoolSize, Mothership motherShip, int spawnInterval, int minSpawnAmount, int maxSpawnAmount) {
      _motherShip = motherShip;
      _spawnInterval = spawnInterval;
      _minSpawnAmount = minSpawnAmount;
      _maxSpawnAmount = maxSpawnAmount;

      for (int asteroidCount = 0; asteroidCount < initialPoolSize; ++asteroidCount) {
        Asteroid asteroid = new Asteroid();
        _availableAsteroids.Enqueue(asteroid);
      }

      ChroneManager.AddListener(this);
    }

    public void OnChroneTick() {
      ++_chroneCounter;

      if (_chroneCounter % _spawnInterval == 0) {
        int newCount = _random.Next(_minSpawnAmount, _maxSpawnAmount + 1);
        Console.WriteLine($"\n[Событие] Спавн {newCount} новых астероидов!");

        for (int spawnIndex = 0; spawnIndex < newCount; ++spawnIndex) {
          Asteroid asteroid = Spawn();
          _motherShip.AddAsteroid(asteroid);
        }
      }
    }

    public Asteroid Spawn() {
      Asteroid asteroid;

      if (_availableAsteroids.Count == 0) {
        asteroid = new Asteroid();
      } else {
        asteroid = _availableAsteroids.Dequeue();
      }

      asteroid.SetSpawnID(++_nextSpawnID);
      asteroid.Reset();
      return asteroid;
    }

    public void Recycle(Asteroid usedAsteroid) {
      usedAsteroid.Reset();
      _availableAsteroids.Enqueue(usedAsteroid);
    }

    public int GetAvailableCount() {
      return _availableAsteroids.Count;
    }
  }
}
