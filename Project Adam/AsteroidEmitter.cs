using System;
using System.Collections.Generic;
using AsteroidSimulator;
using AsteroidSimulator.Interfaces;
using AsteroidSimulator.Managers;

namespace AsteroidSimulator.Models {
  public class AsteroidEmitter : IChronListener {
    private readonly List<Asteroid> _activeBelt;
    private readonly Random _random;
    private readonly bool _subscribeSpawnedAsteroidsToChron;

    public Queue<Asteroid> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize, List<Asteroid> activeBelt, Random random, bool subscribeSpawnedAsteroidsToChron = true) {
      int asteroidIndex;

      _activeBelt = activeBelt;
      _random = random;
      _subscribeSpawnedAsteroidsToChron = subscribeSpawnedAsteroidsToChron;

      for (asteroidIndex = 0; asteroidIndex < initialSize; ++asteroidIndex) {
        _available.Enqueue(new Asteroid());
      }
    }

    public void RegisterWithChronManager() {
      ChronManager.AddListener(this);
    }

    public Asteroid Spawn() {
      if (_available.Count == 0) {
        Console.WriteLine("WARNING: Pool is empty! Creating new asteroid on the fly.");
        return new Asteroid();
      }

      return _available.Dequeue();
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }

    public int PoolSize => _available.Count;

    public void OnChronTick() {
      int spawnBatch;
      int maxExclusive;
      int asteroidIndex;
      Asteroid spawned;
      int currentChron;

      currentChron = SimulationClock.CurrentChron;

      if (currentChron % SimulationConstants.SpawnIntervalChrons != 0) {
        return;
      }

      maxExclusive = SimulationConstants.MaxSpawnBatchExclusive;
      spawnBatch = _random.Next(SimulationConstants.MinSpawnBatch, maxExclusive);
      Console.WriteLine("Emitter: spawning " + spawnBatch + " asteroid(s) (chron " + currentChron + ").");

      for (asteroidIndex = 0; asteroidIndex < spawnBatch; ++asteroidIndex) {
        spawned = Spawn();
        _activeBelt.Add(spawned);
        if (_subscribeSpawnedAsteroidsToChron) {
          ChronManager.AddListener(spawned);
        }
      }
    }
  }
}
