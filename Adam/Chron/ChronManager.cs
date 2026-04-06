using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace AsteroidSimulation.Chron
{
  public static class ChronManager
  {
    private static List<IChronListener> listeners = new List<IChronListener>();

    public static void AddListener(IChronListener listener)
    {
      if (!listeners.Contains(listener))
      {
        listeners.Add(listener);
      }
    }

    public static void RemoveListener(IChronListener listener)
    {
      listeners.Remove(listener);
    }

    public static void MakeChronTick()
    {
      foreach (IChronListener listener in listeners)
      {
        listener.OnChronTick();
      }
    }
  }
}
