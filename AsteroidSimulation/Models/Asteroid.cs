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

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;
    public int DepletionAmount;
    public int MinEchos;

    private int MinDepletionAmount = 50;
    private int MaxDepletionAmount = 150;
    private int DefaultMinEchos = 0;
    private int MinMaxEchos = 100;
    private int MaxMaxEchos = 1001;

    // Конструктор
    public Asteroid() {
      // Уникальные значения для каждого астероида
      DepletionAmount = _random.Next(MinDepletionAmount, MaxDepletionAmount);  // от 50 до 150 (вокруг 100)
      MinEchos = DefaultMinEchos;

      MaxEchos = _random.Next(MinMaxEchos, MaxMaxEchos);
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
