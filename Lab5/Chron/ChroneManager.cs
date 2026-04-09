using System.Collections.Generic;

// механизм Наблюдателя. Класс статический.
public static class ChroneManager {
  private static List<IChronListener> _listenerList = new List<IChronListener>();

  public static void AddListener(IChronListener listener) {
    _listenerList.Add(listener);
  }

  public static void RemoveListener(IChronListener listener) {
    _listenerList.Remove(listener);
  }

  public static void MakeChroneTick() {
    foreach (var listener in _listenerList) {
      listener.OnChronTick();
    }
  }
}
