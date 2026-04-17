using System;

public class Asteroid : IChronListener {
  private static readonly Random _random = new Random();
  private static int _createCounter = 0;
  private static int _spawnCounter = 0;

  public int currentEchos;
  public int maxEchos;
  public AsteroidState state;
  public int spawnID;
  public int createID;

  public Asteroid() {
    createID = ++_createCounter;
    maxEchos = _random.Next(100, 1001);
    currentEchos = maxEchos;
    state = AsteroidState.Idle;
    spawnID = ++_spawnCounter;
  }

  public void Reset() {
    currentEchos = maxEchos;
    state = AsteroidState.Idle;
  }

  public void MarkSpawned() {
    spawnID = ++_spawnCounter;
  }

  // деградация отключена — зона стабилизирована станцией
  public void OnChronTick() {
  }

  public void PrintInfo() {
    Console.WriteLine(
      "  [Spawn #" + spawnID + " | Create #" + createID + "]" +
      "  Echos: " + currentEchos + "/" + maxEchos +
      "  State: " + state
    );
  }
}
