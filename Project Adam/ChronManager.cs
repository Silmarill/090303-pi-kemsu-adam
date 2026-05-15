using System.Collections.Generic;
using AsteroidSimulator.Interfaces;

namespace AsteroidSimulator.Managers {
  public static class ChronManager {
    private static readonly List<IChronListener> Listeners = new List<IChronListener>();

    public static void AddListener(IChronListener listener) {
      if (!Listeners.Contains(listener)) {
        Listeners.Add(listener);
      }
    }

    public static void RemoveListener(IChronListener listener) {
      Listeners.Remove(listener);
    }

    public static void MakeChronTick() {
      IChronListener[] snapshot;
      snapshot = Listeners.ToArray();

      for (int index = 0; index < snapshot.Length; ++index) {
        snapshot[index].OnChronTick();
      }
    }

    public static void ClearListeners() {
      Listeners.Clear();
    }
  }
}
