using System;
using System.Collections.Generic;
using System.Text;

public enum HarvesterState {
  Idle,
  Mining
}

namespace Asteroids {
  public class HarvesterShip {
    public int ID;
    public string Name;
    public int CargoCapacity;
    public int CargoCurrent;
    public int BiteSize;
    public int Lvl;

    public HarvesterState StateHarvest { get; set; }

    private Asteroid _targetAsteroid;
    private int _jobCounter = 0;
    private int _currentMined = 0;

    private static readonly string[] Names = new[]
    {
        "Вадим Б12", "Копала", "Артур Копатель", "Копатель офлайн",
        "Лютейший бурила", "Закопышь", "БРБРБР", "Потерявший Астероид",
        "Натуральный Бур", "Олег", "Друг Админа", "Копатель Копалкин",
        "Бурящий Небеса", "ААААААААААААА", "Без Баб", "БезБурый",
        "Два Бура Один Астероид", "Продам Гараж", "Где деньги взять", "ГойдаЛет 3000",
        "Сиамский сиамец", "Человек продавший астероид", "Solid Snake", "Big Boss",
        "Танк на бурах", "Человек Буривший Мир", "НеГей", "Шаролёт",
        "Черный", "Натуральный Шахтер", "Утилязатор", "Space Station 14",
        "Прямиком из Нанотрейзен", "НАНОТРЕЙЗЕН СОСААААТЬ!", "Слава Горлексу!", "Приколист из MI13",
        "Шкебедедопдоп головного мозга", "Сэр есть Сэр", "На буре вертел", "Ебоштепсель",
        "Бур длинной в метр", "Импотент", "Стальной бур - стальные яйца", "Я бурил - меня копали",
        "Истинный Буритель", "Забурил досмерти", "А куда копать?", "Срочник",
        "Огузок", "ПОМОГИТЕ Я ЗАСТРЯЛ!!!", "Крутое название", "КиррилУбиваторКосмосаИГалактикВсегоСущего3000"
    };

    private static int _idCounter = 0;
    private static Random random = new Random();

    public HarvesterShip() {
      ID = ++_idCounter;
      CargoCapacity = random.Next(100, 600);
      BiteSize = random.Next(1, 200);
      Name = Names[random.Next(Names.Length)];
      StateHarvest = HarvesterState.Idle;
      Lvl = (CargoCapacity / BiteSize);
    }

    public bool IsIdle => StateHarvest == HarvesterState.Idle;

    public void AssignAsteroid(Asteroid asteroid) {
      _targetAsteroid = asteroid;

      StateHarvest = HarvesterState.Mining;
      asteroid.State = AsteroidState.Mining;

      _currentMined = 0;
    }

    public Report Work() {
      if (StateHarvest != HarvesterState.Mining || _targetAsteroid == null)
        return null;

      int mined = Math.Min(BiteSize, _targetAsteroid.CurrentEchos);

      _targetAsteroid.CurrentEchos -= mined;
      CargoCurrent += mined;
      _currentMined += mined;

      if (_targetAsteroid.CurrentEchos <= 0) {
        _targetAsteroid.CurrentEchos = 0;
        _targetAsteroid.State = AsteroidState.Depleted;
      }

      if (CargoCurrent >= CargoCapacity || _targetAsteroid.State == AsteroidState.Depleted) {
        if (_targetAsteroid.State != AsteroidState.Depleted) {
          _targetAsteroid.State = AsteroidState.Idle;
        }

        var report = new Report {
          JobNumber = ++_jobCounter,
          AsteroidSpawnID = _targetAsteroid.SpawnID,
          AmountMined = _currentMined
        };

        CargoCurrent = 0;
        StateHarvest = HarvesterState.Idle;
        _targetAsteroid = null;

        CargoCapacity += random.Next(1, 70);
        BiteSize += random.Next(1, 30);
        Lvl += 1;

        return report;
      }
      return null;
    }
  }
}
