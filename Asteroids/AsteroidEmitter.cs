namespace AsteroidsLab.Asteroids;

public class AsteroidEmitter
{
  private readonly Queue<Asteroid> _available;
  private static int s_nextSpawnId = 0;

  public AsteroidEmitter(int initialSize)
  {
    int poolIndex;
    Asteroid asteroid;

    _available = new Queue<Asteroid>();

    for (poolIndex = 0; poolIndex < initialSize; ++poolIndex)
    {
      asteroid = new Asteroid();
      _available.Enqueue(asteroid);
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

  public void Recycle(Asteroid asteroid)
  {
    asteroid.Reset();
    _available.Enqueue(asteroid);
  }
}
