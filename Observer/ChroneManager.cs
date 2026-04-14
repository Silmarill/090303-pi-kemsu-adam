using AsteroidSimulation.Common;
using System;
using System.Collections.Generic;

namespace AsteroidSimulation.Observer
{
    public static class ChroneManager
    {
        private static List<IChroneListener> _listenerList = new List<IChroneListener>();

        public static int CurrentChrone = 0;

        public static void AddListener(IChroneListener listener)
        {
            _listenerList.Add(listener);
        }

        public static void RemoveListener(IChroneListener listener)
        {
            _listenerList.Remove(listener);
        }

        public static void MakeChroneTick()
        {
            ++CurrentChrone;
            
            for (int index = _listenerList.Count - 1; index >= 0; --index)
            {
                _listenerList[index].OnChroneTick();
            }
        }
    }
}