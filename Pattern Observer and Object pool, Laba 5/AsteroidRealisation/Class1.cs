using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Observer_and_Object_pool__Laba_5{
  
  public class Asteroid {
    
    public enum AsteroidState {
      Idle,
      Depleted
    }

    Random random = new Random();
    public int MaxEchos, CurrentEchos, SpawnID, CreateID;
    public AsteroidState State;

    public Asteroid() {
      this.MaxEchos = random.Next(1, 1001);
      this.CurrentEchos = this.MaxEchos;
      State = AsteroidState.Idle;
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
