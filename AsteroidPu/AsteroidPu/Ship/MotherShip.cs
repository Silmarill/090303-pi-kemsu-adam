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
        fleet.Add(new HarvesterShip($"Harvester{indexI}", capasityOfCargo, sizeOfBite, this));
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
      Report report = new Report(harvester.harvesterID, harvester.currentAsteroid.SpawnID, harvester.cargoCurrent);
      if (!workLog.ContainsKey(harvester.nameHarvester)) {
        workLog[harvester.nameHarvester] = new List<Report>();
      }
      workLog[harvester.nameHarvester].Add(report);
      harvester.cargoCurrent = 0;
      if(harvester.currentAsteroid.State == AsteroidState.Depleted) {
        _asteroidEmitter.Recycle(harvester.currentAsteroid);
      }
      harvester.state = HarvesterState.Idle;
    }

    public void PrintAsteroidItemsInfo() {
      Console.WriteLine($"Active asteroid:\n");
      for (int indexI = 0; indexI < _activeAsteroidItems.Count; ++indexI) {
        Console.WriteLine($"{_activeAsteroidItems[indexI]}\n");
      }
    }
    public void PrintHarvesterItemsInfo() {
      Console.WriteLine("Harvesters:\n");
      for (int indexI = 0; indexI < fleet.Count; ++indexI) {
        Console.WriteLine($"{fleet[indexI]}\n");
      }
    }

    public void PrintTotalMined() {
      Console.WriteLine("Total mined:\n");
      for (int indexI = 0; indexI < fleet.Count; ++indexI) {
        Console.WriteLine($"{fleet[indexI].nameHarvester} harvester got {fleet[indexI].asteroidItemsMined} asteroids.\n");
      }
    }

    public void PrintFullWorklog() {
      List<string> fullNamesHarvesters = new List<string>(workLog.Keys);
      for(int indexI = 0; indexI < workLog.Count; ++indexI) {
        string newName = fullNamesHarvesters[indexI];
        List<Report> reports = workLog[newName];
        for (int indexJ = 0; indexJ < fullNamesHarvesters.Count; ++indexJ) {
          reports[indexJ].Print();
        }
      }

    }

    public int GetActiveAsteroidItemsCount() {
      return _activeAsteroidItems.Count;
    }
  }
}
