using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ChroneManager {
  private static List<IChroneListener> _listenerList = new List<IChroneListener>();

  public static void AddListener(IChroneListener listener) {
    _listenerList.Add(listener);
  }

  public static void RemoveListener(IChroneListener listener) {
    _listenerList.Remove(listener);
  }

  public static void MakeChroneTick() {
    List<IChroneListener> copy = new List<IChroneListener>(_listenerList);
    foreach (IChroneListener listener in copy) {
      listener.OnChroneTick();
    }
  }
}