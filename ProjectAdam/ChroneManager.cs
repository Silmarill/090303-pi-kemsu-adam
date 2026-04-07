using System.Collections.Generic;

namespace ProjectAdam;

public static class ChronManager
{
  private static List<IChronListener> s_listeners;

  static ChronManager()
  {
    s_listeners = new List<IChronListener>();
  }

  public static void AddListener(IChronListener listener)
  {
    s_listeners.Add(listener);
  }

  public static void RemoveListener(IChronListener listener)
  {
    s_listeners.Remove(listener);
  }

  public static void MakeChronTick()
  {
    IChronListener listener;
    int listenerIndex;
    for (listenerIndex = 0; listenerIndex < s_listeners.Count; ++listenerIndex)
    {
      listener = s_listeners[listenerIndex];
      listener.OnChronTick();
    }
  }
}
