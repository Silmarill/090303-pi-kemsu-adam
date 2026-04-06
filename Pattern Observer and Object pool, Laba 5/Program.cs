using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konovalov {

  public interface IChroneListener {
    void OnChroneTick();
  }

  public static class ChroneManager {
    private static List<IChroneListener> _listenerList = new List<IChroneListener>();

    public static void AddListener(IChroneListener listener) {
      _listenerList.Add(listener);
    }

    public static void RemoveListener(IChroneListener listener) {
      _listenerList.Remove(listener);
    }

    public static void MakeChroneTick() {
      foreach (var listener in _listenerList) {
        listener.OnChroneTick();
      }
    }
  }

  public class AsteroidEmitter {
    private Queue<AsteroidEmitter> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize) {
      for (int i = 0; i < initialSize; ++i) {
        AsteroidEmitter asteroid = new Asteroid();
        _available.Enqueue(asteroid);
      }
    }

    public AsteroidEmitter Spawn() {
      if (_available.Count == 0) {
        return new Asteroid();
      }
      return _available.Dequeue();
    }

    public void Recycle(AsteroidEmitter asteroid) {
      asteroid.Reset();
      _availavle.Enquenue(asteroid);
    }
  }

  class void Asteroid

}
