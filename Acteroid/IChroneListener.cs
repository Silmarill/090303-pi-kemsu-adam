using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Asteroids {
	public interface IChroneListener {
		void OnChronTick();
	}

	public static class ChroneManager {
		private static List<IChroneListener> _listenerList = new List<IChroneListener>();

		public static void AddListener(IChroneListener listener) {
			_listenerList.Add(listener);
		}

		public static void RemoveListener(IChroneListener listener) {
			_listenerList.Remove(listener);
		}

		public static void MakeChroneTick() {
			foreach (var listener in _listenerList) {
				listener.OnChronTick();
			}
		}

	}
}