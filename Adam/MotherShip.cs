using System;
using System.Collections.Generic;
using Asteroids;
using Harvester;

namespace MotherShip {
  public class MotherShip {
    public List<HarvesterShip> fleet;
    public Dictionary<string, List<Report>> worklog;

    public MotherShip()
    {
      int nextId;
      int cargoCapacity;
      int biteSize;
      string[] names;
      int index;

      fleet = new List<HarvesterShip>();
      worklog = new Dictionary<string, List<Report>>();

      nextId = 1;
      cargoCapacity = 500;
      biteSize = 50;

      names = new string[] { "Харвестер-Альфа", "Харвестер-Бета", "Харвестер-Гамма", "Харвестер-Дельта", "Харвестер-Эпсилон" };

      for (index = 0; index < names.Length; ++index)
      {
        HarvesterShip ship;

        ship = new HarvesterShip(names[index], nextId, cargoCapacity, biteSize);
        fleet.Add(ship);
        worklog.Add(ship.name, new List<Report>());
        ++nextId;
      }
    }

    public void AddReport(HarvesterShip ship, Report report)
    {
      if (worklog.ContainsKey(ship.name))
      {
        worklog[ship.name].Add(report);
      }
    }

    public int GetTotalMinedByHarvester(string harvesterName)
    {
      int total;
      List<Report> reports;

      total = 0;

      if (worklog.ContainsKey(harvesterName))
      {
        reports = worklog[harvesterName];
        foreach (var report in reports)
        {
          total = total + report.amountMined;
        }
      }

      return total;
    }

    public void PrintTotalMined()
    {
      int total;
      Console.WriteLine("\n--- СУММАРНАЯ ДОБЫЧА ПО ХАРВЕСТЕРАМ ---");
      foreach (var ship in fleet)
      {

        total = GetTotalMinedByHarvester(ship.name);
        Console.WriteLine($"  {ship.name}: {total} единиц Echos");
      }
    }

    public void PrintFullWorklog()
    {
      Console.WriteLine("\n========== ПОЛНЫЙ ЖУРНАЛ РАБОТ (WORKLOG) ==========");
      foreach (var ship in fleet)
      {
        List<Report> reports;

        Console.WriteLine($"\n{ship.name}:");
        reports = worklog[ship.name];

        if (reports.Count == 0)
        {
          Console.WriteLine("    Нет отчётов");
        }
        else
        {
          foreach (var report in reports)
          {
            report.PrintInfo();
          }
        }
      }
      Console.WriteLine("==================================================\n");
    }

    public void PrintAllInfo(List<Asteroid> activeAsteroids, int chronCounter)
    {
      int displayNumber;
      int displayOffset;

      displayOffset = 1;
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine($"=== ТЕКУЩИЙ ХРОН: {chronCounter} ===");
      Console.ResetColor();

      Console.WriteLine("\n--- АКТИВНЫЕ АСТЕРОИДЫ ---");
      if (activeAsteroids.Count == 0)
      {
        Console.WriteLine("  Нет активных астероидов");
      }
      else
      {
        for (int position = 0; position < activeAsteroids.Count; ++position)
        {
          displayNumber = position + displayOffset;
          Console.Write($"[{displayNumber}] ");
          activeAsteroids[position].PrintInfo();
        }
      }

      Console.WriteLine("\n--- ФЛОТ ХАРВЕСТЕРОВ ---");
      foreach (var ship in fleet)
      {
        ship.PrintInfo();
      }

      PrintTotalMined();
    }

    public void UpdateHarvesters(List<Asteroid> activeAsteroids)
    {
      List<HarvesterShip> completedHarvesters;

      completedHarvesters = new List<HarvesterShip>();

      foreach (var ship in fleet)
      {
        if (ship.state == HarvesterState.Mining)
        {
          ship.Mine();

          if (ship.IsMiningComplete())
          {
            completedHarvesters.Add(ship);
          }
        }
      }

      foreach (var ship in completedHarvesters)
      {
        Report report;

        report = ship.Unload();
        AddReport(ship, report);
      }

      foreach (var ship in fleet)
      {
        if (ship.state == HarvesterState.Idle)
        {
          Asteroid targetAsteroid;

          targetAsteroid = FindAvailableAsteroid(activeAsteroids);
          if (targetAsteroid != null)
          {
            ship.StartMining(targetAsteroid);
          }
        }
      }
    }

    public Asteroid FindAvailableAsteroid(List<Asteroid> activeAsteroids)
    {
      foreach (var asteroid in activeAsteroids)
      {
        if (asteroid.state == AsteroidState.Idle)
        {
          return asteroid;
        }
      }
      return null;
    }

    public List<Asteroid> RemoveDepletedAsteroids(List<Asteroid> activeAsteroids, AsteroidEmitter emitter)
    {
      List<Asteroid> remainingAsteroids;
      List<Asteroid> depletedList;

      remainingAsteroids = new List<Asteroid>();
      depletedList = new List<Asteroid>();

      foreach (var asteroid in activeAsteroids)
      {
        if (asteroid.state == AsteroidState.Depleted)
        {
          depletedList.Add(asteroid);
        }
        else
        {
          remainingAsteroids.Add(asteroid);
        }
      }

      foreach (var depleted in depletedList)
      {
        emitter.Recycle(depleted);
        Console.WriteLine($"  [Удалён] Астероид SpawnId:{depleted.spawnId} истощён и возвращён в пул");
      }

      return remainingAsteroids;
    }

    public void SpawnNewAsteroids(int chronCounter, AsteroidEmitter emitter, List<Asteroid> activeAsteroids)
    {
      int newAsteroidsCount;
      int maxNewAsteroidsInclusive;
      int minNewAsteroids;
      int maxNewAsteroids;
      int spawnInterval;
      Random random;

      minNewAsteroids = 1;
      maxNewAsteroids = 3;
      spawnInterval = 5;

      if (chronCounter % spawnInterval == 0)
      {
        random = new Random();
        maxNewAsteroidsInclusive = maxNewAsteroids + minNewAsteroids;
        newAsteroidsCount = random.Next(1, maxNewAsteroidsInclusive);
        Console.WriteLine($"\n*** Генерация {newAsteroidsCount} новых астероидов! ***");

        for (int index = 0; index < newAsteroidsCount; ++index)
        {
          Asteroid newAsteroid;

          newAsteroid = emitter.Spawn();
          activeAsteroids.Add(newAsteroid);
        }
      }
    }
  }
}