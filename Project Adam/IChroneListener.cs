using System.Collections.Generic;

namespace Project_Adam {
  public interface IChroneListener {
    void OnChroneTick();
  }

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
      foreach (var listener in _listenerList.ToList()) { 
        listener?.OnChroneTick(); 
      }
    }
  }
}
