using System;

public class Asteroid : IChroneListener {
  public int currentEchos;
  public int maxEchos;
  public AsteroidState state;
  public int spawnId;
  public int createId;

  private static int globalCreateIdCounter;
  private static Random random;

  public Asteroid() {
    if (random == null) {
      random = new Random();
      globalCreateIdCounter = 0;
    }

    createId = ++globalCreateIdCounter;

    maxEchos = random.Next(100, 1001);
    currentEchos = maxEchos;

    state = AsteroidState.Idle;
    spawnId = 0;
  }

  public void SetSpawnId(int spawnId) {
    this.spawnId = spawnId;
  }

  public void Reset() {
    currentEchos = maxEchos;
    state = AsteroidState.Idle;
  }

  public void OnChroneTick() {
    // стабилизационное поле MotherShip (астероиды не деградируют)
  }

  public override string ToString() {
    return $"[CreateId: {createId:D2} | SpawnId: {spawnId:D2}] Echos: {currentEchos}/{maxEchos} | State: {state}";
  }
}