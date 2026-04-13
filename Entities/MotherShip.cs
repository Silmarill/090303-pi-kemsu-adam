using System;
using System.Collections.Generic;

public class MotherShip : IChroneListener {
  // Флот харвестеров
  public List<HarvesterShip> fleet { get; private set; }

  // Журнал работ (ключ — имя харвестера)
  public Dictionary<string, List<Report>> worklog { get; private set; }

  // Размер флота
  private int fleetSize;

  // Счетчик заданий
  private int globalJobCounter;

  // Конструктор
  public MotherShip() {
    HarvesterShip ship;
    string shipName;

    fleet = new List<HarvesterShip>();
    worklog = new Dictionary<string, List<Report>>();

    fleetSize = 5;
    globalJobCounter = 0;

    for (int harvesterIndex = 1; harvesterIndex <= fleetSize; ++harvesterIndex) {
      shipName = $"Harvester-{harvesterIndex:D2}";

      ship = new HarvesterShip(harvesterIndex, shipName);

      fleet.Add(ship);
      worklog[shipName] = new List<Report>();

      // Подписка харвестера на хроны
      ChroneManager.AddListener(ship);
    }

    // Подписка материнского корабля
    ChroneManager.AddListener(this);
  }

  // Обработка хрона
  public void OnChroneTick() {
    AssignTargets();
  }

  // Назначение целей харвестерам
  private void AssignTargets() {
    HarvesterShip ship;

    for (int shipIndex = 0; shipIndex < fleet.Count; ++shipIndex) {
      ship = fleet[shipIndex];

      if (ship.State != HarvesterState.Idle) {
        continue;
      }
    }
  }

  // Передача астероидов материнскому кораблю
  public void SetActiveAsteroids(List<Asteroid> activeAsteroids) {
    // Пока пусто
  }

  // Отчёт
  public void DeliverReport(HarvesterShip ship, int asteroidSpawnId, int amountMined) {
    string shipName = ship.Name;

    // Если ничего не было добыто, отчёт не создаётся
    if (amountMined <= 0) {
      return;
    }

    // Увеличение глобального счётчика заданий
    globalJobCounter++;

    Report report = new Report(globalJobCounter, asteroidSpawnId, amountMined);

    // Сохранение отчёта в журнале
    if (worklog.ContainsKey(shipName)) {
      worklog[shipName].Add(report);
    }
  }

  // Вывод журнала
  public void PrintFullWorklog() {
    Console.WriteLine("\n=== Full worklog ===");

    // Получение списка имён харвестеров для упорядоченного вывода
    List<string> harvesterNames = new List<string>(worklog.Keys);

    // Сортировка имён харвестеров по алфавиту
    for (int harvesterIndex = 0; harvesterIndex < harvesterNames.Count; ++harvesterIndex) {
      string currentHarvesterName = harvesterNames[harvesterIndex];
      List<Report> reports = worklog[currentHarvesterName];

      Console.WriteLine($"Harvester: {currentHarvesterName} ({reports.Count} reports)");

      // Вывод отчётов для текущего харвестера
      for (int reportIndex = 0; reportIndex < reports.Count; ++reportIndex) {
        Console.WriteLine($"  {reports[reportIndex]}");
      }
    }

    Console.WriteLine("====================================\n");
  }

  // Вывод суммарной информации о добыче каждого харвестера
  public void PrintSummary() {
    int totalMined = 0;

    Console.WriteLine("\n=== Summary ===");

    // Вывод информации для каждого харвестера
    for (int shipIndex = 0; shipIndex < fleet.Count; ++shipIndex) {
      HarvesterShip ship = fleet[shipIndex];

      // Подсчёт общего количества добытых эхов для текущего харвестера
      if (worklog.ContainsKey(ship.Name)) {
        List<Report> reports = worklog[ship.Name];

        // Подсчёт общего количества добытых эхов для текущего харвестера
        for (int reportIndex = 0; reportIndex < reports.Count; ++reportIndex) {
          totalMined += reports[reportIndex].amountMined;
        }
      }

      Console.WriteLine($"{ship.Name}: {totalMined} Echos | Asteroids mined: {ship.AsteroidsMined}");
    }
  }
}