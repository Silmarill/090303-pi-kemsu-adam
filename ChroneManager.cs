using System;

public interface IChroneListener {
  void OnChroneTick();
}

public static class ChroneManager {
  private List<IChroneListener> _listenerList = new List<IChroneListener>();

  public void AddListener(IChroneListener listener) {
    _listenerList.Add(listener);
  }

  public void RemoveListerner(IChroneListener listener) {
    _listenerList.Remove(listener);
  }

  public void MakeChrineTick() {
    foreach (var listener in _listenerList) {
      listener.OnCroneTick();
    }
  }
}