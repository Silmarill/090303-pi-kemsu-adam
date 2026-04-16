using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Asteroid : IChroneListener {
  public int CurrentEchos;
  public int MaxEchos;
  public AsteroidState State;
  public int SpawnID;
  public int CreateID;
  public static int nextCreateId;
  public static int nextSpawnId;
  public static Random random = new Random();
  public static MotherShip HomeStation;
  public Asteroid() {
    MaxEchos = random.Next(100, 1001);
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
    CreateID = ++nextCreateId;
  }

  public void Reset() {
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  public void OnChroneTick() {
    if (HomeStation != null && !HomeStation.IsStabilizing) {
      CurrentEchos -= 100;
      if (CurrentEchos <= 0) {
        CurrentEchos = 0;
        State = AsteroidState.Depleted;
      }
    }
  }

  public void PrintInfo() {
    Console.WriteLine($"Asteroid {SpawnID}: Echos {CurrentEchos}/{MaxEchos}, State: {State}.");
  }
}