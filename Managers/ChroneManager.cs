using AsteroidsLab.Interfaces;

namespace AsteroidsLab.Managers;

public static class ChroneManager
{
  private static List<IChroneListener> s_listenerList;

  static ChroneManager()
  {
    s_listenerList = new List<IChroneListener>();
  }

  public static void AddListener(IChroneListener listener)
  {
    s_listenerList.Add(listener);
  }

  public static void RemoveListener(IChroneListener listener)
  {
    s_listenerList.Remove(listener);
  }

  public static void MakeChroneTick()
  {
    int listenerIndex;

    for (listenerIndex = 0; listenerIndex < s_listenerList.Count; ++listenerIndex)
    {
      s_listenerList[listenerIndex].OnChroneTick();
    }
  }
}
