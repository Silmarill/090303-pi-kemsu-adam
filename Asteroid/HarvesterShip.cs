using System;

namespace Asteroid {
  public enum HarvesterState {
    Idle,
    Mining
  }

  public class HarvesterShip : IChronListener {
    private static int _nextId = 1;

    private const int CargoCapacityValue = 500;
    private const int BiteSizeValue = 50;

    public HarvesterState state;
    public Asteroid currentAsteroid;
    public int id;
    public string name;
    public int asteroidsMined;
    public int cargoCurrent;
    public int currentJobNumber;

    private MotherShip _motherShip;

    public HarvesterShip(MotherShip motherShip) {
      _motherShip = motherShip;
      id = ++_nextId;
      name = "Harvester-" + id;
      asteroidsMined = 0;
      cargoCurrent = 0;
      state = HarvesterState.Idle;
      currentAsteroid = null;
      currentJobNumber = 0;
    }

    public void OnChronTick() {
      if (state == HarvesterState.Idle) {
        TryStartMining();
      } 
      
      else if (state == HarvesterState.Mining) {
        ContinueMining();
      }
    }

    private void TryStartMining() {
      Asteroid targetAsteroid;
      targetAsteroid = _motherShip.GetAvailableAsteroid();

      int increasingJob;
      increasingJob = 1;

      if (targetAsteroid == null) {
        return;
      }

      currentAsteroid = targetAsteroid;
      currentAsteroid.StartMining();
      state = HarvesterState.Mining;
      currentJobNumber = currentJobNumber + increasingJob;
    }

    private void ContinueMining() {
      if (currentAsteroid == null) {
        state = HarvesterState.Idle;

        return;
      }

      if (currentAsteroid.state == AsteroidState.Depleted) {
        FinishMining(true);

        return;
      }

      int minedAmount;
      minedAmount = currentAsteroid.Mine(BiteSizeValue);

      if (minedAmount > 0) {
        cargoCurrent = cargoCurrent + minedAmount;
      }

      if (currentAsteroid.state == AsteroidState.Depleted) {
        FinishMining(true);
      } 
      
      else if (cargoCurrent >= CargoCapacityValue) {
        FinishMining(false);
      }
    }

    private void FinishMining(bool asteroidDepleted) {
      if (currentAsteroid != null) {
        if (asteroidDepleted) {
          currentAsteroid.StopMining();
          _motherShip.RecycleAsteroid(currentAsteroid);
        } 
        
        else {
          currentAsteroid.StopMining();
        }

        currentAsteroid = null;
      }

      int minedInThisJob;
      minedInThisJob = cargoCurrent;

      int asteroidSpawnIdForReport;

      int increasingAstMine;
      increasingAstMine = 1;

      if (currentAsteroid != null) {
        asteroidSpawnIdForReport = currentAsteroid.spawnId;
      } 
      
      else {
        asteroidSpawnIdForReport = 0;
      }

      Report newReport;
      newReport = new Report(currentJobNumber, asteroidSpawnIdForReport, minedInThisJob);

      _motherShip.AddReport(name, newReport);

      asteroidsMined = asteroidsMined + increasingAstMine;
      cargoCurrent = 0;
      state = HarvesterState.Idle;
    }

    public int GetTotalMined() {
      int total;
      total = _motherShip.GetHarvesterTotalMined(name);

      return total;
    }

    public override string ToString() {
      string stateText;

      if (state == HarvesterState.Idle) {
        stateText = "Idle";
      } 
      
      else {
        stateText = "Mining";
      }

      string asteroidInfo;

      if (currentAsteroid != null) {
        asteroidInfo = $" | Asteroid: {currentAsteroid.spawnId}";
      } 
      
      else {
        asteroidInfo = "";
      }

      return $"{name,-14} | {stateText,-6} | Cargo: {cargoCurrent,3}/{CargoCapacityValue} | Mined: {asteroidsMined,2} asteroids{asteroidInfo}";
    }
  }
}
