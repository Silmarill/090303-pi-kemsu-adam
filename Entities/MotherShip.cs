using System;
using System.Collections.Generic;

public class MotherShip : IChroneListener {
  // Флот харвестеров и журнал работ
  public List<HarvesterShip> Fleet { get; private set; } = new List<HarvesterShip>();
  // Ключ — имя харвестера, значение - список его отчётов
  public Dictionary<string, List<Report>> Worklog { get; private set; } = new Dictionary<string, List<Report>>();
  // Список активных астероидов (будет передаваться из Program)
  private const int FLEET_SIZE = 5;

  // Для нумерации заданий
  private int _globalJobCounter = 0;

  // Конструктор для инициализации флота харвестеров и подписки на хроны
  public MotherShip() {
    for (int i = 1; i <= FLEET_SIZE; i++) {
      string name = $"Harvester-{i:D2}";
      var ship = new HarvesterShip(i, name);
      Fleet.Add(ship);
      Worklog[name] = new List<Report>();

      // Харвестеры подписываются на хроны
      ChroneManager.AddListener(ship);
    }

    ChroneManager.AddListener(this);
  }

  /*
   * Метод, вызываемый при каждом тике хронометра
   * На 15-м хроне выводится полный Worklog (будет вызвано из Program)
  */
  public void OnChroneTick() {
    // Логика назначения целей свободным харвестерам
    AssignTargets();
  }

  // Метод для назначения целей свободным харвестерам
  private void AssignTargets() {
    // Cвободные харвестеры берут первый Idle астероид
    foreach (var ship in Fleet) {
      if (ship.State != HarvesterState.Idle) {
        continue;
      }
    }
  }

  public void SetActiveAsteroids(List<Asteroid> activeAsteroids) {
    // Пока пусто
  }

  // Метод для разгрузки и создания отчёта (вызывается из HarvesterShip при необходимости)
  public void DeliverReport(HarvesterShip ship, int asteroidSpawnID, int amountMined) {
    if (amountMined <= 0) {
      return;
    }

    _globalJobCounter++;
    var report = new Report(_globalJobCounter, asteroidSpawnID, amountMined);

    string key = ship.Name;
    if (Worklog.ContainsKey(key)) {
      Worklog[key].Add(report);
    }
  }

  // Метод для вывода полного журнала работ
  public void PrintFullWorklog() {
    Console.WriteLine("\n=== ПОЛНЫЙ ЖУРНАЛ РАБОТ (Worklog) ===");
    foreach (var entry in Worklog) {
      Console.WriteLine($"Харвестер: {entry.Key} ({entry.Value.Count} отчётов)");
      foreach (var r in entry.Value) {
        Console.WriteLine($"   {r}");
      }
    }
    Console.WriteLine("====================================\n");
  }

  public void PrintSummary() {
    Console.WriteLine("\n=== СУММАРНАЯ ДОБЫЧА ===");
    foreach (var ship in Fleet) {
      int totalMined = 0;
      if (Worklog.ContainsKey(ship.Name)) {
        foreach (var r in Worklog[ship.Name]) {
          totalMined += r.AmountMined;
        }
      }
      Console.WriteLine($"{ship.Name}: {totalMined} Echos | Астероидов добыто: {ship.AsteroidsMined}");
    }
  }
}