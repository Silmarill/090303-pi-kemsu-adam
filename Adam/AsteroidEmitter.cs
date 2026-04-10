using System;
using System.Collections.Generic;

namespace Asteroids {
  public class AsteroidEmitter {
    public Queue<Asteroid> available;
    public int totalCreated;

    public AsteroidEmitter(int initialSize)
    {
      int asteroidIndex;
      Asteroid newAsteroid;

      available = new Queue<Asteroid>();
      totalCreated = 0;

      for (asteroidIndex = 0; asteroidIndex < initialSize; ++asteroidIndex)
      {
        newAsteroid = new Asteroid();
        available.Enqueue(newAsteroid);
        totalCreated = totalCreated + 1;
      }
      Console.WriteLine($"[Пул] Создан пул с {initialSize} астероидами");
    }

    public Asteroid Spawn()
    {
      Asteroid asteroid;

      if (available.Count == 0)
      {
        asteroid = new Asteroid();
        totalCreated = totalCreated + 1;
        Console.WriteLine($"[Пул] Предупреждение: пул пуст! Создан новый астероид. Всего создано: {totalCreated}");
      }
      else
      {
        asteroid = available.Dequeue();
        Console.WriteLine($"[Пул] Взят астероид из пула. Осталось свободных: {available.Count}");
      }

      asteroid.SetSpawnId();
      return asteroid;
    }

    public void Recycle(Asteroid asteroid)
    {
      asteroid.Reset();
      available.Enqueue(asteroid);
      Console.WriteLine($"[Пул] Астероид возвращён в пул. Свободных: {available.Count}");
    }

    public int GetAvailableCount()
    {
      return available.Count;
    }
  }
}