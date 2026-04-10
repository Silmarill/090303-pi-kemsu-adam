using System;
using Asteroids;

namespace Harvester {
  public class HarvesterShip {
    public int nextId;
    public int cargoCapacity;
    public int biteSize;

    public int id;
    public string name;
    public int asteroidsMined;
    public int cargoCurrent;
    public HarvesterState state;
    public Asteroid currentAsteroid;
    public int currentJobNumber;

    public HarvesterShip(string name, int nextIdValue, int cargoCapacityValue, int biteSizeValue)
    {
      this.nextId = nextIdValue;
      this.cargoCapacity = cargoCapacityValue;
      this.biteSize = biteSizeValue;

      this.id = this.nextId;
      ++this.nextId;
      this.name = name;
      this.asteroidsMined = 0;
      this.cargoCurrent = 0;
      this.state = HarvesterState.Idle;
      this.currentAsteroid = null;
      this.currentJobNumber = 0;
    }

    public bool StartMining(Asteroid asteroid)
    {
      bool miningStarted;

      if (state == HarvesterState.Idle && asteroid != null && asteroid.state == AsteroidState.Idle)
      {
        miningStarted = asteroid.StartMining();
        if (miningStarted)
        {
          currentAsteroid = asteroid;
          state = HarvesterState.Mining;
          ++currentJobNumber;
          return true;
        }
      }
      return false;
    }

    public void Mine()
    {
      int amountToMine;
      int actualMined;

      if (state == HarvesterState.Mining && currentAsteroid != null)
      {

        if (currentAsteroid.currentEchos >= biteSize)
        {
          amountToMine = biteSize;
        }
        else
        {
          amountToMine = currentAsteroid.currentEchos;
        }

        if (cargoCurrent + amountToMine <= cargoCapacity)
        {
          actualMined = amountToMine;
        }
        else
        {
          actualMined = cargoCapacity - cargoCurrent;
        }

        if (actualMined > 0)
        {
          currentAsteroid.Mine(actualMined);
          cargoCurrent = cargoCurrent + actualMined;
        }
      }
    }

    public bool IsMiningComplete()
    {
      if (state == HarvesterState.Mining)
      {
        if (currentAsteroid != null && currentAsteroid.state == AsteroidState.Depleted)
        {
          return true;
        }

        if (cargoCurrent >= cargoCapacity)
        {
          return true;
        }
      }
      return false;
    }

    public Report Unload()
    {
      Report report;
      int amountMined;

      amountMined = cargoCurrent;
      report = new Report(currentJobNumber, currentAsteroid.spawnId, amountMined);

      ++asteroidsMined;
      cargoCurrent = 0;
      state = HarvesterState.Idle;

      if (currentAsteroid != null && currentAsteroid.state == AsteroidState.Depleted)
      {
        currentAsteroid = null;
      }
      else if (currentAsteroid != null)
      {
        currentAsteroid.state = AsteroidState.Idle;
        currentAsteroid = null;
      }

      return report;
    }

    public void PrintInfo()
    {
      Console.WriteLine($"  {name} (ID: {id}) | Статус: {state} | Груз: {cargoCurrent}/{cargoCapacity} | Астероидов добыто: {asteroidsMined}");
      if (state == HarvesterState.Mining && currentAsteroid != null)
      {
        Console.WriteLine($"    Добывает астероид SpawnId: {currentAsteroid.spawnId} (Echos: {currentAsteroid.currentEchos})");
      }
    }

    public int GetCargoCapacity()
    {
      return cargoCapacity;
    }
  }
}