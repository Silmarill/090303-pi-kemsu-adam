using System;

namespace AsteroidPu {
  public class Asteroid {

    public int CurrentEchos;
    int stepEchos = 100,
      MaxEchos,
      minСorrectNum = 0;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Asteroid() {
      Random countOfAsteroids = new Random();

      MaxEchos = countOfAsteroids.Next(100, 1000);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      //Genius CreateID
      ++CreateID;
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChoneTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos -= stepEchos;
        if (CurrentEchos < minСorrectNum) {
          CurrentEchos = minСorrectNum;
        }
        if (CurrentEchos == minСorrectNum) {
          State = AsteroidState.Depleted;
        }
      }
    }
  }
}