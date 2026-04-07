// "Контракт" для системы времени (хронов). Он реализует паттерн Observer (Наблюдатель)
public interface IChroneListener {
  void OnChroneTick();
}