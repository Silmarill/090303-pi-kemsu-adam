using AsteroidSimulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation.Managers {
  public class AsteroidEmitter {
    public Queue<Asteroid> _available = new Queue<Asteroid>();
    private int _nextSpawnId = 1;

    public AsteroidEmitter(int initialSize) {
      for (int asteroidCount = 0; asteroidCount < initialSize; ++asteroidCount) {
        Asteroid asteroid = new Asteroid();
        _available.Enqueue(asteroid);
      }
      Console.WriteLine($"[Пул] Создан пул с {initialSize} астероидами");
    }

    public Asteroid Spawn() {
      Asteroid asteroid;

      if (_available.Count == 0) {
        asteroid = new Asteroid();
        Console.WriteLine($"[Пул] ВНИМАНИЕ: Пул пуст! Создан новый астероид (CreateID: {asteroid.CreateID})");
      } else {
        asteroid = _available.Dequeue();
        Console.WriteLine($"[Пул] Астероид взят из пула (CreateID: {asteroid.CreateID})");
      }

      asteroid.SetSpawnID(++_nextSpawnId);
      return asteroid;
    }

    public void Recycle(Asteroid asteroid) {
      if (asteroid.State == AsteroidState.Depleted) {
        asteroid.Reset();
        _available.Enqueue(asteroid);
        Console.WriteLine($"[Пул] Астероид возвращён в пул (CreateID: {asteroid.CreateID}, SpawnID: {asteroid.SpawnID})");
      }
    }

    public int AvailableCount => _available.Count;
  }
}
