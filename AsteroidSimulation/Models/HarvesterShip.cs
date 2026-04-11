using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation.Models {
  public class HarvesterShip {
    private static int _nextId = 1;

    public int ID;
    public string Name;
    public int AsteroidsMined;      // Количество отработанных астероидов
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;
    public HarvesterState State;

    // Ссылка на астероид, который сейчас добывается (если State == Mining)
    public Asteroid CurrentAsteroid;

    public HarvesterShip(string name) {
      ID = ++_nextId;
      Name = name;
      AsteroidsMined = 0;
      CargoCapacity = 500;        
      CargoCurrent = 0;
      BiteSize = 50;              
      State = HarvesterState.Idle;
      CurrentAsteroid = null;
    }

    // Метод добычи
    // Возвращает true, если добыча завершена (астероид истощён или трюм полон)
    public bool Mine(Asteroid asteroid) {
      if (asteroid == null || asteroid.State != AsteroidState.Mining) {
        return false;
      }

      // Сколько можно добыть за этот укус
      int availableToMine = asteroid.CurrentEchos;
      int canMine = Math.Min(availableToMine, BiteSize);

      // Проверяем, сколько осталось места в трюме
      int spaceLeft = CargoCapacity - CargoCurrent;
      int actualMined = Math.Min(canMine, spaceLeft);

      if (actualMined <= 0) {
        // Трюм полон или астероид пуст
        return true;
      }

      // Добываем
      asteroid.CurrentEchos -= actualMined;
      CargoCurrent += actualMined;

      // Если астероид истощился
      if (asteroid.CurrentEchos <= 0) {
        asteroid.CurrentEchos = 0;
        asteroid.State = AsteroidState.Depleted;
        return true;  // добыча завершена
      }

      // Если трюм заполнился
      if (CargoCurrent >= CargoCapacity) {
        return true;  // добыча завершена
      }

      return false;  // добыча продолжается
    }

    // Разгрузка на станции (создаёт отчёт)
    public Report Unload(int jobNumber) {
      Report report = new Report(jobNumber, CurrentAsteroid.SpawnID, CargoCurrent);
      ++AsteroidsMined;
      CargoCurrent = 0;
      CurrentAsteroid = null;
      State = HarvesterState.Idle;
      return report;
    }

    // Начать добычу астероида
    public void StartMining(Asteroid asteroid) {
      CurrentAsteroid = asteroid;
      asteroid.State = AsteroidState.Mining;
      State = HarvesterState.Mining;
    }

    public override string ToString() {
      string status;
      if (State == HarvesterState.Idle) {
        status = "Idle";
      } else {
        status = $"Mining (Asteroid SpawnID={CurrentAsteroid?.SpawnID})";
      }

      return $"Harvester #{ID} [{Name}]: {status}, Cargo: {CargoCurrent}/{CargoCapacity}, Mined asteroids: {AsteroidsMined}";
    }
  }
}
