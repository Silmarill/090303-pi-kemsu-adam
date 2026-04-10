using System;
using System.Collections.Generic;

namespace Asteroids {
	internal class Program {
		static void Main(string[] args) {
			int poolInitialSize = 5;
			int startingAsteroidsCount = 3;
			int spawnInterval = 5;
			int minSpawnAmount = 1;
			int maxSpawnAmount = 3;
			int harvesterFleetSize = 5;//
			int harvesterCargoCapacity = 500;//
			int harvesterBiteSize = 50;//
			int worklogPrintInterval = 15;//

			AsteroidEmitter asteroidEmitter = new AsteroidEmitter(poolInitialSize);
			MotherShip motherStation = new MotherShip(harvesterFleetSize, harvesterCargoCapacity, harvesterBiteSize);//
			List<HarvesterShip> harvesterFleet = motherStation.GetHarvesterFleet();//
			List<Asteroid> activeAsteroidsList = new List<Asteroid>();
			int chronTickCounter = 0;
			Random randomGenerator = new Random();

			for (int asteroidIndex = 0; asteroidIndex < startingAsteroidsCount; ++asteroidIndex) {
				Asteroid newAsteroid = asteroidEmitter.Spawn();
				activeAsteroidsList.Add(newAsteroid);
				ChronoManager.AddListener(newAsteroid);
			}

			Console.WriteLine($"=== СИМУЛЯЦИЯ АСТЕРОИДОВ ===\n" +
												$"Enter - следующий хрон\n" +
												$"R - суммарная добыча\n" +/////////////////////
												$"Esc - выход\n" +
												$"=================================\n");

			bool isProgramRunning = true;

			while (isProgramRunning) {
				Console.Clear();
				Console.WriteLine("=== АКТИВНЫЕ АСТЕРОИДЫ ===\n");

				if (activeAsteroidsList.Count == 0) {
					Console.WriteLine("Нет активных астероидов");
				} else {
					foreach (Asteroid currentAsteroid in activeAsteroidsList) {
						currentAsteroid.PrintInfo();
					}
				}

				motherStation.PrintHarvesterFleetStatus(); //

				Console.WriteLine($"\nХрон #{0}", chronTickCounter);
				Console.WriteLine($"Активных астероидов: {0}", activeAsteroidsList.Count);
				Console.WriteLine($"Свободно в пуле: {0}", asteroidEmitter.GetAvailableCount());

				motherStation.PrintTotalMined();
				
				Console.Write("\nНажмите Enter для следующего хрона, R для отчёта, Esc для выхода: ");

				ConsoleKeyInfo pressedKey = Console.ReadKey(true);

				if (pressedKey.Key == ConsoleKey.Escape) {
					isProgramRunning = false;
				} else if (pressedKey.Key == ConsoleKey.R) {
					Console.Clear();
					motherStation.PrintTotalMined();
					motherStation.PrintFullWorklog();
					Console.WriteLine("\nНажмите любую клавишу для продолжения...");
					Console.ReadKey(true);
				} else if (pressedKey.Key == ConsoleKey.Enter) {
					chronTickCounter = chronTickCounter + 1;

					ChronoManager.MakeChronTick();

					// каждый харвестер, который не в Idle, делает укус астероида
					foreach (HarvesterShip currentHarvester in harvesterFleet) {
						if (!currentHarvester.IsIdle()) {
							currentHarvester.Mine(motherStation);
						}
					}

					// собираем список всех свободных (Idle) астероидов
					List<Asteroid> idleAsteroids = new List<Asteroid>();
					foreach (Asteroid currentAsteroid in activeAsteroidsList) {
						if (currentAsteroid.State == AsteroidState.Idle) {
							idleAsteroids.Add(currentAsteroid);
						}
					}

					// назначаем свободных харвестеров на свободные астероиды
					foreach (HarvesterShip currentHarvester in harvesterFleet) {
						if (currentHarvester.IsIdle() && idleAsteroids.Count > 0) {
							Asteroid targetAsteroid = idleAsteroids[0];
							idleAsteroids.RemoveAt(0);
							currentHarvester.StartMining(targetAsteroid);
						}
					}

					if (chronTickCounter % spawnInterval == 0) {
						int newAsteroidsAmount = randomGenerator.Next(minSpawnAmount, maxSpawnAmount + 1);
						Console.WriteLine("\n>>> Хрон #{0}: появилось {1} новых астероидов!", chronTickCounter, newAsteroidsAmount);
						for (int asteroidIndex = 0; asteroidIndex < newAsteroidsAmount; ++asteroidIndex) {
							Asteroid newAsteroid = asteroidEmitter.Spawn();
							activeAsteroidsList.Add(newAsteroid);
							ChronoManager.AddListener(newAsteroid);
						}
					}

					List<Asteroid> depletedAsteroidsList = new List<Asteroid>();

					foreach (Asteroid currentAsteroid in activeAsteroidsList) {
						if (currentAsteroid.State == AsteroidState.Depleted) {
							depletedAsteroidsList.Add(currentAsteroid);
						}
					}

					// если астероид истощён - принудительно завершаем добычу харвестера
					foreach (Asteroid depletedAsteroid in depletedAsteroidsList) {
						foreach (HarvesterShip currentHarvester in harvesterFleet) {
							currentHarvester.ForceFinishMining(motherStation);
						}
					}

					foreach (Asteroid depletedAsteroid in depletedAsteroidsList) {
						activeAsteroidsList.Remove(depletedAsteroid);
						ChronoManager.RemoveListener(depletedAsteroid);
						asteroidEmitter.Recycle(depletedAsteroid);
						Console.WriteLine("Астероид #{0} истощен и возвращен в пул", depletedAsteroid.CreateID);
					}

					// на каждом 15-м хроне выводим полный worklog
					if (chronTickCounter % worklogPrintInterval == 0 && chronTickCounter > 0) {
						Console.WriteLine($"\n*** ХРОН #{chronTickCounter} - ПОЛНЫЙ ЖУРНАЛ РАБОТ ***");
						motherStation.PrintFullWorklog();
						Console.WriteLine("\nНажмите любую клавишу для продолжения...");
						Console.ReadKey(true);
					}

					if (depletedAsteroidsList.Count > 0) {
						Console.WriteLine("\nНажмите любую клавишу для продолжения...");
						Console.ReadKey(true);
					}
				}
			}

			Console.WriteLine("\nПрограмма завершена.");
		}
	}
}