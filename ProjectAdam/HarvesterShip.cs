using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAdam {
  public enum HarvesterState {
    Idle,
    Mining
  }
  public class HarvesterShip {
    public HarvesterState State;
    public int ID;
    public string Name;
    public int AsteroidsMined;
    public int CargoCapacity = 500;
    public int CargoCurrent;
    public int BiteSize = 50;
    public int JobCounter;

    public HarvesterShip(int id, string name) {
      ID = id;
      Name = name;
    }

    public Report Mine(Asteroid asteroid) {
     
      if (asteroid == null || asteroid.State == AsteroidState.Depleted)
        return null;
      
      asteroid.State = AsteroidState.Mining;
      State = HarvesterState.Mining;

      int mined = Math.Min(BiteSize, asteroid.CurrentEchos);
      asteroid.CurrentEchos -= mined;

      CargoCurrent += mined;

      if (asteroid.CurrentEchos <= 0) {
        asteroid.CurrentEchos = 0;
        asteroid.State = AsteroidState.Depleted;
        AsteroidsMined++;
      }

      JobCounter++;

      return new Report(JobCounter, asteroid.SpawnID, mined);
    }
    public bool isFull() {
      return CargoCurrent >= CargoCapacity;
    }

    public void Unload() {
      CargoCurrent = 0;
      State = HarvesterState.Idle;
    }
  }
}
