using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Asteroids {
	internal class Asteroid {

		public int MaxEchos = random.Next(100, 1000);
		public int CurrentEchos;
		public int SpawnID;
		public int CreateID;

		private static Random random = new Random();
		public enum AsteroidState {
			efff = 1,
			Depleted = 2
		}
		public AsteroidState State { get; set; }

		public void Reset() {
			CurrentEchos = MaxEchos;
			State = AsteroidState.efff;
		}

		public void OnChonTick() {
			if (State == AsteroidState.efff) {
				CurrentEchos -= 100;
				if (CurrentEchos < 0 || CurrentEchos == 0) {
					CurrentEchos = 0;
					State = AsteroidState.Depleted;
				}
			}
		}

	}
}