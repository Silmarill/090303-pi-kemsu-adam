using System;
using System.Collections.Generic;

namespace Asteroids {
	public static class ChronoManager {
		private static List<IChronListener> _listenerList = new List<IChronListener>();

		public static void AddListener(IChronListener listener) {
			_listenerList.Add(listener);
		}

		public static void RemoveListener(IChronListener listener) {
			_listenerList.Remove(listener);
		}

		public static void MakeChronTick() {
			foreach (IChronListener listener in _listenerList) {
				listener.OnChronTick();
			}
		}
	}
}