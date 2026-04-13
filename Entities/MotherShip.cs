using System;
using System.Collections.Generic;

public class MotherShip : IChroneListener {
  // Флот харвестеров
  public List<HarvesterShip> fleet;

  // Журнал работ (ключ — имя харвестера)
  public Dictionary<string, List<Report>> worklog;

  // Размер флота
  private const int fleetSize = 5;

  // Счетчик заданий
  private int globalJobCounter;

  // Список активных астероидов
  private List<Asteroid> activeAsteroids;

  // Конструктор
  public MotherShip() {
    string harvesterName;

    fleet = new List<HarvesterShip>();
    worklog = new Dictionary<string, List<Report>>();

    globalJobCounter = 0;
    activeAsteroids = null;

    // Инициализация флота харвестеров и добавление их в список слушателей хрона
    for (int harvesterIndex = 1; harvesterIndex <= fleetSize; ++harvesterIndex) {
      harvesterName = $"Harvester-{harvesterIndex:D2}";

      // Создание харвестера и установка ссылки на MotherShip
      HarvesterShip ship = new HarvesterShip(harvesterIndex, harvesterName);
      ship.SetMotherShip(this);

      // Добавление харвестера в флот и инициализация его журнала работ
      fleet.Add(ship);
      worklog[harvesterName] = new List<Report>();

      // Добавление харвестера в список слушателей хрона
      ChroneManager.AddListener(ship);
    }

    // Добавление MotherShip в список слушателей хрона для получения уведомлений о каждом тике времени
    ChroneManager.AddListener(this);
  }

  /*
   * Метод, вызываемый при каждом тике времени (ChroneTick), который отвечает за назначение целей харвестерам, находящимся в состоянии Idle.
   * Он проходит по каждому харвестеру и каждому активному астероиду, проверяя их состояния,
   * и если харвестер может начать добычу с астероида, он назначает его в качестве цели для харвестера.
  */
  public void OnChroneTick() {
    AssignTargets();
  }

  private void AssignTargets() {
    if (activeAsteroids == null) {
      return;
    }

    // Проход по каждому харвестеру в флоте
    for (int shipIndex = 0; shipIndex < fleet.Count; ++shipIndex) {
      HarvesterShip ship = fleet[shipIndex];

      // Проверка, находится ли харвестер в состоянии Idle. Если нет, переходим к следующему харвестеру
      if (ship.state != HarvesterState.Idle) {
        continue;
      }

      // Проход по каждому активному астероиду
      for (int asteroidIndex = 0; asteroidIndex < activeAsteroids.Count; ++asteroidIndex) {
        Asteroid asteroid = activeAsteroids[asteroidIndex];

        // Проверка, находится ли астероид в состоянии Idle. Если нет, переходим к следующему астероиду
        if (asteroid.state != AsteroidState.Idle) {
          continue;
        }

        // Проверка, может ли харвестер начать добычу с астероида. Если да, назначаем его в качестве цели для харвестера и переходим к следующему харвестеру
        if (ship.TryAssignTarget(asteroid)) {
          break;
        }
      }
    }
  }

  /*
   * Метод для установки списка активных астероидов, который будет использоваться для назначения целей харвестерам.
   * Он принимает список активных астероидов и сохраняет его в поле activeAsteroids для дальнейшего использования при назначении целей.
  */
  public void SetActiveAsteroids(List<Asteroid> activeAsteroids) {
    this.activeAsteroids = activeAsteroids;
  }

  // Метод для получения отчета о выполненной работе от харвестера. Он принимает имя харвестера, идентификатор спавна астероида и количество добытых эхосов.
  public void DeliverReport(HarvesterShip ship, Asteroid asteroid, int amountMined) {
    if (amountMined <= 0) {
      return;
    }

    globalJobCounter++;

    Report report = new Report(globalJobCounter, asteroid.spawnId, amountMined);

    string harvesterName = ship.name;

    worklog[harvesterName].Add(report);
  }

  public void PrintFullWorklog() {
    string harvesterName;

    Console.WriteLine("\n=== Full Worklog ===");

    // Получение списка имен харвестеров из журнала работ для итерации
    List<string> harvesterNames = new List<string>(worklog.Keys);

    // Проход по каждому харвестеру в журнале работ и печать его имени, количества отчетов и каждого отчета, связанного с ним
    for (int harvesterIndex = 0; harvesterIndex < harvesterNames.Count; ++harvesterIndex) {
      harvesterName = harvesterNames[harvesterIndex];
      List<Report> reports = worklog[harvesterName];

      Console.WriteLine($"Harvester: {harvesterName} ({reports.Count})");

      // Печать каждого отчета, связанного с текущим харвестером, для отображения подробной информации о каждом задании, выполненном этим харвестером
      for (int reportIndex = 0; reportIndex < reports.Count; ++reportIndex) {
        Console.WriteLine($"  {reports[reportIndex]}");
      }
    }

    Console.WriteLine("====================\n");
  }

  public void PrintSummary() {
    int totalMined;

    Console.WriteLine("\n=== Summary ===");

    // Проход по каждому харвестеру в флоте для суммирования количества добытых эхосов из всех отчетов, связанных с этим харвестером, а также количества добытых астероидов
    for (int shipIndex = 0; shipIndex < fleet.Count; ++shipIndex) {
      HarvesterShip ship = fleet[shipIndex];

      totalMined = 0;

      if (worklog.ContainsKey(ship.name)) {
        List<Report> reports = worklog[ship.name];

        // Суммирование количества добытых эхосов из всех отчетов, связанных с текущим харвестером, для получения общего количества добытых эхосов этим харвестером
        for (int reportIndex = 0; reportIndex < reports.Count; ++reportIndex) {
          totalMined += reports[reportIndex].amountMined;
        }
      }

      Console.WriteLine($"{ship.name}: {totalMined} Echos | Asteroids mined: {ship.asteroidsMined}");
    }
  }
}