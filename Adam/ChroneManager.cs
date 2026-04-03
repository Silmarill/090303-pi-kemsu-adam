using System.Collections.Generic;

namespace Core {
  public interface IChroneListener {
    void OnChroneTick();
  }

  public static class ChroneManager {
    public static List<IChroneListener> listenerList = new List<IChroneListener>();

    public static void AddListener(IChroneListener listener)
    {
      if (!listenerList.Contains(listener))
      {
        listenerList.Add(listener);
      }
    }

    public static void RemoveListener(IChroneListener listener)
    {
      listenerList.Remove(listener);
    }

    public static void MakeChroneTick()
    {
      foreach (var listener in listenerList)
      {
        listener.OnChroneTick();
      }
    }
  }
}