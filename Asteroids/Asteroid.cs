using AsteroidsLab.Interfaces;

namespace AsteroidsLab.Asteroids;

public class Asteroid : IChroneListener
{
  private static int s_nextCreateId;
  private static Random s_random;

  static Asteroid()
  {
    s_nextCreateId = 0;
    s_random = new Random();
  }

  public int CurrentEchos;
  public int MaxEchos;
  public AsteroidState State;
  public int SpawnId;
  public int CreateId;

  public Asteroid()
  {
    int randomCap;
    int minEchosCapacityInclusive;
    int maxEchosRandomUpperExclusive;

    minEchosCapacityInclusive = 100;
    maxEchosRandomUpperExclusive = 1001;

    ++s_nextCreateId;
    CreateId = s_nextCreateId;
    randomCap = s_random.Next(minEchosCapacityInclusive, maxEchosRandomUpperExclusive);
    MaxEchos = randomCap;
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
    SpawnId = 0;
  }

  public void SetSpawnId(int spawnId)
  {
    SpawnId = spawnId;
  }

  public void Reset()
  {
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  public void OnChroneTick()
  {
    int newAmount;
    int echosLostPerChron;

    echosLostPerChron = 100;

    if (State != AsteroidState.Idle)
    {
      return;
    }

    newAmount = CurrentEchos - echosLostPerChron;
    if (newAmount < 0)
    {
      newAmount = 0;
    }

    CurrentEchos = newAmount;

    if (CurrentEchos == 0)
    {
      State = AsteroidState.Depleted;
    }
  }

  public void PrintInfo()
  {
    Console.WriteLine(
      "  CreateId: " + CreateId
      + ", SpawnId: " + SpawnId
      + ", MaxEchos: " + MaxEchos
      + ", CurrentEchos: " + CurrentEchos
      + ", State: " + State);
  }
}
