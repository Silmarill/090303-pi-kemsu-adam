using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSpace {
  public enum AsteroidState { Idle, Mining, Depleted }
  public class Asteroid : IChroneListener {
    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;
    public int EchosDecreasePerTick;

    private int minMaxechos = 100;
    private int maxMaxechos = 1001;
    private int DefaultEchosDecreasePerTick = 100;
    private static int _createCounter = 0;
    private static int _spawnCounter = 0;

    public Asteroid() {
      CreateID = ++_createCounter;
      MaxEchos = new Random().Next(minMaxechos, maxMaxechos);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnID = 0;
      EchosDecreasePerTick = DefaultEchosDecreasePerTick;
      SpawnID = ++_spawnCounter;
    }

    public void OnChronTick() {
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