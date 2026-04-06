using System;
using System.Collections.Generic;
using AsteroidSimulation.Models;

namespace AsteroidSimulation.Pool
{
  public class AsteroidEmitter
  {
    private Queue<Asteroid> availableAsteroids = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize)
    {
      for (int asteroidIndex = 0; asteroidIndex < initialSize; ++asteroidIndex)
      {
        availableAsteroids.Enqueue(new Asteroid());
      }
    }

    public Asteroid Spawn()
    {
      if (availableAsteroids.Count == 0)
      {
        Console.WriteLine("Внимание: пул пуст, создаётся новый астероид!");
        return new Asteroid();
      }

      return availableAsteroids.Dequeue();
    }

    public void Recycle(Asteroid asteroid)
    {
      asteroid.Reset();
      availableAsteroids.Enqueue(asteroid);
    }
  }
}