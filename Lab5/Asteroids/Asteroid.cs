using System;

public class Asteroid : IChronListener {
  private static readonly Random _random = new Random();
  private static int _createCounter = 0;
  private static int _spawnCounter = 0;

  public int CurrentEchos { get; private set; }
  public int MaxEchos { get; private set; }
  public AsteroidState State { get; private set; }
  public int SpawnID { get; private set; }
  public int CreateID { get; private set; }

  public Asteroid() {
    CreateID = ++_createCounter;
    MaxEchos = _random.Next(100, 1001);
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
    SpawnID = ++_spawnCounter;
  }

  // сбрасывает состояние для возврата в пул, SpawnID обновляется при следующем спавне
  public void Reset() {
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  // вызывается из AsteroidEmitter.Spawn() — помечает факт извлечения из пула
  public void MarkSpawned() {
    SpawnID = ++_spawnCounter;
  }

  public void OnChronTick() {
    if (State != AsteroidState.Idle) {
      return;
    }

    CurrentEchos -= 100;
    if (CurrentEchos < 0) {
      CurrentEchos = 0;
    }
    if (CurrentEchos == 0) {
      State = AsteroidState.Depleted;
    }
  }

  public void PrintInfo() {
    Console.WriteLine(
      "  [Spawn #" + SpawnID + " | Create #" + CreateID + "]" +
      "  Echos: " + CurrentEchos + "/" + MaxEchos +
      "  State: " + State
    );
  }
}
