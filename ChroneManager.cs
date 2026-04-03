using System;

namespace Asteroid
{
    public static class ChroneManager
    {
        private static List<IChroneListener> _listenerList = new List<IChroneListener>();

        public void AddListener(IChroneListener listener)
        {
            _listenerList.Add(listener);
        }

        public void RemoveListener(IChroneListener listener)
        {
            _listenerList.Remove(listener);
        }

        public void MakeChroneTick()
        {
            foreach (var listener in _listenerList)
            {
                listener.OnChroneTick();
            }
        }
    }
}