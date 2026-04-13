using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidPu.Ship {
  public class MotherShip {

    public List<HarvesterShip> fleet = new List<HarvesterShip>();
    public Dictionary<string, List<Report>> workLog = new Dictionary<string, List<Report>>();

    List<Asteroid> _activeAsteroidItems = new List<Asteroid>();
    AsteroidEmitter _asteroidEmitter;

    public MotherShip(int harvesterCount, int capasityOfCargo, int sizeOfBite) {
      for (int indexI = 0; indexI < harvesterCount; ++indexI) {
        fleet[indexI] = new HarvesterShip($"Harvester{indexI}", capasityOfCargo, sizeOfBite, this);
      }
    }

    public void SetEmitter(AsteroidEmitter emitter) {
      _asteroidEmitter = emitter;
    }

    public void AddAsteroid(Asteroid asteroid) {
      _activeAsteroidItems.Add(asteroid);
    }

    public void RemoveAsteroid(Asteroid asteroid) {
      _activeAsteroidItems.Remove(asteroid);
      _asteroidEmitter.Recycle(asteroid);
    }

    public Asteroid GetIdleAsteroid() {
      for (int indexI = 0; indexI < _activeAsteroidItems.Count; ++indexI) {
        if (_activeAsteroidItems[indexI].State == AsteroidState.Idle) {
          return _activeAsteroidItems[indexI];
        }
      }
      return null;
    }

  }
}
