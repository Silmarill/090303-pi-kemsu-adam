using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AsteroidEmitter : IChroneListener {
  private Queue<Asteroid> _availableAsteroids;
  private MotherShip _mothership;
  private int _spawnInterval;
  private int _minSpawnAmount;
  private int _maxSpawnAmount;
  private Random _random;
  private int _chronCounter;

  public AsteroidEmitter(int initialPoolSize, MotherShip motherShip, int spawnInterval, int minSpawnAmount, int maxSpawnAmount) {
    _availableAsteroids = new Queue<Asteroid>();
    _mothership = motherShip;
    _spawnInterval = spawnInterval;
    _minSpawnAmount = minSpawnAmount;
    _maxSpawnAmount = maxSpawnAmount;
    _random = new Random();
    _chronCounter = 0;

    for (int asteroidIndex = 0; asteroidIndex < initialPoolSize; ++asteroidIndex) {
      _availableAsteroids.Enqueue(new Asteroid(_mothership));
    }

    ChroneManager.AddListener(this);
  }

  public void OnChroneTick() {
    ++_chronCounter;
    if (_chronCounter == _spawnInterval) {
      _chronCounter = 0;

      int newCount = _random.Next(_minSpawnAmount, _maxSpawnAmount + 1);

      for (int asteroidNumber = 0; asteroidNumber < newCount; ++asteroidNumber) {
        Asteroid newAsteroid = Spawn();
        _mothership.AddAsteroid(newAsteroid);
        ChroneManager.AddListener(newAsteroid);
      }
    }
  }

  public Asteroid Spawn() {
    if (_availableAsteroids.Count == 0) {
      return new Asteroid(_mothership);
    }

    return _availableAsteroids.Dequeue();
  }

  public void Recycle(Asteroid asteroid) {
    asteroid.Reset();
    _availableAsteroids.Enqueue(asteroid);
  }

  public int GetAvailableCount() {
    return _availableAsteroids.Count;
  }
}