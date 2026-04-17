using AsteroidsLab.Asteroids;
using AsteroidsLab.Interfaces;
using AsteroidsLab.Managers;

namespace AsteroidsLab.Fleet;

public class HarvesterShip : IChroneListener
{
  public int Id;
  public string Name;
  public int AsteroidsMined;
  public int CargoCapacity;
  public int CargoCurrent;
  public int BiteSize;
  public HarvesterState State;
  public Asteroid? CurrentAsteroid;
  public MotherShip HomeStation;

  private int _nextJobNumber;

  public HarvesterShip(int id, string name, int cargoCapacity, int biteSize, MotherShip homeStation)
  {
    Id = id;
    Name = name;
    CargoCapacity = cargoCapacity;
    CargoCurrent = 0;
    BiteSize = biteSize;
    AsteroidsMined = 0;
    State = HarvesterState.Idle;
    CurrentAsteroid = null;
    HomeStation = homeStation;
    _nextJobNumber = 0;

    ChroneManager.AddListener(this);
  }

  public void StartMining(Asteroid asteroid)
  {
    CurrentAsteroid = asteroid;
    State = HarvesterState.Mining;
    asteroid.State = AsteroidState.Mining;
  }

  public void OnChroneTick()
  {
    int fromRock;
    int roomInCargo;
    int transfer;
    Asteroid rock;

    if (State != HarvesterState.Mining || CurrentAsteroid == null)
    {
      return;
    }

    rock = CurrentAsteroid;

    fromRock = BiteSize;
    if (fromRock > rock.CurrentEchos)
    {
      fromRock = rock.CurrentEchos;
    }

    roomInCargo = CargoCapacity - CargoCurrent;
    if (roomInCargo < fromRock)
    {
      transfer = roomInCargo;
    }
    else
    {
      transfer = fromRock;
    }

    if (transfer > 0)
    {
      rock.CurrentEchos -= transfer;
      CargoCurrent += transfer;
      if (rock.CurrentEchos == 0)
      {
        rock.State = AsteroidState.Depleted;
      }
    }

    if (CargoCurrent >= CargoCapacity || rock.State == AsteroidState.Depleted)
    {
      HomeStation.FinishHarvest(this);
    }
  }

  public Report CreateReport()
  {
    int spawnId;
    int amount;
    Report report;

    spawnId = 0;

    if (CurrentAsteroid != null)
    {
      spawnId = CurrentAsteroid.SpawnId;
    }

    amount = CargoCurrent;
    ++_nextJobNumber;
    report = new Report(_nextJobNumber, spawnId, amount);
    return report;
  }

  public void PrintInfo()
  {
    Console.WriteLine(
      "  " + Name + " | " + State
      + " | cargo " + CargoCurrent + "/" + CargoCapacity
      + " | asteroids fully mined: " + AsteroidsMined);
  }
}
