using System;

namespace Project_Adam {
  public class HarvesterShip {
    private static int _nextId = 0;

    private const int DefaultCargoCapacity = 500;
    private const int DefaultBiteSize = 50;

    public int ID;
    public string Name;
    public int AsteroidsMined;
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;
    public HarvesterState State;
    public int CurrentAsteroidSpawnID;

    public HarvesterShip(string name) {
      ID = ++_nextId;
      Name = name;
      AsteroidsMined = 0;
      CargoCapacity = DefaultCargoCapacity;
      CargoCurrent = 0;
      BiteSize = DefaultBiteSize;
      State = HarvesterState.Idle;
      CurrentAsteroidSpawnID = -1;
    }

    public void Mine(Asteroid asteroid) {
      if (State != HarvesterState.Mining) {
        return;
      }

      int actualMine = Math.Min(BiteSize, asteroid.CurrentEchos);
      asteroid.CurrentEchos -= actualMine;
      CargoCurrent += actualMine;

      if (asteroid.CurrentEchos == 0) {
        asteroid.State = AsteroidState.Depleted;
      }
    }
  }
}
