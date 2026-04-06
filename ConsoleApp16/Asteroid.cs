using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp16 {
  public enum AsteroidState { Idle, Depleted }
  public class Asteroid : IChroneListener {
    public int CurrentEchos { get; set; }
    public int MaxEchos { get; set; }
    public AsteroidState State { get; set; } 
    public int SpawnID { get; set; }
    public int CreateID { get; set; }

    private static int _createCounter = 0;
    public Asteroid() {
      CreateID = _createCounter++;
      MaxEchos = new Random().Next(100, 1001);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle; 
      SpawnID = 0;
    }

    public void OnChronTick() {
      CurrentEchos -= 100;
      if (CurrentEchos <= 0) {
        State = AsteroidState.Depleted; ;
      }
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnID++;
    }

    public void OnChoneTrick() {
      OnChronTick();
    }
  }
}
