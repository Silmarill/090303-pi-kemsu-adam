using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidPu {
  public class Asteroid {

    int CurrentEchos;
    int MaxEchos;
    AsteroidState State;
    int SpawnID;
    int CreateID;

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
        --CurrentEchos;

      }
    }
  }
}