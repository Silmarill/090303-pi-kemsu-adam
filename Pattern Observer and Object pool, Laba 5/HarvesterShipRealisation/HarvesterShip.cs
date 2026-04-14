using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Observer_and_Object_pool__Laba_5.HarvesterShipRealisation {
  public enum HarvesterState {
    Idle,
    Mining
  }

  public class HarvesterShip : IChroneListener {
    public int ID;
    public string name;
    public int asteroidsMined;
    public int cargoCapacity;
    public int cargoCurrent;
    public int biteSize;
    public HarvesterState state;
    public Asteroid currentAsteroid;
    public MotherShip homeStation;

    public HarvesterShip(string name, int cargoCapacity, int biteSize) {
      this.name = name;
      this.cargoCapacity = cargoCapacity;
      this.biteSize = biteSize;
    }

    public void StartMining(Asteroid asteroid) {
      currentAsteroid = asteroid;
      state = HarvesterState.Mining;
      asteroid.State = Asteroid.AsteroidState.Mining;
    }

    public void OnChroneTick() {
      if (state == HarvesterState.Mining) {
        currentAsteroid.CurrentEchos -= biteSize;
        cargoCurrent += biteSize;

        if (cargoCurrent == cargoCapacity || currentAsteroid.CurrentEchos == 0) {
         state = HarvesterState.Idle;
        }
      }
    }
  }
}