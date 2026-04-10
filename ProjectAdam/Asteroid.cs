using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAdam {
  public enum AsteroidState {
    Idle,
    Depleted
  }
  public class Asteroid : IChroneListener {
    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    private static int createCount = 0;
    private static int spawnCount = 0;
    private static Random random = new Random();
    private static int minEchosRange = 100;
    private static int maxEchosRange = 1001;
    private static int chunkSize = 100;

    public Asteroid() {
      CreateID = ++createCount; //захотелось начинать ID с одного
      MaxEchos = random.Next(minEchosRange, maxEchosRange);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnSpawn() {
      SpawnID = ++spawnCount; //захотелось начинать ID с одного
      Reset();
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChroneTick() {
      if (State == AsteroidState.Idle) {

        CurrentEchos -= chunkSize;

        if (CurrentEchos < 0) {
          CurrentEchos = 0;
        }

        if (CurrentEchos == 0) {
          State = AsteroidState.Depleted;
        }

      } else {
        return;
      }
    }
  }
}
