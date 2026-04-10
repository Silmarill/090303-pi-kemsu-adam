using AsteroidPu.Chrones;
using AsteroidPu.Ship.Harvester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidPu.Ship {
  public class HarvesterShip : IChroneListener {
    int harvesterID,
      cargoCapasity,
      cargoCurrent,
      biteSize;
    string nameHarvester;
    int sateroidItemsMined = 0;
    HarvesterState state;
    Asteroid currentAsteroid = new Asteroid();
    MotherShip HomeStation;

    public HarvesterShip(string name, int capasity, int sizeForBites) {
      nameHarvester = name;
      cargoCapasity = capasity;
      biteSize = sizeForBites;
    }

    public void OnChroneTick() {

    }

  }
}
