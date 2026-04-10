using System;

namespace AsteroidSimulator.Models {
  public class Asteroid : Interfaces.IChronListener {
    public static int GlobalCreateCounter;
    public static int GlobalSpawnCounter;

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int CreateId;
    public int SpawnId;

    static Asteroid()
    {
      GlobalCreateCounter = 0;
      GlobalSpawnCounter = 0;
    }

    public Asteroid() {
      Random randomNumber;
      int minEchos;
      int maxEchos;
      int rangeOffset;

      randomNumber = new Random();
      minEchos = 100;
      maxEchos = 1000;
      rangeOffset = 1;

      this.MaxEchos = randomNumber.Next(minEchos, maxEchos + rangeOffset);
      this.CurrentEchos = this.MaxEchos;
      this.State = AsteroidState.Idle;
      this.CreateId = ++GlobalCreateCounter;
      this.SpawnId = 0;
    }

    public void Reset() {
      Random randomNumber;
      int minEchos;
      int maxEchos;
      int rangeOffset;

      randomNumber = new Random();
      minEchos = 100;
      maxEchos = 1000;
      rangeOffset = 1;

      this.MaxEchos = randomNumber.Next(minEchos, maxEchos + rangeOffset);
      this.CurrentEchos = this.MaxEchos;
      this.State = AsteroidState.Idle;
      this.SpawnId = ++GlobalSpawnCounter;
    }

    public void OnChronTick() {
      int echosLossPerTick;

      echosLossPerTick = 100;

      if (this.State == AsteroidState.Idle) {
        this.CurrentEchos = this.CurrentEchos - echosLossPerTick;

        if (this.CurrentEchos <= 0) {
          this.CurrentEchos = 0;
          this.State = AsteroidState.Depleted;
        }
      }
    }

    public override string ToString()
    {
      string result;

      result = "Asteroid #" + this.SpawnId + " (Created: #" + this.CreateId + ") | Echos: " + this.CurrentEchos + "/" + this.MaxEchos + " | State: " + this.State;
      return result;
    }
  }
}