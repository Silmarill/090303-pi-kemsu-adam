using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Observer_and_Object_pool__Laba_5 {
  public static class ChroneManager {
    private static List<IChroneListener> _listenerList = new List<IChroneListener>();

    static public void AddListener(IChroneListener listener) {
      _listenerList.Add(listener);
    }

    static public void RemoveListener(IChroneListener listener) {
      _listenerList.Remove(listener);
    }

    static public void MakeChroneTick() {
      foreach (var listener in _listenerList) {
        listener.OnChroneTick();
      }
    }
  }
}
