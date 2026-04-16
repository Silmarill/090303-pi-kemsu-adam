using AsteroidSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSpace {
  public class HarvesterShip {
    public int ID;
    public string Name;
    public int AsteroidMined;
    public int BiteSize;
    public int CargoCapacity;
    public int CargoCurrent;
    public HarvesterState State;

    private Asteroid _currentAsteroid;
    private int _currentJobNumber;
    private static int DefaultCargoCapacity = 500;
    private static int DefaultBiteSize = 50;
    private static int _nextId = 0;

    public HarvesterShip(string shipName) : this(shipName, DefaultCargoCapacity, DefaultBiteSize) {
    }

    public HarvesterShip(string shipName, int cargoCapacity, int biteSize) {
      ID = ++_nextId;
      Name = shipName;
      CargoCapacity = cargoCapacity;
      BiteSize = biteSize;
      CargoCurrent = 0;
      AsteroidMined = 0;
      State = HarvesterState.Idle;
      _currentAsteroid = null;
      _currentJobNumber = 0;
    }

    public bool Mine(Asteroid targetAsteroid) {
      if (targetAsteroid == null || targetAsteroid.State == AsteroidState.Depleted) {
        return false;
      }

      int possibleMineAmount = Math.Min(BiteSize, targetAsteroid.CurrentEchos);
      int availableCargoSpace = CargoCapacity - CargoCurrent;
      int actualMinedAmount = Math.Min(possibleMineAmount, availableCargoSpace);

      if (actualMinedAmount <= 0) {
        return false;
      }

      targetAsteroid.CurrentEchos -= actualMinedAmount;
      CargoCurrent += actualMinedAmount;

      if (targetAsteroid.CurrentEchos <= 0) {
        targetAsteroid.State = AsteroidState.Depleted;
      }

      return true;
    }

    public void StartMining(Asteroid targetAsteroid, int jobNumber) {
      _currentAsteroid = targetAsteroid;
      _currentJobNumber = jobNumber;
      State = HarvesterState.Mining;
      targetAsteroid.State = AsteroidState.Mining;
    }

    public bool ContinueMining() {
      if (_currentAsteroid == null || State != HarvesterState.Mining) {
        return false;
      }

      Mine(_currentAsteroid);

      bool isAsteroidDepleted = _currentAsteroid.State == AsteroidState.Depleted;
      bool isCargoFull = CargoCurrent >= CargoCapacity;

      if (isAsteroidDepleted || isCargoFull) {
        return true;
      }

      return false;
    }

    public Report FinishMining() {
      if (_currentAsteroid == null) {
        return null;
      }

      int minedAmount = CargoCurrent;
      Report completedReport = new Report(_currentJobNumber, _currentAsteroid.SpawnID, minedAmount);

      ++AsteroidMined;
      CargoCurrent = 0;
      State = HarvesterState.Idle;

      if (_currentAsteroid.State != AsteroidState.Depleted) {
        _currentAsteroid.State = AsteroidState.Idle;
      }

      _currentAsteroid = null;

      return completedReport;
    }

    public void Unload() {
      CargoCurrent = 0;
    }

    public string GetHarvesterInfo() {
      return Name + " (ID:" + ID + ") | Статус: " + State + " | Груз: " + CargoCurrent + "/" + CargoCapacity + " | Астероидов добыто: " + AsteroidMined;
    }
  }
}