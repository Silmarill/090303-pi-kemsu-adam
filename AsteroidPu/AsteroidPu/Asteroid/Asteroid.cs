using System;
using AsteroidPu.Chrones;

namespace AsteroidPu {
  public class Asteroid : IChroneListener {

    public int CurrentEchos;
    int stepEchos = 100,
      MaxEchos;
    public int minCorrectNum = 0;
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

      ChronoManager.AddListener(this);
    }

    public void TakeResourse(int biteSize) {
      CurrentEchos -= biteSize;
      if (CurrentEchos <= minCorrectNum) {
        CurrentEchos = minCorrectNum;
        State = AsteroidState.Depleted;
        ChronoManager.RemoveListener(this);
      }
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChroneTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos -= stepEchos;
        if (CurrentEchos < minCorrectNum) {
          CurrentEchos = minCorrectNum;
        }
      }

      if (CurrentEchos == minCorrectNum) {
        State = AsteroidState.Depleted;
        ChronoManager.RemoveListener(this);
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