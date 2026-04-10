using System.Collections.Generic;
using AsteroidSimulator.Interfaces;

namespace AsteroidSimulator.Managers {
  public static class ChronManager {
    public static List<IChronListener> Listeners;

    static ChronManager() {
      Listeners = new List<IChronListener>();
    }

    public static void AddListener(IChronListener listener) {
      Listeners.Add(listener);
    }

    public static void RemoveListener(IChronListener listener) {
      Listeners.Remove(listener);
    }

    public static void MakeChronTick() {
      foreach (IChronListener currentListener in Listeners) {
        currentListener.OnChronTick();
      }
    }
  }
}