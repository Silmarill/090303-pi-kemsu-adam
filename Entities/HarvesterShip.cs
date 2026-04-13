using System;

public class HarvesterShip : IChroneListener {
  public readonly int id;
  public readonly string name;
  public int asteroidsMined;

  // Для определения вместимости и размера "куса" при добыче
  public int cargoCapacity;
  public int biteSize;

  // Текущее количество ресурса в трюме и состояние добычи
  public int cargoCurrent;

  // Состояние добычи: Idle (ожидание), Mining (добыча)
  public HarvesterState state;

  // Текущий астероид, который добывает корабль
  private Asteroid currentTarget;

  // Ссылка на MotherShip для отправки отчетов о добыче
  private MotherShip motherShip;

  // Конструктор для инициализации харвестера с уникальным ID и именем
  public HarvesterShip(int id, string name) {
    this.id = id;
    this.name = name;

    asteroidsMined = 0;
    cargoCurrent = 0;
    state = HarvesterState.Idle;

    cargoCapacity = 500;
    biteSize = 50;

    currentTarget = null;
  }

  // Метод для установки ссылки на MotherShip, чтобы харвестер мог отправлять отчеты о добыче
  public void SetMotherShip(MotherShip ship) {
    motherShip = ship;
  }

  // Метод, вызываемый при каждом тике времени (ChroneTick), который отвечает за выполнение добычи, если харвестер находится в состоянии Mining
  public void OnChroneTick() {
    if (state == HarvesterState.Mining && currentTarget != null) {
      Mine(currentTarget);
    }
  }

  /*
   * Метод для выполнения добычи ресурса с астероида. Он проверяет состояние астероида и количество оставшегося ресурса, а также обновляет текущий груз в трюме харвестера.
   * Если астероид истощен или трюм полон, вызывается метод FinishMining для завершения добычи.
  */
  public void Mine(Asteroid asteroid) {
    if (asteroid == null || asteroid.state != AsteroidState.Mining || asteroid.currentEchos <= 0) {
      FinishMining();
      return;
    }

    // Вычисление количества ресурса, которое можно добыть за один кусок, учитывая оставшееся количество ресурса на астероиде и вместимость трюма харвестера
    int biteAmount = Math.Min(biteSize, asteroid.currentEchos);

    asteroid.currentEchos -= biteAmount;
    cargoCurrent += biteAmount;

    // Проверка, истощен ли астероид или трюм харвестера полон, и завершение добычи, если это так
    if (asteroid.currentEchos <= 0) {
      asteroid.currentEchos = 0;
      asteroid.state = AsteroidState.Depleted;
      FinishMining();
    } else if (cargoCurrent >= cargoCapacity) {
      FinishMining();
    }
  }

  /*
   * Метод для завершения добычи, который обновляет состояние харвестера и астероида, а также отправляет отчет о добыче в MotherShip, если был добыт ресурс.
   * Он также сбрасывает текущий груз в трюме и возвращает харвестер в состояние Idle.
  */
  private void FinishMining() {
    if (currentTarget != null) {
      if (cargoCurrent > 0) {
        asteroidsMined++;

        // Отправляется отчет о добыче в MotherShip, если был добыт ресурс
        if (motherShip != null) {
          motherShip.DeliverReport(this, currentTarget, cargoCurrent);
        }
      }

      // Сброс состояния астероида и текущей цели
      currentTarget = null;
    }

    // Сброс текущего груза в трюме и возвращение харвестера в состояние Idle
    cargoCurrent = 0;
    state = HarvesterState.Idle;
  }

  /*
   * Метод для попытки назначения астероида в качестве цели для добычи.
   * Он проверяет, что харвестер находится в состоянии Idle и что астероид также находится в состоянии Idle,
   * прежде чем назначить его в качестве текущей цели и изменить состояние обоих на Mining.
   * Если назначение успешно, метод возвращает true; в противном случае возвращает false.
  */
  public bool TryAssignTarget(Asteroid asteroid) {
    if (state != HarvesterState.Idle || asteroid == null || asteroid.state != AsteroidState.Idle) {
      return false;
    }

    currentTarget = asteroid;

    // Назначение астероида в качестве цели для добычи и изменение состояния обоих на Mining
    asteroid.state = AsteroidState.Mining;
    state = HarvesterState.Mining;

    return true;
  }

  /*
   * Переопределение метода ToString для предоставления удобного формата отображения информации о харвестере,
   * включая его ID, имя, состояние, текущий груз в трюме и количество добытых астероидов.
  */
  public override string ToString() {
    return $"Harvester [{id:D2}] {name} | Status: {state} | Cargo: {cargoCurrent}/{cargoCapacity} | Mined: {asteroidsMined}";
  }
}