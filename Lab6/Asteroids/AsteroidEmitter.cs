using System;
using System.Collections.Generic;

public class AsteroidEmitter : IChronListener {
  private Queue<Asteroid> _available;
  private MotherShip _motherShip;
  private int _spawnInterval;
  private int _minSpawnAmount;
  private int _maxSpawnAmount;
  private Random _random;
  private int _chronCounter;

  public AsteroidEmitter(
    int initialPoolSize,
    MotherShip motherShip,
    int spawnInterval,
    int minSpawnAmount,
    int maxSpawnAmount
  ) {
    _available = new Queue<Asteroid>();
    _motherShip = motherShip;
    _spawnInterval = spawnInterval;
    _minSpawnAmount = minSpawnAmount;
    _maxSpawnAmount = maxSpawnAmount;
    _random = new Random();
    _chronCounter = 0;

    for (int index = 0; index < initialPoolSize; ++index) {
      _available.Enqueue(new Asteroid());
    }

    ChroneManager.AddListener(this);
  }

  public void OnChronTick() {
    ++_chronCounter;
    if (_chronCounter % _spawnInterval != 0) {
      return;
    }
    int spawnCount = _random.Next(_minSpawnAmount, _maxSpawnAmount + 1);
    for (int index = 0; index < spawnCount; ++index) {
      Asteroid asteroid = Spawn();
      _motherShip.AddAsteroid(asteroid);
    }
  }

  public Asteroid Spawn() {
    Asteroid asteroid;
    if (_available.Count == 0) {
      Console.WriteLine("  [Pool] Warning: pool is empty, creating new Asteroid.");
      asteroid = new Asteroid();
    } else {
      asteroid = _available.Dequeue();
    }
    asteroid.MarkSpawned();
    return asteroid;
  }

  public void Recycle(Asteroid usedAsteroid) {
    usedAsteroid.Reset();
    _available.Enqueue(usedAsteroid);
  }

  public int GetAvailableCount() {
    return _available.Count;
  }
}
