using System.Collections.Generic;

public static class ChroneManager
{
    public static List<IChronListener> Listeners = new List<IChronListener>();

    public static void AddListener(IChronListener listener)
    {
        if (!Listeners.Contains(listener)) Listeners.Add(listener);
    }

    public static void RemoveListener(IChronListener listener)
    {
        Listeners.Remove(listener);
    }

    public static void MakeChroneTick()
    {
        var snapshot = new List<IChronListener>(Listeners);
        foreach (var listener in snapshot)
        {
            listener.OnChroneTick();
        }
    }
}