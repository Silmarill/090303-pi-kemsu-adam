using AsteroidSimulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation.Models {
  public class Asteroid : IChroneListener {
    private static int _nextCreateId = 1;
    private static readonly Random _random = new Random();

    // Поля экземпляра (разные для каждого астероида)
    public int DepletionAmount { get; private set; }
    public int MinEchos { get; private set; }

    public int CurrentEchos { get; private set; }
    public int MaxEchos { get; private set; }
    public AsteroidState State { get; private set; }
    public int SpawnID { get; private set; }
    public int CreateID { get; private set; }

    // Конструктор
    public Asteroid() {
      // Уникальные значения для каждого астероида
      DepletionAmount = _random.Next(50, 151);  // от 50 до 150 (вокруг 100)
      MinEchos = 0;

      MaxEchos = _random.Next(100, 1001);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;

      // Уникальные ID
      CreateID = ++_nextCreateId;
      SpawnID = 0;
    }

    // Сброс астероида при возврате в пул
    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    // Логика деградации на каждом хроне
    public void OnChroneTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos -= DepletionAmount;
        if (CurrentEchos <= MinEchos) {
          CurrentEchos = MinEchos;
          State = AsteroidState.Depleted;
        }
      }
    }

    // Установка ID спавна (вызывается эмиттером)
    public void SetSpawnID(int id) {
      SpawnID = id;
    }

    // Для удобного вывода информации
    public override string ToString() {
      return $"Asteroid [CreateID: {CreateID}, SpawnID: {SpawnID}, " +
             $"MaxEchos: {MaxEchos}, CurrentEchos: {CurrentEchos}, " +
             $"DepletionAmount: {DepletionAmount}, State: {State}]";
    }
  }

}
