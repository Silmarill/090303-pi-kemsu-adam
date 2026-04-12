using System.Collections.Generic;

namespace Project_Adam {
  public interface IChroneListener {
    void OnChroneTick();
  }

  public static class ChroneManager {
    private static List<IChroneListener> _listenerList = new List<IChroneListener>();

    public static void AddListener(IChroneListener listener) {
      _listenerList.Add(listener);
    }

    public static void RemoveListener(IChroneListener listener) {
      _listenerList.Remove(listener);
    }

    public static void MakeChroneTick() {
      foreach (IChroneListener listener in _listenerList) {
        listener.OnChroneTick();
      }
    }
  }
}
