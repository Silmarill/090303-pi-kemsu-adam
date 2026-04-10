using Asteroids;
using System.Collections.Generic;

public static class ChronoManager {
  private static List<IChronListener> _listenerList = new List<IChronListener>();

  public static int CurrentTick { get; private set; } = 0;

  public static void AddListener(IChronListener listener) {
    if (!_listenerList.Contains(listener)) {
      _listenerList.Add(listener);
    }
  }

  public static void RemoveListener(IChronListener listener) {
    _listenerList.Remove(listener);
  }

  public static void MakeChronTick() {
    CurrentTick++;

    var listenersCopy = new List<IChronListener>(_listenerList);
    foreach (var listener in listenersCopy) {
      listener.OnChronTick();
    }
  }
} 