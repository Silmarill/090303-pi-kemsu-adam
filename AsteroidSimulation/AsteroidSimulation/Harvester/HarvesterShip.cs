using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation {
  public class HarvesterShip : IChroneListener {
    private static int _nextHarvesterId = 1;

    private int _harvesterId = 0;
    private int _cargoCapacity = 500;
    private int _cargoCurrent = 0;
    private int _biteSize = 50;
    private string _harvesterName;
    private int _asteroidsMined = 0;
    private HarvesterState _state = HarvesterState.Idle;

    public Asteroid currentAsteroid = null;
    private MotherShip _homeStation;

    public HarvesterShip(string name, int capacity, int biteSizeValue, MotherShip station) {
      _harvesterId = _nextHarvesterId++;
      _harvesterName = name;
      _cargoCapacity = capacity;
      _biteSize = biteSizeValue;
      _homeStation = station;
    }

    public void OnChronTick() {
      if (_state == HarvesterState.Mining && currentAsteroid != null) {
        int mineAmount = Math.Min(_biteSize, currentAsteroid.CurrentEchos);
        currentAsteroid.CurrentEchos -= mineAmount;
        _cargoCurrent += mineAmount;

        if (currentAsteroid.CurrentEchos <= 0) {
          currentAsteroid.CurrentEchos = 0;
          currentAsteroid.State = AsteroidState.Depleted;
          _homeStation.FinishHarvest(this);
        }
        else if (_cargoCurrent >= _cargoCapacity) {
          _homeStation.FinishHarvest(this);
        }
      }
    }

    public void StartMining(Asteroid asteroid) {
      currentAsteroid = asteroid;
      asteroid.State = AsteroidState.Mining;
      _state = HarvesterState.Mining;
    }

    public Report CreateReport(int jobNumber) {
      return new Report(jobNumber, currentAsteroid.SpawnId, _cargoCurrent);
    }

    public void Unload() {
      _asteroidsMined++;
      _cargoCurrent = 0;
      currentAsteroid = null;
      _state = HarvesterState.Idle;
    }

    public void PrintInfo() {
      Console.Write($"\nInfo about {_harvesterId} harvester: ");
      Console.WriteLine($"\nName: {_harvesterName}" +
                        $"\nCapacity: {_cargoCapacity}" +
                        $"\nCurrent: {_cargoCurrent}" +
                        $"\nMined asteroids: {_asteroidsMined}" +
                        $"\nState: {_state}");
    }

    public string GetName() {
      return _harvesterName;
    }

    public int GetCargoCurrent() {
      return _cargoCurrent;
    }

    public HarvesterState GetState() {
      return _state;
    }

    public int GetHarvesterId() {
      return _harvesterId;
    }

    public int GetAsteroidsMined() {
      return _asteroidsMined;
    }
  }
}