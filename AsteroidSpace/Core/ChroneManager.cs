using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSpace {
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
        listener.OnChoneTrick();
      }
    }
  }
}
