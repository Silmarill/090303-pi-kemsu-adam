using AsteroidSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation.Managers {
  public class MotherShip {

    public List<HarvesterShip> Fleet;

    // Журнал работ: ключ - имя харвестера, значение - список отчётов
    public Dictionary<string, List<Report>> Worklog;

    // Счётчик заданий для каждого харвестера
    private Dictionary<string, int> _jobCounter;

    public MotherShip() {
      Fleet = new List<HarvesterShip>();
      Worklog = new Dictionary<string, List<Report>>();
      _jobCounter = new Dictionary<string, int>();

      // Создаём 5 харвестеров с именами
      string[] names = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };
      foreach (string name in names) {
        HarvesterShip ship = new HarvesterShip(name);
        Fleet.Add(ship);
        Worklog[ship.Name] = new List<Report>();
        _jobCounter[ship.Name] = 0;
      }
    }

    // Стабилизация зоны (включается при создании станции)
    public void EnableZoneStabilization() {
      Asteroid.IsZoneStable = true;
      Console.WriteLine("[MotherShip] Стабилизация зоны АКТИВНА. Астероиды не деградируют.");
    }

    // Отключение стабилизации (если понадобится)
    public void DisableZoneStabilization() {
      Asteroid.IsZoneStable = false;
      Console.WriteLine("[MotherShip] Стабилизация зоны ОТКЛЮЧЕНА.");
    }

    // Разгрузка харвестера на станции
    public void UnloadHarvester(HarvesterShip harvester) {
      if (harvester.State != HarvesterState.Mining) {
        return;
      }

      // Увеличиваем счётчик заданий для этого харвестера
      ++_jobCounter[harvester.Name];
      int jobNumber = _jobCounter[harvester.Name];

      // Создаём отчёт и разгружаем
      Report report = harvester.Unload(jobNumber);

      // Добавляем отчёт в журнал
      Worklog[harvester.Name].Add(report);

      Console.WriteLine($"[MotherShip] {harvester.Name} разгрузился. Добыто:{report.AmountMined} ед. с астероида SpawnID={report.AsteroidSpawnID}");
    }

    // Вывод суммарной добычи по каждому харвестеру
    public void PrintTotalMined() {
      Console.WriteLine("\n=== СУММАРНАЯ ДОБЫЧА ПО ХАРВЕСТЕРАМ ===");
      foreach (var harvester in Fleet) {
        int total = 0;
        if (Worklog.ContainsKey(harvester.Name)) {
          total = Worklog[harvester.Name].Sum(report => report.AmountMined);
        }
        Console.WriteLine($"{harvester.Name}: {total} ед. (всего астероидов:{harvester.AsteroidsMined})");
      }
      Console.WriteLine("----------------------------------------\n");
    }

    // Вывод полного журнала работ (все отчёты)
    public void PrintFullWorklog() {
      Console.WriteLine("\n==================== ПОЛНЫЙ WORKLOG ====================");
      foreach (var harvester in Fleet) {
        Console.WriteLine($"\n--- {harvester.Name} ---");
        if (Worklog[harvester.Name].Count == 0) {
          Console.WriteLine("  (нет отчётов)");
        } else {
          foreach (Report report in Worklog[harvester.Name]) {
            Console.WriteLine($"  {report}");
          }
        }
      }
      Console.WriteLine("=======================================================\n");
    }

    // Получить свободного харвестера (Idle)
    public HarvesterShip GetIdleHarvester() {
      return Fleet.FirstOrDefault(harvester => harvester.State == HarvesterState.Idle);
    }

    // Получить список всех харвестеров
    public List<HarvesterShip> GetAllHarvesters() {
      return Fleet;
    }
  }
}
