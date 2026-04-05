using AsteroidSimulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation.Managers {
  public static class ChroneManager {
    private static List<IChroneListener> _listenerList = new List<IChroneListener>();

    public static void AddListener(IChroneListener listener) {
      if (!_listenerList.Contains(listener)) {
        _listenerList.Add(listener);
      }
    }

    public static void RemoveListener(IChroneListener listener) {
      _listenerList.Remove(listener);
    }

    public static void MakeChroneTick() {
      for (int listenerIndex = 0; listenerIndex < _listenerList.Count; ++listenerIndex) {
        _listenerList[listenerIndex].OnChroneTick();
      }
    }
  }
}
