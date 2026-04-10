using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp16 {
  public enum AsteroidState { Idle, Depleted }
  public class Asteroid : IChroneListener {
    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID; 
    public int EchosDecreasePerTick;

    private int minMaxechos = 100;
    private int maxMaxechos = 1001;
    private static int _createCounter = 0;
    public Asteroid() {
      CreateID = _createCounter++;
      MaxEchos = new Random().Next(minMaxechos, maxMaxechos);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle; 
      SpawnID = 0;
      EchosDecreasePerTick = 100;
    }

    public void OnChronTick() {
      CurrentEchos -= EchosDecreasePerTick;
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
