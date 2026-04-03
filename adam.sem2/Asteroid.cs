using System;

namespace AsteroidSimulator.Models {
  public class Asteroid : Interfaces.IChronListener {
    public static int s_globalCreateCounter = 0;
    public static int s_globalSpawnCounter = 0;

    public int _currentEchos;
    public int _maxEchos;
    public AsteroidState _state;
    public int _createId;
    public int _spawnId;

    public int CurrentEchos => _currentEchos;
    public int MaxEchos => _maxEchos;
    public AsteroidState State => _state;
    public int CreateId => _createId;
    public int SpawnId => _spawnId;

    public Asteroid()
    {
      Random random = new Random();
      int minEchos;
      int maxEchos;
      int echosLoss;

      minEchos = 100;
      maxEchos = 1000;
      echosLoss = 100;

      _maxEchos = random.Next(minEchos, maxEchos + 1);
      _currentEchos = _maxEchos;
      _state = AsteroidState.Idle;
      _createId = ++s_globalCreateCounter;
      _spawnId = 0;
    }

    public void Reset()
    {
      Random random = new Random();
      int minEchos;
      int maxEchos;
      int echosLoss;

      minEchos = 100;
      maxEchos = 1000;
      echosLoss = 100;

      _maxEchos = random.Next(minEchos, maxEchos + 1);
      _currentEchos = _maxEchos;
      _state = AsteroidState.Idle;
      _spawnId = ++s_globalSpawnCounter;
    }

    public void OnChronTick()
    {
      int echosLoss;

      echosLoss = 100;

      if (_state == AsteroidState.Idle)
      {
        _currentEchos -= echosLoss;

        if (_currentEchos <= 0)
        {
          _currentEchos = 0;
          _state = AsteroidState.Depleted;
        }
      }
    }

    public override string ToString()
    {
      return $"Asteroid #{_spawnId} (Created: #{_createId}) | Echos: {_currentEchos}/{_maxEchos} | State: {_state}";
    }
  }
}