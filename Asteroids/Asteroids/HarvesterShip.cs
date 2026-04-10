using System;
using System.Collections.Generic;
using System.Text;

public enum HarvesterState {
  Idle,
  Mining
}

namespace Asteroids {
  public class HarvesterShip {
    public int ID;
    public string Name;
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;

    public HarvesterState StateHarvest { get; set; }

    private Asteroid _targetAsteroid;
    private int _jobCounter = 0;
    private int _currentMined = 0;

    private static readonly string[] Names = new[]
    {
        "Вадим Б12", "Копала", "Артур Копатель", "Копатель офлайн",
        "Лютейший бурила", "Закопышь", "БРБРБР", "Потерявший Астероид",
        "Натуральный Бур", "Олег"
    };

    private static int _idCounter = 0;
    private static Random random = new Random();

    public HarvesterShip() {
      ID = ++_idCounter;
      CargoCapacity = random.Next(200, 1000);
      BiteSize = random.Next(10, 100);
      Name = Names[random.Next(Names.Length)];
      StateHarvest = HarvesterState.Idle;
    }

    public bool IsIdle => StateHarvest == HarvesterState.Idle;

    public void AssignAsteroid(Asteroid asteroid) {
      _targetAsteroid = asteroid;
      StateHarvest = HarvesterState.Mining;
      _currentMined = 0;
    }

    public Report Work() {
      if (StateHarvest != HarvesterState.Mining || _targetAsteroid == null)
        return null;

      int mined = Math.Min(BiteSize, _targetAsteroid.CurrentEchos);

      _targetAsteroid.CurrentEchos -= mined;
      CargoCurrent += mined;
      _currentMined += mined;

      if (_targetAsteroid.CurrentEchos <= 0) {
        _targetAsteroid.CurrentEchos = 0;
        _targetAsteroid.State = AsteroidState.Depleted;
      }

      if (CargoCurrent >= CargoCapacity || _targetAsteroid.State == AsteroidState.Depleted) {
        CargoCurrent = 0;
        StateHarvest = HarvesterState.Idle;

        var report = new Report {
          JobNumber = ++_jobCounter,
          AsteroidSpawnID = _targetAsteroid.SpawnID,
          AmountMined = _currentMined
        };

        _targetAsteroid = null;
        return report;
      }

      return null;
    }
  }
}
