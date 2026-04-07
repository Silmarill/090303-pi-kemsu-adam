using AsteroidSimulation.Entity;
using AsteroidSimulation.ObjectPool;
using AsteroidSimulation.Observer;
using AsteroidSimulation.Common;
using System;
using System.Collections.Generic;

namespace AsteroidSimulation
{
    class Program
    {
        static void Main()
        {
            int chroneCounter = 0;
            int asteroidCount = 5;
            
            AsteroidEmitter uwuEmitter = new AsteroidEmitter(asteroidCount);
            List<Asteroid> activeAsteroids = new List<Asteroid>();
            Random rand = new Random();

            for (int spawnIndex = 0; spawnIndex < 3; ++spawnIndex)
            {
                activeAsteroids.Add(uwuEmitter.Spawn());
            }

            Console.WriteLine("Симуляция астероидов запущена. Нажмите Enter для шага и Esc для выхода");

            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.Escape)
                {
                    break;
                }

                if (key != ConsoleKey.Enter)
                {
                    continue;
                }

                Console.Clear();

                ++chroneCounter;
                Console.WriteLine($"Хрон №{chroneCounter}");

                // Уведомление об изменении времени тиков
                for (int tickIndex = 0; tickIndex < activeAsteroids.Count; ++tickIndex)
                {
                    activeAsteroids[tickIndex].OnChroneTick();
                }

                // Каждые 5 тиков спавнятся 1-3 новых астероида
                if (chroneCounter % 5 == 0)
                {
                    int toSpawn = rand.Next(1, 4);

                    for (int spawnIndex = 0; spawnIndex < toSpawn; ++spawnIndex)
                    {
                        activeAsteroids.Add(uwuEmitter.Spawn());
                    }

                    Console.WriteLine($"Появилось {toSpawn} астероидов");
                }

                // Астероиды со статусом Depleted удаляются и возвращаются в пул
                // Цикл работает от конца к началу, чтобы не пропустить объект и
                // не влиять на порядок объектов, которые ещё не проверены
                for (int removeIndex = activeAsteroids.Count - 1; removeIndex >= 0; --removeIndex)
                {
                    if (activeAsteroids[removeIndex].State == AsteroidState.Depleted)
                    {
                        uwuEmitter.Recycle(activeAsteroids[removeIndex]);
                        activeAsteroids.RemoveAt(removeIndex);
                    }
                }

                Console.WriteLine($"Активных объектов: {activeAsteroids.Count}");

                for (int activeIndex = 0; activeIndex < activeAsteroids.Count; ++activeIndex)
                {
                    Asteroid asteroid = activeAsteroids[activeIndex];

                    Console.WriteLine($"[CreateID:{asteroid.CreateID} | SpawnID:{asteroid.SpawnID}] " +
                                      $"Ресурс: {asteroid.CurrentEchos}/{asteroid.MaxEchos}");
                }
            }
        }
    } 
}