using System;

namespace AsteroidSimulator.Models {
  public class Asteroid {
    public static int GlobalSpawnCounter;
    public static int GlobalCreateCounter;

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int CreateId;
    public int SpawnId;

    public Asteroid() {
      int minEchos;
      int maxEchos;
      int rangeOffset;
      int maxInclusive;
      int stepOne;
      Random randomGenerator;

      minEchos = 100;
      maxEchos = 1000;
      rangeOffset = 1;
      stepOne = 1;
      maxInclusive = maxEchos + rangeOffset;
      randomGenerator = new Random();

      this.MaxEchos = randomGenerator.Next(minEchos, maxInclusive);
      this.CurrentEchos = this.MaxEchos;
      this.State = AsteroidState.Idle;
      this.CreateId = GlobalCreateCounter + stepOne;
      GlobalCreateCounter = this.CreateId;
      this.SpawnId = 0;
    }

    public void Reset() {
      int minEchos;
      int maxEchos;
      int rangeOffset;
      int maxInclusive;
      int stepOne;
      Random randomGenerator;

      minEchos = 100;
      maxEchos = 1000;
      rangeOffset = 1;
      stepOne = 1;
      maxInclusive = maxEchos + rangeOffset;
      randomGenerator = new Random();

      this.MaxEchos = randomGenerator.Next(minEchos, maxInclusive);
      this.CurrentEchos = this.MaxEchos;
      this.State = AsteroidState.Idle;
      this.SpawnId = GlobalSpawnCounter + stepOne;
      GlobalSpawnCounter = this.SpawnId;
    }

    public int Mine(int biteSize) {
      int minedAmount;
      int zero;

      zero = 0;

      if (this.State != AsteroidState.Mining) {
        return zero;
      }

      if (biteSize < this.CurrentEchos) {
        minedAmount = biteSize;
      } else {
        minedAmount = this.CurrentEchos;
      }

      this.CurrentEchos = this.CurrentEchos - minedAmount;

      if (this.CurrentEchos == zero) {
        this.State = AsteroidState.Depleted;
      }

      return minedAmount;
    }

    public void StartMining() {
      if (this.State == AsteroidState.Idle) {
        this.State = AsteroidState.Mining;
      }
    }

    public void StopMining() {
      if (this.State == AsteroidState.Mining) {
        this.State = AsteroidState.Idle;
      }
    }

    public override string ToString() {
      string result;

      result = "Asteroid #" + this.SpawnId + " (Created:" + this.CreateId + ") | " + this.CurrentEchos + "/" + this.MaxEchos + " | " + this.State;
      return result;
    }
  }
}