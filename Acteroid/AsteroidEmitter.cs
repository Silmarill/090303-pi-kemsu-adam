using System;
using System.Collections.Generic;

namespace Asteroids {
	public class AsteroidEmitter {
		private Queue<Asteroid> _availableAsteroids = new Queue<Asteroid>();

		public AsteroidEmitter(int initialPoolSize) {
			for (int asteroidIndex = 0; asteroidIndex < initialPoolSize; asteroidIndex++) {
				Asteroid newAsteroid = new Asteroid();
				_availableAsteroids.Enqueue(newAsteroid);
			}
		}

		public Asteroid Spawn() {
			if (_availableAsteroids.Count == 0) {
				Console.WriteLine("Предупреждение: пул пуст, создан новый астероид");
				return new Asteroid();
			}
			return _availableAsteroids.Dequeue();
		}

		public void Recycle(Asteroid usedAsteroid) {
			usedAsteroid.Reset();
			_availableAsteroids.Enqueue(usedAsteroid);
		}

		public int GetAvailableCount() {
			return _availableAsteroids.Count;
		}
	}
}