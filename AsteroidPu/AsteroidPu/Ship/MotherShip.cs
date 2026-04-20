using AsteroidPu.Ship.Harvester;
using System;
using System.Collections.Generic;

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
          _activeAsteroidItems[indexI].State = AsteroidState.Mining;
          return _activeAsteroidItems[indexI];
        }
      }
      return null;
    }

    public void AssignIdleHarvesters() {
      for (int indexI = 0; indexI < fleet.Count; ++indexI) {
        if (fleet[indexI].state == HarvesterState.Idle) {
          fleet[indexI].currentAsteroid = GetIdleAsteroid();
        }
      }
    }

    public void FinishHarvest(HarvesterShip harvester) {
      harvester.state = HarvesterState.Idle;
      workLog

    }

    public void PrintAsteroidItemsInfo() {
      for (int indexI = 0; indexI <= _activeAsteroidItems.Count; ++indexI) {
        Console.WriteLine(_activeAsteroidItems[indexI]);
      }
    }
    public void PrintHarvesterItemsInfo() {
      for (int indexI = 0; indexI <= fleet.Count; ++indexI) {
        Console.WriteLine(fleet[indexI]);
      }
    }

    public void PrintTotalMined() {
      for (int indexI = 0; indexI <= fleet.Count; ++indexI) {
        Console.WriteLine($"{fleet[indexI].nameHarvester} harvester got {fleet[indexI].asteroidItemsMined} asteroids");
      }
    }

    public void PrintFullWorklog() {

    }

    public int GetActiveAsteroidItemsCount() {
      return _activeAsteroidItems.Count;
    }
  }
}
