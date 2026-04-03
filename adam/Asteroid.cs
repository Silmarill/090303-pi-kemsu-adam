using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Asteroid {
  public int CurrentEchos;
  public int MaxEchos;
  public AsteroidState State;
  public int SpawnID;
  public int CreateID;

  public Asteroid() {
    Random random = new Random();
    MaxEchos = random.Next(100, 1001);
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  public void Reset() {
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  public void OnChronTick() {
    if (State == AsteroidState.Idle) {
      CurrentEchos -= 100;
      if (CurrentEchos <= 0) {
        CurrentEchos = 0;
        State = AsteroidState.Depleted;
      }
    }
  }
}

