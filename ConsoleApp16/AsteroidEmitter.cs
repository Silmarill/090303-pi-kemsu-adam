using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp16 {
  public class AsteroidEmitter {
    private Queue<Asteroid> _avaible = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize) {
      for (int i = 0; i < initialSize; ++i) {
        Asteroid asteroid = new Asteroid();
        _available.Enqueue(asteroid);
      }
    }

    public Asteroid Spawn() {
      if (_avaible.Count == 0) {
        return new Asteroid();
      }
      return _avaible.Dequeue();
    }
    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }
  }
}