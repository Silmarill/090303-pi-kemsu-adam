using System.Collections.Generic;

// Интерфейс для объектов, которые хотят получать уведомления о хроне
public static class ChroneManager {
  // Список подписчиков, которые будут уведомляться о хроне
  private static List<IChroneListener> _listenerList = new List<IChroneListener>();

  // Метод для добавления подписчика в список
  public static void AddListener(IChroneListener listener) {
    _listenerList.Add(listener);
  }

  // Метод для удаления подписчика из списка
  public static void RemoveListener(IChroneListener listener) {
    _listenerList.Remove(listener);
  }

  // Метод для уведомления всех подписчиков о хроне
  public static void MakeChroneTick() {
    foreach (var listener in _listenerList) {
      listener.OnChroneTick();
    }
  }
}