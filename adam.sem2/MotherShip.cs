using System;
using System.Collections.Generic;
using System.Linq;

namespace AsteroidSimulator.Models {
  public class MotherShip {
    public List<HarvesterShip> Fleet { get; private set; }
    public Dictionary<string, List<Report>> Worklog { get; private set; }

    private int totalChrons = 0;
    private Random rand = new Random();

    public MotherShip(int harvesterCount = 5)
    {
      Fleet = new List<HarvesterShip>();
      Worklog = new Dictionary<string, List<Report>>();

      string[] names = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };
      for (int i = 0; i < harvesterCount && i < names.Length; i++)
      {
        var ship = new HarvesterShip(names[i]);
        Fleet.Add(ship);
        Worklog[ship.Name] = new List<Report>();
      }
    }

    public void OnChronTick(List<Asteroid> activeAsteroids)
    {
      totalChrons++;

      foreach (var ship in Fleet)
      {
        if (ship.State == HarvesterState.Mining)
        {
          ship.MineTick();
        }
      }

      foreach (var ship in Fleet)
      {
        if (ship.State == HarvesterState.Idle && ship.CargoCurrent > 0)
        {
          int jobNum = Worklog[ship.Name].Count + 1;
          var report = ship.Unload(jobNum);
          Worklog[ship.Name].Add(report);
        }
      }
      foreach (var ship in Fleet)
      {
        if (ship.IsIdle)
        {
          var freeAsteroids = activeAsteroids.Where(a => a.State == AsteroidState.Idle).ToList();
          if (freeAsteroids.Any())
          {
            var target = freeAsteroids[rand.Next(freeAsteroids.Count)];
            ship.AssignAsteroid(target);
          }
        }
      }
    }

    public Dictionary<string, int> GetTotalMinedPerHarvester()
    {
      var result = new Dictionary<string, int>();
      foreach (var ship in Fleet)
      {
        int total = Worklog[ship.Name].Sum(r => r.AmountMined);
        result[ship.Name] = total;
      }
      return result;
    }

    public void PrintWorklog()
    {
      Console.WriteLine("\n=== FULL WORKLOG ===");
      foreach (var ship in Fleet)
      {
        Console.WriteLine($"\n{ship.Name} (ID:{ship.Id}):");
        if (Worklog[ship.Name].Count == 0)
          Console.WriteLine("  No jobs yet");
        else
        {
          foreach (var report in Worklog[ship.Name])
            Console.WriteLine($"  {report}");
        }
      }
      Console.WriteLine("===================\n");
    }
  }
}