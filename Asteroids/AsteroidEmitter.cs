using AsteroidsLab.Fleet;
using AsteroidsLab.Interfaces;
using AsteroidsLab.Managers;

namespace AsteroidsLab.Asteroids;

public class AsteroidEmitter : IChroneListener
{
  private readonly Queue<Asteroid> _available;
  private static int s_nextSpawnId;

  private MotherShip _motherShip;
  private int _spawnInterval;
  private int _minSpawnAmount;
  private int _maxSpawnAmount;
  private Random _random;
  private int _chronTickCounter;

  static AsteroidEmitter()
  {
    s_nextSpawnId = 0;
  }

  public AsteroidEmitter(
    int initialPoolSize,
    MotherShip motherShip,
    int spawnInterval,
    int minSpawnAmount,
    int maxSpawnAmount)
  {
    int poolPrefillIndex;
    Asteroid asteroid;

    _motherShip = motherShip;
    _spawnInterval = spawnInterval;
    _minSpawnAmount = minSpawnAmount;
    _maxSpawnAmount = maxSpawnAmount;
    _random = new Random();
    _chronTickCounter = 0;

    _available = new Queue<Asteroid>();

    for (poolPrefillIndex = 0; poolPrefillIndex < initialPoolSize; ++poolPrefillIndex)
    {
      asteroid = new Asteroid();
      _available.Enqueue(asteroid);
    }

    ChroneManager.AddListener(this);
  }

  public void OnChroneTick()
  {
    int spawnCount;
    int spawnBatchIndex;
    Asteroid spawned;
    int nextExclusive;

    ++_chronTickCounter;
    if (_chronTickCounter % _spawnInterval != 0)
    {
      return;
    }

    nextExclusive = _maxSpawnAmount + 1;
    spawnCount = _random.Next(_minSpawnAmount, nextExclusive);

    for (spawnBatchIndex = 0; spawnBatchIndex < spawnCount; ++spawnBatchIndex)
    {
      spawned = Spawn();
      _motherShip.AddAsteroid(spawned);
    }
  }

  public Asteroid Spawn()
  {
    Asteroid asteroid;

    if (_available.Count == 0)
    {
      Console.WriteLine("Warning: pool is empty, a new asteroid was allocated.");
      asteroid = new Asteroid();
    }
    else
    {
      asteroid = _available.Dequeue();
    }

    ++s_nextSpawnId;
    asteroid.SetSpawnId(s_nextSpawnId);
    return asteroid;
  }

  public void Recycle(Asteroid usedAsteroid)
  {
    usedAsteroid.Reset();
    _available.Enqueue(usedAsteroid);
  }

  public int GetAvailableCount()
  {
    return _available.Count;
  }
}
