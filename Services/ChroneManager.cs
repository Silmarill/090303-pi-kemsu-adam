using System.Collections.Generic;

public static class ChroneManager {
  // Список объектов, ожидающих уведомления о такте времени
  private static List<IChroneListener> _listenerList = new List<IChroneListener>();

  // Методы для добавления и удаления объектов из списка подписчиков
  public static void AddListener(IChroneListener listener) {
    _listenerList.Add(listener);
  }

  // Метод для удаления объекта из списка подписчиков
  public static void RemoveListener(IChroneListener listener) {
    _listenerList.Remove(listener);
  }

  public static void MakeChroneTick() {
    // Оповещение всех подписанных объектов
    foreach (var currentListener in _listenerList) {
      currentListener.OnChroneTick();
    }
  }
}