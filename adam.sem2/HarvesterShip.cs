using System;

namespace AsteroidSimulator.Models {
  public class HarvesterShip {
    private static int nextId = 1;

    public int Id { get; private set; }
    public string Name { get; private set; }
    public int AsteroidsMined { get; private set; }
    public int CargoCapacity { get; private set; }
    public int CargoCurrent { get; private set; }
    public int BiteSize { get; private set; }
    public HarvesterState State { get; private set; }

    private Asteroid currentAsteroid;

    public HarvesterShip(string name, int cargoCapacity = 500, int biteSize = 50)
    {
      Id = nextId++;
      Name = name;
      CargoCapacity = cargoCapacity;
      BiteSize = biteSize;
      CargoCurrent = 0;
      AsteroidsMined = 0;
      State = HarvesterState.Idle;
      currentAsteroid = null;
    }

    public bool IsIdle => State == HarvesterState.Idle;

    public bool AssignAsteroid(Asteroid asteroid)
    {
      if (State != HarvesterState.Idle) return false;
      if (asteroid == null || asteroid.State != AsteroidState.Idle) return false;

      currentAsteroid = asteroid;
      currentAsteroid.StartMining();
      State = HarvesterState.Mining;
      return true;
    }

    public bool MineTick()
    {
      if (State != HarvesterState.Mining) return false;
      if (currentAsteroid == null || currentAsteroid.State == AsteroidState.Depleted)
      {
        FinishMining();
        return false;
      }

      int mined = currentAsteroid.Mine(BiteSize);
      CargoCurrent += mined;

      if (currentAsteroid.State == AsteroidState.Depleted)
      {
        FinishMining();
        return true;
      }

      if (CargoCurrent >= CargoCapacity)
      {
        FinishMining();
        return true;
      }

      return true;
    }

    private void FinishMining()
    {
      if (currentAsteroid != null)
      {
        currentAsteroid.StopMining();
        currentAsteroid = null;
      }
      State = HarvesterState.Idle;
    }

    public Report Unload(int jobNumber)
    {
      var report = new Report
      {
        JobNumber = jobNumber,
        AsteroidSpawnID = currentAsteroid?.SpawnId ?? 0,
        AmountMined = CargoCurrent
      };
      AsteroidsMined++;
      CargoCurrent = 0;
      return report;
    }

    public Report UnloadPartial(int jobNumber)
    {
      return Unload(jobNumber);
    }

    public override string ToString()
    {
      return $"{Name} (ID:{Id}) | {State} | Cargo:{CargoCurrent}/{CargoCapacity} | Mined:{AsteroidsMined}";
    }
  }
}