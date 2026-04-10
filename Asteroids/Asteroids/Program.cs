using Asteroids;

class Program {
  static void Main() {
    var emitter = new AsteroidEmitter(5);
    var activeAsteroids = new List<Asteroid>();

    int turn = 0;
    Random random = new Random();

    for (int i = 0; i < 3; ++i) {
      var a = emitter.Spawn();
      activeAsteroids.Add(a);
      ChroneManager.AddListener(a);
    }

    while (true) {
      Console.WriteLine("Нажми Enter для следующего хода...");
      Console.ReadLine();

      turn++;

      ChroneManager.MakeChroneTick();

      if (turn % 5 == 0) {
        int count = random.Next(1, 4);

        for (int i = 0; i < count; ++i) {
          var a = emitter.Spawn();
          activeAsteroids.Add(a);
          ChroneManager.AddListener(a);
        }
      }

      for (int i = activeAsteroids.Count - 1; i >= 0; ++i) {
        var a = activeAsteroids[i];

        if (a.State == AsteroidState.Depleted) {
          ChroneManager.RemoveListener(a);
          emitter.Recycle(a);
          activeAsteroids.RemoveAt(i);
        }
      }

      Console.Clear();
      Console.WriteLine($"Ход: {turn}");
      Console.WriteLine("Активные астероиды:\n");

      foreach (var a in activeAsteroids) {
        Console.WriteLine(a);
      }
    }
  }
}
