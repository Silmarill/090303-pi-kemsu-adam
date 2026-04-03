using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids {
  public interface IChroneListener {
    void OnChoneTrick();
  }
  public class ChroneManager {
    private List<IChroneListener> _listenerList = new List<IChroneListener>();

    public void AddListener(IChroneListener listener) {
      _listenerList.Add(listener);
    }

    public void RemoveListener(IChroneListener listener) {
      _listenerList.Remove(listener);
    }

    public void MakeChroneTick() {
      foreach (var listener in _listenerList) {
        listener.OnChoneTrick();
      }
    }
  }

}
