using Asteroids;

class Program {
  static void Main() {
    var emitter = new AsteroidEmitter(5);
    var activeAsteroids = new List<Asteroid>();
    var motherShip = new MotherShip();

    int turn = 0;
    Random random = new Random();

    for (int i = 0; i < 6; ++i) {
      var a = emitter.Spawn();
      activeAsteroids.Add(a);
      ChroneManager.AddListener(a);
    }

    Console.WriteLine($"=== TURN {turn} ===\n");

    Console.WriteLine("ASTEROIDS:");
    foreach (var a in activeAsteroids) {
      Console.WriteLine($"ID:{a.SpawnID} | {a.CurrentEchos}/{a.MaxEchos} | {a.State}");
    }

    Console.WriteLine();

    Console.WriteLine("HARVESTERS:");
    foreach (var h in motherShip.Harvesters) {
      Console.WriteLine($"{h.Name} | {h.Lvl} | {h.StateHarvest} | Cargo: {h.CargoCurrent}/{h.CargoCapacity} | Drill DMG: {h.BiteSize}");
    }

    Console.WriteLine();

    Console.WriteLine("TOTAL MINED:");
    foreach (var pair in motherShip.WorkLog) {
      int total = pair.Value.Sum(r => r.AmountMined);
      Console.WriteLine($"{pair.Key}: {total}");
    }

    Console.WriteLine("\n[ENTER] - next turn | [R] - report");

    while (true) {
      var key = Console.ReadKey(true);

      if (key.Key == ConsoleKey.Enter) {
        ++turn;

        motherShip.ProcessTurn(activeAsteroids);

        if (turn % 5 == 0) {
          int count = random.Next(2, 6);
          for (int i = 0; i < count; i++) {
            activeAsteroids.Add(emitter.Spawn());
          }
        }

        for (int i = activeAsteroids.Count - 1; i >= 0; i--) {
          if (activeAsteroids[i].State == AsteroidState.Depleted) {
            emitter.Recycle(activeAsteroids[i]);
            activeAsteroids.RemoveAt(i);
          }
        }

        Console.Clear();

        Console.WriteLine($"=== TURN {turn} ===\n");

        Console.WriteLine("ASTEROIDS:");
        foreach (var a in activeAsteroids) {
          Console.WriteLine($"ID:{a.SpawnID} | {a.CurrentEchos}/{a.MaxEchos} | {a.State}");
        }

        Console.WriteLine();

        Console.WriteLine("HARVESTERS:");
        foreach (var h in motherShip.Harvesters) {
          Console.WriteLine($"{h.Name} | {h.Lvl} | {h.StateHarvest} | Cargo: {h.CargoCurrent}/{h.CargoCapacity} | Drill DMG: {h.BiteSize}");
        }

        Console.WriteLine();

        Console.WriteLine("TOTAL MINED:");
        foreach (var pair in motherShip.WorkLog) {
          int total = pair.Value.Sum(r => r.AmountMined);
          Console.WriteLine($"{pair.Key}: {total}");
        }

        Console.WriteLine("\n[ENTER] - next turn | [R] - report");
      }

      if (key.Key == ConsoleKey.R) {
        motherShip.PrintSummary();
      }

      if (turn % 15 == 0 && turn != 0) {
        motherShip.PrintFullLog();
      }
    }
  }
}
