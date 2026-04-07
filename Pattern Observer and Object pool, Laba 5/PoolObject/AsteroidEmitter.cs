using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Observer_and_Object_pool__Laba_5.PoolObject {
  public class AsteroidEmitter {
    private Queue<Asteroid> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize) {
      for (int i = 0; i < initialSize; ++i) {
        Asteroid asteroid = new Asteroid();
        _available.Enqueue(asteroid);
      }
    }

    public Asteroid Spawn() {
      if (_available.Count == 0) {
        return new Asteroid();
      }
      return _available.Dequeue();
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }
  }
}
