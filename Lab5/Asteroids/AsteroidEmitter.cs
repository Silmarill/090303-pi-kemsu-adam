using System;
using System.Collections.Generic;

public class AsteroidEmitter {
  private Queue<Asteroid> _available = new Queue<Asteroid>();

  // создаём пул и сразу наполняем его объектами
  public AsteroidEmitter(int initialSize) {
    for (int i = 0; i < initialSize; ++i) {
      Asteroid asteroid = new Asteroid();
      _available.Enqueue(asteroid);
    }
  }

  // взять астероид из пула
  public Asteroid Spawn() {
    Asteroid asteroid;
    if (_available.Count == 0) {
      // если пуст, создаём новый (автоматическое расширение)
      Console.WriteLine("[Pool] Warning: pool is empty, creating new Asteroid.");
      asteroid = new Asteroid();
    } else {
      asteroid = _available.Dequeue();
    }
    asteroid.MarkSpawned();
    return asteroid;
  }

  // вернуть астероид обратно в пул
  public void Recycle(Asteroid asteroid) {
    // в методе Reset сбрасываем состояние астероида
    asteroid.Reset();
    _available.Enqueue(asteroid);
  }

  public int AvailableCount => _available.Count;
}
