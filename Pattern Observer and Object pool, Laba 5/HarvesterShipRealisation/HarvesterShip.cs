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
    int ID;
    string name;
    int asteroidsMined;
    int cargoCapacity;
    int cargoCurrent;
    int biteSize;
    HarvesterState state;
    Asteroid currentAsteroid;
    MotherShip homeStation;
  }
}
