using System;

namespace Asteroids {
	public enum HarvesterState {
		Idle,
		Mining
	}

	public class HarvesterShip {
		private static int _nextHarvesterId = 1;

		public int HarvesterID;
		public string Name;
		public int AsteroidsMined;
		public int CargoCapacity;
		public int CargoCurrent;
		public int BiteSize;
		public HarvesterState State;

		private Asteroid _currentAsteroid;
		private int _jobCounterForHarvester;

		public HarvesterShip(string nameForHarvester, int cargoCapacityForHarvester, int biteSizeForHarvester) {
			HarvesterID = _nextHarvesterId;
			_nextHarvesterId = _nextHarvesterId + 1;

			Name = nameForHarvester;
			CargoCapacity = cargoCapacityForHarvester;
			BiteSize = biteSizeForHarvester;
			CargoCurrent = 0;
			AsteroidsMined = 0;
			State = HarvesterState.Idle;
			_currentAsteroid = null;
			_jobCounterForHarvester = 0;
		}

		public bool IsIdle() {
			return State == HarvesterState.Idle;
		}

		public void StartMining(Asteroid asteroidForMining) {
			if (State != HarvesterState.Idle) {
				return;
			}
			if (asteroidForMining.State != AsteroidState.Idle) {
				return;
			}

			_currentAsteroid = asteroidForMining;
			_currentAsteroid.State = AsteroidState.Mining;
			State = HarvesterState.Mining;
		}

		public void Mine(MotherShip motherShipForReport) {
			if (State != HarvesterState.Mining) {
				return;
			}
			if (_currentAsteroid == null) {
				return;
			}

			int amountToMine = Math.Min(BiteSize, _currentAsteroid.CurrentEchos);
			amountToMine = Math.Min(amountToMine, CargoCapacity - CargoCurrent);

			if (amountToMine <= 0) {
				FinishMining(motherShipForReport);
				return;
			}

			_currentAsteroid.CurrentEchos = _currentAsteroid.CurrentEchos - amountToMine;
			CargoCurrent = CargoCurrent + amountToMine;

			if (_currentAsteroid.CurrentEchos <= 0) {
				_currentAsteroid.CurrentEchos = 0;
				_currentAsteroid.State = AsteroidState.Depleted;
				FinishMining(motherShipForReport);
				return;
			}

			if (CargoCurrent >= CargoCapacity) {
				FinishMining(motherShipForReport);
			}
		}

		private void FinishMining(MotherShip motherShipForReport) {
			if (_currentAsteroid == null) {
				return;
			}
			if (CargoCurrent > 0) {
				_jobCounterForHarvester = _jobCounterForHarvester + 1;
				Report newReport = new Report(_jobCounterForHarvester, _currentAsteroid.SpawnID, CargoCurrent);
				motherShipForReport.AddReport(Name, newReport);

				if (_currentAsteroid.State == AsteroidState.Depleted) {
					AsteroidsMined = AsteroidsMined + 1;
				}
			}

			if (_currentAsteroid.State != AsteroidState.Depleted) {
				_currentAsteroid.State = AsteroidState.Idle;
			}

			_currentAsteroid = null;
			CargoCurrent = 0;
			State = HarvesterState.Idle;
		}

		public void ForceFinishMining(MotherShip motherShipForReport) {
			if (State == HarvesterState.Mining && _currentAsteroid != null) {
				FinishMining(motherShipForReport);
			}
		}

		public void PrintInfo() {
			Console.WriteLine("Харвестер #{0} \"{1}\" | {2} | Груз:{3}/{4} | Добыл:{5}",
							HarvesterID, Name, State, CargoCurrent, CargoCapacity, AsteroidsMined);
		}
	}
}