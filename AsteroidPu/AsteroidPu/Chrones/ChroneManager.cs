using System;
using System.Collections.Generic;

namespace AsteroidPu.Chrones {
  public static class ChroneManager {
    private static List<IChroneListener> _listenerList = new List<IChroneListener>();

    public static void Addlistener(IChroneListener listener) {
      _listenerList.Add(listener);
    }

    public static void RemoveListener(IChroneListener listener) {
      _listenerList.Remove(listener);
    }

    public static void MakeCgroneTick() {
      foreach (var listener in _listenerList) {
        listener.OnChroneTick();
      }
    }
  }
}
