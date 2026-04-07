using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAdam {
  public class AsteroidEmitter {
    private Queue<Asteroid> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize) {
      for (int i = 0; i < initialSize; ++i) {
        Asteroid asteroid = new Asteroid();
        _available.Enqueue(asteroid);
      }
    }

    public Asteroid Spawn() {
      Asteroid asteroid;

      if (_available.Count == 0) {
        asteroid = new Asteroid();
      } else {
        asteroid = _available.Dequeue();
      }

      asteroid.OnSpawn();
      return asteroid;
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }
  }
}
