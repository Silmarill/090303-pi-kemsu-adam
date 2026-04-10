using System;

// :0 new
namespace Asteroids {
	public class Report {
		public int JobNumber;
		public int AsteroidSpawnID;
		public int AmountMined;

		public Report(int jobNumberForReport, int asteroidSpawnIdForReport, int amountMinedForReport) {
			JobNumber = jobNumberForReport;
			AsteroidSpawnID = asteroidSpawnIdForReport;
			AmountMined = amountMinedForReport;
		}
	}
}