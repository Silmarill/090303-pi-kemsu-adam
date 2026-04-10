using System;
using System.Collections.Generic;

class Program
{
    // Настройки симуляции (магические числа вынесены сюда)
    const int InitialPoolSize = 5;
    const int StartAsteroidCount = 3;
    const int SpawnEveryNChrons = 5;
    const int MinNewSpawns = 1;
    const int MaxNewSpawns = 3;

    static void Main()
    {
        AsteroidEmitter emitter = new AsteroidEmitter(InitialPoolSize);
        List<Asteroid> activeList = new List<Asteroid>();
        Random rnd = new Random();
        int chronCounter = 0;

        // Начальный спавн
        for (int i = 0; i < StartAsteroidCount; i++)
        {
            activeList.Add(emitter.Spawn());
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"--- ХРОН: {chronCounter} ---");
            Console.WriteLine($"Активно: {activeList.Count} | В пуле: {emitter.Available.Count}");
            Console.WriteLine("--------------------------------------------------");

            foreach (var a in activeList)
            {
                Console.WriteLine(a.ToString());
            }

            Console.WriteLine("\n[Enter] - Тик | [Esc] - Выход");
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Escape) break;
            if (key == ConsoleKey.Enter)
            {
                chronCounter++;

                ChroneManager.MakeChroneTick();

                if (chronCounter % SpawnEveryNChrons == 0)
                {
                    int count = rnd.Next(MinNewSpawns, MaxNewSpawns + 1);
                    for (int i = 0; i < count; i++)
                    {
                        activeList.Add(emitter.Spawn());
                    }
                }

                for (int i = activeList.Count - 1; i >= 0; i--)
                {
                    if (activeList[i].State == AsteroidState.Depleted)
                    {
                        emitter.Recycle(activeList[i]);
                        activeList.RemoveAt(i);
                    }
                }
            }
        }
    }
}