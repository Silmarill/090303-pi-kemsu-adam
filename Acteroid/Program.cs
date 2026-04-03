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

			AsteroidEmitter asteroidEmitter = new AsteroidEmitter(poolInitialSize);
			List<Asteroid> activeAsteroidsList = new List<Asteroid>();
			int chronTickCounter = 0;
			Random randomGenerator = new Random();

			for (int asteroidIndex = 0; asteroidIndex < startingAsteroidsCount; asteroidIndex++) {
				Asteroid newAsteroid = asteroidEmitter.Spawn();
				activeAsteroidsList.Add(newAsteroid);
				ChronoManager.AddListener(newAsteroid);
			}

			Console.WriteLine($"=== СИМУЛЯЦИЯ АСТЕРОИДОВ ===" +
												$"Enter - следующий хрон" +
												$"Esc - выход" +
												$"=================================");

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

				Console.WriteLine("\nХрон #{0}", chronTickCounter);
				Console.WriteLine("Активных астероидов: {0}", activeAsteroidsList.Count);
				Console.WriteLine("Свободно в пуле: {0}", asteroidEmitter.GetAvailableCount());
				Console.Write("\nНажмите Enter для следующего хрона или Esc для выхода: ");

				ConsoleKeyInfo pressedKey = Console.ReadKey(true);

				if (pressedKey.Key == ConsoleKey.Escape) {
					isProgramRunning = false;
				} else if (pressedKey.Key == ConsoleKey.Enter) {
					chronTickCounter = chronTickCounter + 1;

					ChronoManager.MakeChronTick();

					if (chronTickCounter % spawnInterval == 0) {
						int newAsteroidsAmount = randomGenerator.Next(minSpawnAmount, maxSpawnAmount + 1);
						Console.WriteLine("\n>>> Хрон #{0}: появилось {1} новых астероидов!", chronTickCounter, newAsteroidsAmount);

						for (int asteroidIndex = 0; asteroidIndex < newAsteroidsAmount; asteroidIndex++) {
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

					foreach (Asteroid depletedAsteroid in depletedAsteroidsList) {
						activeAsteroidsList.Remove(depletedAsteroid);
						ChronoManager.RemoveListener(depletedAsteroid);
						asteroidEmitter.Recycle(depletedAsteroid);
						Console.WriteLine("Астероид #{0} истощен и возвращен в пул", depletedAsteroid.CreateID);
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