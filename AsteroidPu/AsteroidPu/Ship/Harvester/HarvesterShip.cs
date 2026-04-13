using AsteroidPu.Chrones;
using AsteroidPu.Ship.Harvester;
using System;

namespace AsteroidPu.Ship {
  public class HarvesterShip : IChroneListener {
    int harvesterID = 0,
      cargoCapasity = 10,
      cargoCurrent = 0,
      biteSize;
    string nameHarvester;
    int asteroidItemsMined = 0;
    HarvesterState state;
    public Asteroid currentAsteroid;
    MotherShip HomeStation;

    public HarvesterShip(string name, int capasity, int sizeForBites, MotherShip station) {
      nameHarvester = name;
      cargoCapasity = capasity;
      biteSize = sizeForBites;
      HomeStation = station;
      ++harvesterID;
    }

    public void OnChroneTick() {
      if (state == HarvesterState.Mining && currentAsteroid != null) {

        currentAsteroid.CurrentEchos -= biteSize;

        if (currentAsteroid.CurrentEchos < currentAsteroid.minСorrectNum) {
          currentAsteroid.CurrentEchos = currentAsteroid.minСorrectNum;
        }

        ++cargoCurrent;

        if (currentAsteroid.State == AsteroidState.Depleted || cargoCurrent == cargoCapasity) {
          HomeStation.FinishHarvest(this);
        }
      }
    }

    public void StartMining(Asteroid asteroid) {
      currentAsteroid = asteroid;
      asteroid.State = AsteroidState.Mining;
      state = HarvesterState.Mining;

    }

    public Report CreateReport() {

    }

    public void PrintInfo() {
      Console.Write($"\nInfo about {harvesterID} harvester: ");
      Console.WriteLine($"\nName: {nameHarvester}"+
                        $"\nCapasity: {cargoCapasity}"+
                        $"\nCurrent: {cargoCurrent}"+
                        $"\nState: {state}");
    }

  }
}
