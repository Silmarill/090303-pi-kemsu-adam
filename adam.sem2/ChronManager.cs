using System.Collections.Generic;
using AsteroidSimulator.Interfaces;

namespace AsteroidSimulator.Managers {
  public static class ChronManager {
    public static List<IChronListener> _listenerList = new List<IChronListener>();

    public static void AddListener(IChronListener listener)
    {
      if (!_listenerList.Contains(listener))
        _listenerList.Add(listener);
    }

    public static void RemoveListener(IChronListener listener)
    {
      _listenerList.Remove(listener);
    }

    public static void MakeChronTick()
    {
      foreach (var listener in _listenerList)
      {
        listener.OnChronTick();
      }
    }

    public static int ListenerCount => _listenerList.Count;
  }
}