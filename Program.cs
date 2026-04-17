using AsteroidSimulation.Entity;
using AsteroidSimulation.ObjectPool;
using AsteroidSimulation.Observer;
using System;
using System.Collections.Generic;

namespace AsteroidSimulation {
    class Program {
        static void Main() {
            //int chroneCounter = 0;
            int asteroidCount = 5;

            AsteroidEmitter uwuEmitter = new AsteroidEmitter(asteroidCount);
            List<Asteroid> activeAsteroids = new List<Asteroid>();
            Random rand = new Random();

            MotherShip.InitializeFleet();//sss

            for (int spawnIndex = 0; spawnIndex < 3; ++spawnIndex) {
                activeAsteroids.Add(uwuEmitter.Spawn());
            }

            Console.WriteLine("Симуляция астероидов запущена. Нажмите Enter для шага, R - общая добыча, Esc для выхода");

            while (true) {
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.Escape) {
                    break;
                }

                if (key == ConsoleKey.R) {
                    Console.WriteLine($"\n[Матриарх] Общая добыча флота: {MotherShip.ResourcesMined} Echos");
                    continue;
                }

                if (key != ConsoleKey.Enter) {
                    continue;
                }

                Console.Clear();

                ChroneManager.MakeChroneTick();

                //++chroneCounter;

                Console.WriteLine($"Хрон №{ChroneManager.CurrentChrone}");

                MotherShip.AssignTasks(activeAsteroids);

                /* Уведомление об изменении времени тиков
                for (int tickIndex = 0; tickIndex < activeAsteroids.Count; ++tickIndex)
                {
                    activeAsteroids[tickIndex].OnChroneTick();
                }*/

                // Каждые 5 тиков спавнятся 1-3 новых астероида
                if (ChroneManager.CurrentChrone % 5 == 0 && activeAsteroids.Count < 15) {
                    int toSpawn = rand.Next(1, 4);

                    for (int spawnIndex = 0; spawnIndex < toSpawn; ++spawnIndex) {
                        activeAsteroids.Add(uwuEmitter.Spawn());
                    }

                    Console.WriteLine($"Появилось {toSpawn} астероидов");
                }

                // Астероиды со статусом Depleted удаляются и возвращаются в пул
                // Цикл работает от конца к началу, чтобы не пропустить объект и
                // не влиять на порядок объектов, которые ещё не проверены
                for (int removeIndex = activeAsteroids.Count - 1; removeIndex >= 0; --removeIndex) {
                    if (activeAsteroids[removeIndex].State == AsteroidState.Depleted) {
                        uwuEmitter.Recycle(activeAsteroids[removeIndex]);
                        activeAsteroids.RemoveAt(removeIndex);
                    }
                }

                if (ChroneManager.CurrentChrone % 15 == 0) {
                    MotherShip.ShowFullWorklog();
                }

                Console.WriteLine($"Активных объектов: {activeAsteroids.Count}");

                for (int activeIndex = 0; activeIndex < activeAsteroids.Count; ++activeIndex) {
                    Asteroid asteroid = activeAsteroids[activeIndex];

                    Console.WriteLine($"[CreateID:{asteroid.CreateID} | SpawnID:{asteroid.SpawnID}] " +
                                      $"Ресурс: {asteroid.CurrentEchos}/{asteroid.MaxEchos} | Статус: {asteroid.State}");
                }
            }
        }
    }
}