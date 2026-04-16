using Asteroids;
using System.Collections.Generic;

public static class ChronoManager {
  private static List<IChronListener> _listenerList = new List<IChronListener>();

  public static int CurrentTick { get; private set; } = 0;

  public static void AddListener(IChronListener listener) {
    if (listener != null && !_listenerList.Contains(listener)) {
      _listenerList.Add(listener);
    }
  }

  public static void RemoveListener(IChronListener listener) {
    if (listener != null) {
      _listenerList.Remove(listener);
    }
  }
  public static void MakeChronTick() {
    CurrentTick++;

    var listenersCopy = new List<IChronListener>(_listenerList);
    foreach (var listener in listenersCopy) {
      listener?.OnChronTick();
    }
  }
  public static void ClearAllListeners() {
    _listenerList.Clear();
    CurrentTick = 0;
  }
}