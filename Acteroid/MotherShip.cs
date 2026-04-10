using System;
using System.Collections.Generic;

namespace Asteroids {
	public class MotherShip {
		private List<HarvesterShip> _harvesterFleet;
		private Dictionary<string, List<Report>> _worklog;
		public MotherShip(int harvesterCountForFleet, int cargoCapacityForHarvester, int biteSizeForHarvester) {

			_harvesterFleet = new List<HarvesterShip>();
			_worklog = new Dictionary<string, List<Report>>();

			for (int harvesterIndex = 0; harvesterIndex < harvesterCountForFleet; ++harvesterIndex) {
				string harvesterName = $"Harvester-{harvesterIndex + 1}";
				HarvesterShip newHarvester = new HarvesterShip(harvesterName, cargoCapacityForHarvester, biteSizeForHarvester);
				_harvesterFleet.Add(newHarvester);
			}
		}

		public List<HarvesterShip> GetHarvesterFleet() {
			return _harvesterFleet;
		}

		public void AddReport(string harvesterNameForReport, Report reportForAdding) {
			if (!_worklog.ContainsKey(harvesterNameForReport)) {
				_worklog[harvesterNameForReport] = new List<Report>();
			}
			_worklog[harvesterNameForReport].Add(reportForAdding);
		}

		public void PrintTotalMined() {
			Console.WriteLine("\n=== СУММАРНАЯ ДОБЫЧА ===");

			foreach (var kvp in _worklog) {
				int totalForHarvester = 0;
				foreach (Report singleReport in kvp.Value) {
					totalForHarvester = totalForHarvester + singleReport.AmountMined;
				}
				Console.WriteLine($"{kvp.Key}: {totalForHarvester} ед.");
			}
		}

		public void PrintFullWorklog() {
			Console.WriteLine("\n========== WORKLOG ==========");

			foreach (var kvp in _worklog) {
				Console.WriteLine($"\n--- {kvp.Key} ---");
				foreach (Report singleReport in kvp.Value) {
					Console.WriteLine($"  #{singleReport.JobNumber} | SpawnID:{singleReport.AsteroidSpawnID} | {singleReport.AmountMined}ед.");
				}
			}
			Console.WriteLine("=============================");
		}

		public void PrintHarvesterFleetStatus() {
			Console.WriteLine("\n=== ФЛОТ ХАРВЕСТЕРОВ ===");
			foreach (HarvesterShip currentHarvester in _harvesterFleet) {
				currentHarvester.PrintInfo();
			}
		}
	}
}