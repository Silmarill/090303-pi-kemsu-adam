using System;

namespace AsteroidPu {
  public class Asteroid {

    public int CurrentEchos;
    int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Asteroid() {
      Random countOfAsteroids = new Random();

      MaxEchos = countOfAsteroids.Next(100, 1000);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      //Genius CreateID
      if (CreateID < MaxEchos) {
        ++CreateID;
      }
      //No less genius SpawnID
      if (CreateID % 3 == 0) {
        ++SpawnID;
      }
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChoneTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos -= 100;
        if (CurrentEchos < 0) {
          CurrentEchos = 0;
        }
        if (CurrentEchos == 0) {
          State = AsteroidState.Depleted;
        }
      }
    }
  }
}