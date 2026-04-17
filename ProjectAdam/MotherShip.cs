using ProjectAdam;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAdam
{
  public class MotherShip
  {
    private const int InitialHarvesterCount = 5;

    public List<HarvesterShip> Fleet;
    public Dictionary<string, List<Report>> Worklog;

    private Random randomGenerator;
    private int chronCount;

    public MotherShip()
    {
      string[] names = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };

      this.Fleet = new List<HarvesterShip>();
      this.Worklog = new Dictionary<string, List<Report>>();
      this.randomGenerator = new Random();
      this.chronCount = 0;

      for (int i = 0; i < InitialHarvesterCount && i < names.Length; ++i)
      {
        HarvesterShip newShip = new HarvesterShip(names[i]);
        this.Fleet.Add(newShip);
        this.Worklog.Add(newShip.Name, new List<Report>());
      }
    }

    public void OnChronTick(List<Asteroid> activeAsteroids)
    {
      ++chronCount;

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
          int jobNumber = this.Worklog[ship.Name].Count + 1;
          Report report = ship.Unload(jobNumber);
          this.Worklog[ship.Name].Add(report);
          Console.WriteLine($"[{chronCount}] {ship.Name} unloaded: {report}");
        }
      }

      foreach (var ship in Fleet)
      {
        if (ship.IsIdle() && ship.CargoCurrent == 0)
        {
          var freeAsteroids = activeAsteroids.Where(a => a.State == AsteroidState.Idle && !a.IsBeingMined).ToList();

          if (freeAsteroids.Any())
          {
            Asteroid target = freeAsteroids[randomGenerator.Next(freeAsteroids.Count)];
            ship.AssignAsteroid(target);
            Console.WriteLine($"[{chronCount}] {ship.Name} started mining asteroid #{target.SpawnID}");
          }
        }
      }
    }

    public void PrintStatus(List<Asteroid> activeAsteroids)
    {
      Console.WriteLine("\n=== STATUS ===");
      Console.WriteLine($"Chron: {chronCount}");

      Console.WriteLine("\nAsteroids:");
      foreach (var asteroid in activeAsteroids)
      {
        Console.WriteLine($"  #{asteroid.SpawnID}: {asteroid.CurrentEchos}/{asteroid.MaxEchos} echos, {(asteroid.IsBeingMined ? "being mined" : "free")}");
      }

      Console.WriteLine("\nShips:");
      foreach (var ship in Fleet)
      {
        Console.WriteLine($"  {ship}");
      }

      var totalMined = GetTotalMined();
      Console.WriteLine("\nTotal mined:");
      foreach (var kvp in totalMined)
      {
        Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
      }
      Console.WriteLine("===============\n");
    }

    public Dictionary<string, int> GetTotalMined()
    {
      var result = new Dictionary<string, int>();

      int total;
      foreach (var ship in Fleet)
      {
        total = 0;
        foreach (var report in Worklog[ship.Name])
        {
          total += report.AmountMined;
        }
        result.Add(ship.Name, total);
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
        {
          Console.WriteLine("  No completed jobs");
        }
        else
        {
          foreach (var report in Worklog[ship.Name])
          {
            Console.WriteLine($"  {report}");
          }
        }
      }

      Console.WriteLine("===================\n");
    }
  }
}