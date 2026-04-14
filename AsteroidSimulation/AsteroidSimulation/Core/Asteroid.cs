using System;

namespace AsteroidSimulation {

  public class Asteroid : IChroneListener {
    private static int _globalCreateCounter = 0;

    private int _currentEchos;
    private int _maxEchos;
    private AsteroidState _state;
    private int _spawnId;
    private int _createId;

    public int CurrentEchos { get; set; }
    public int MaxEchos { get { return _maxEchos; } }
    public AsteroidState State { get; set; }
    public int SpawnId { get { return _spawnId; } }
    public int CreateId { get { return _createId; } }

    public Asteroid() {
      Random random = new Random();
      _maxEchos = random.Next(100, 1001);
      _currentEchos = _maxEchos;
      _state = AsteroidState.Idle;
      _createId = ++_globalCreateCounter;
      _spawnId = 0;
    }

    public void Reset() {
      _currentEchos = _maxEchos;
      _state = AsteroidState.Idle;
    }

    public void OnChronTick() {
      // Деградация отключена, так как есть станция Матриарх
      // Астероиды не теряют ресурс сами по себе
    }

    public void SetSpawnId(int spawnId) {
      _spawnId = spawnId;
    }

    public override string ToString() {
      return $"ID(Создания:{_createId}, Появления:{_spawnId}) | Echos: {_currentEchos}/{_maxEchos} | Состояние: {_state}";
    }
  }
}