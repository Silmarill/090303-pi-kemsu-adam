using System;

namespace AsteroidPu {
  public class Asteroid {

    public int CurrentEchos;
    int stepEchos = 100,
      MaxEchos;
    public int minСorrectNum = 0;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Asteroid() {
      Random randomEchos = new Random();

      MaxEchos = randomEchos.Next(100, 1000);
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

    public void PrintInfo() {
      Console.Write($"Info about {this.SpawnID} asteroid:\n\n");
      Console.WriteLine($"CurrentEchos: {this.CurrentEchos}\n" +
                      $"Asteroid Create ID: {this.CreateID}\n" +
                      $"Asteroid Spawn ID: {this.SpawnID}");
      Console.WriteLine("\nEnter esc for exit, enter to continue and R for output total production");
    }
  }
}