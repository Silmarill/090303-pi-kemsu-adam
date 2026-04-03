namespace Project_Adam {
  public class AsteroidEmitter {
    private Queue<Asteroid> _available = new Queue<Asteroid>();

    public AsteroidEmitter(int initialSize) {
      for (int i = 0; i < initialSize; ++i) {
        AsteroidEmitter asteroid = new Asteroid();
        _available.Enqueue(asteroid);
      }

    public Asteroid spawn() {
      if (_available.Count == 0) {
        return new Asteroid();
      }
      return _available.Dequeue();
    }

    public void Recycle(Asteroid asteroid) {
      asteroid.Reset();
      _available.Enqueue(asteroid);
    }
 }

