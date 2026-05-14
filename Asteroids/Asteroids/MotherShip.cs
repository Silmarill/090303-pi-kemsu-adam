using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroids {
  public class MotherShip {
    private List<HarvesterShip> _harvesters = new List<HarvesterShip>();

    public Dictionary<string, List<Report>> WorkLog = new();

    public IReadOnlyList<HarvesterShip> Harvesters => _harvesters;

    public MotherShip() {
      for (int i = 0; i < 8; ++i) {
        var h = new HarvesterShip();
        _harvesters.Add(h);
        WorkLog[h.Name] = new List<Report>();
      }
    }

    public void ProcessTurn(List<Asteroid> asteroids) {
      foreach (var h in _harvesters) {
        if (h.IsIdle) {
          var target = asteroids.FirstOrDefault(a => a.State == AsteroidState.Idle);

          if (target != null) {
            h.AssignAsteroid(target);
          }
        }

        var report = h.Work();

        if (report != null) {
          WorkLog[h.Name].Add(report);
        }
      }
    }

    public void PrintSummary() {
      Console.WriteLine("=== SUMMARY ===");

      foreach (var pair in WorkLog) {
        int total = pair.Value.Sum(r => r.AmountMined);
        Console.WriteLine($"{pair.Key}: {total}");
      }
    }

    public void PrintFullLog() {
      Console.WriteLine("=== FULL LOG ===");

      foreach (var pair in WorkLog) {
        Console.WriteLine($"\n{pair.Key}:");

        foreach (var report in pair.Value) {
          Console.WriteLine(report);
        }
      }
    }
  }
}
