using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Observer_and_Object_pool__Laba_5 {
  
  public class Asteroid : IChroneListener {
    
    public enum AsteroidState {
      Idle,
      Depleted
    }

    Random random = new Random();
    public int MaxEchos;
    public int CurrentEchos;
    public int SpawnID;
    public int CreateID;
    public AsteroidState State;
    public static int StaticSpawnID;
    public static int StaticCreateID;

    public Asteroid() {
      MaxEchos = random.Next(1, 1001);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnID = StaticSpawnID;
      ++StaticSpawnID;
      CreateID = StaticCreateID;
      ++StaticCreateID;
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChroneTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos -= 100;
        if (CurrentEchos < 0) {
          CurrentEchos = 0;
        }
      }
    }
  }
}
