using System.Collections.Generic;

namespace Asteroid
{
  public interface IChronListener {
    void OnChronTick();
  }

  public static class ChronManager {
    private static List<IChronListener> _listeners = new List<IChronListener>();
    private static int _chronCounter = 0;

    public static void AddListener(IChronListener listener) {
      if (!_listeners.Contains(listener)) {
        _listeners.Add(listener);
      }
    }

    public static void RemoveListener(IChronListener listener) {
      _listeners.Remove(listener);
    }

    public static void MakeChronTick() {
      ++_chronCounter;

      foreach (var listener in _listeners) {
        listener.OnChronTick();
      }
    }

    public static int GetCurrentChron() {
      return _chronCounter;
    }
  }
}
