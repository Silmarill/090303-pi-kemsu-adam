using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pattern_Observer_and_Object_pool__Laba_5.HarvesterShipRealisation;
using Pattern_Observer_and_Object_pool__Laba_5.PoolObject;
using Pattern_Observer_and_Object_pool__Laba_5.ProgramReport;

namespace Pattern_Observer_and_Object_pool__Laba_5 {
  public class MotherShip {
    public List<HarvesterShip> Fleet;
    public Dictionary<string, List<Report>> workLog;
    List<Asteroid> _activeAsteroids;
    AsteroidEmitter _asteroidEmitter;

    public void AddAsteroid(Asteroid asteroid) {
      _activeAsteroids.Add(asteroid);
    }

    public void RemoveAsteroid(Asteroid asteroid) {
      _activeAsteroids.Remove(asteroid);
    }


    public MotherShip() {
      Fleet = new List<HarvesterShip>();
    }

    public Asteroid GetIdleAsteroid() {
      for (int count = 0; count < _activeAsteroids.Count; ++count) {
        if (_activeAsteroids[count].State == Asteroid.AsteroidState.Idle) {
          return _activeAsteroids[count];
        }
      }
      return null;
    }

    public void AssignIdleHarvesters(Asteroid asteroid) {
      if (asteroid.State == Asteroid.AsteroidState.Idle) {
        for (int count = 0; count < Fleet.Count; ++count) {
          
        }
      }
    }

    public void FinishHarvest(HarvesterShip harvester) {
      for (int count = 0; count < Fleet.Count; ++count) {
        Fleet[count].state = HarvesterState.Idle;

      }
    }
  }
}
