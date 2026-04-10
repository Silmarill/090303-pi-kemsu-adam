using System;
using System.Collections.Generic;

namespace Asteroid
{
  class Program {
    static void Main() {
      AsteroidEmitter emitter = new AsteroidEmitter(5);
      List<Asteroid> active = new List<Asteroid>();
      Random rnd = new Random();

      int firstAstIndex;
      firstAstIndex = 3;

      int lifeMove;
      lifeMove = 5;

      int lifeCycleStart;
      lifeCycleStart = 1;

      int lifeCycleEnd;
      lifeCycleEnd = 4;

      int indexOffset;
      indexOffset = 1;

      for (int index = 0; index < firstAstIndex; ++index) {
        Asteroid firstAst = emitter.Spawn();
        ChronManager.AddListener(firstAst);
        active.Add(firstAst);
      }

      Console.WriteLine(
        "=== ASTEROID SIMULATION ===" +
        "Press ENTER - next chron, ESC - exit\n"
        );

      while (true) {
        ConsoleKeyInfo key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape) break;

        if (key.Key != ConsoleKey.Enter) continue;

        ChronManager.MakeChronTick();
        int chron = ChronManager.GetCurrentChron();

        if (chron % lifeMove == 0 && chron > 0) {
          int count = rnd.Next(lifeCycleStart, lifeCycleEnd);
          Console.WriteLine($"\n>>> Chron {chron}: +{count} new asteroids");

          for (int index = 0; index < count; ++index) {
            Asteroid firstAst = emitter.Spawn();
            ChronManager.AddListener(firstAst);
            active.Add(firstAst);
          }
        }

        List<Asteroid> toRemove = new List<Asteroid>();
        foreach (Asteroid firstAst in active) {
          if (firstAst.state == AsteroidState.Depleted) {
            toRemove.Add(firstAst);
          }
        }

        foreach (Asteroid firstAst in toRemove) {
          ChronManager.RemoveListener(firstAst);
          emitter.Recycle(firstAst);
          active.Remove(firstAst);
        }

        Console.Clear();
        Console.WriteLine(
          $"=== CHRON {chron} ===" +
          $"Active: {active.Count}, Pool: {emitter.AvailableCount()}\n"
          );

        for (int index = 0; index < active.Count; ++index) {
          Console.WriteLine($"  {index + indexOffset}. {active[index]}");
        }

        Console.WriteLine("\nPress ENTER for next chron, ESC to exit");
      }
    }
  }
}
