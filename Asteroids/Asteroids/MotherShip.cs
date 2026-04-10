using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroids {
  public class MotherShip {

    private List<HarvesterShip> _harvesters = new List<HarvesterShip>();

    public MotherShip() {
      for (int i = 0; i < 5; ++i) {
        var harvester = new HarvesterShip();
        harvester.Create();
        _harvesters.Add(harvester);
      }
    }

    public List<HarvesterShip> GetHarvesters() {
      return _harvesters;
    }
  }
}
