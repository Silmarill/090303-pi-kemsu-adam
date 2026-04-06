using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp16 {
	class Program {
		static void Main(string[] args) {
      AsteroidEmitter emitter = new AsteroidEmitter(5);

      // Спавним астероид
      Asteroid asteroid = emitter.Spawn();
      Console.WriteLine($"Создан астероид ID={asteroid.CreateID}, макс. ресурс={asteroid.MaxEchos}");

      // Имитируем использование астероида (уменьшаем ресурс)
      asteroid.OnChronTick();
      asteroid.OnChronTick();
      Console.WriteLine($"Ресурс после использования: {asteroid.CurrentEchos}, состояние: {asteroid.State}");

      // Возвращаем астероид в пул (переиспользуем)
      emitter.Recycle(asteroid);
      Console.WriteLine("Астероид возвращён в пул.");

      // Спавним следующий астероид (может быть переиспользованный)
      Asteroid nextAsteroid = emitter.Spawn();
      Console.WriteLine($"Следующий астероид ID={nextAsteroid.CreateID}, спавн ID={nextAsteroid.SpawnID}");

      Console.ReadLine();
    }
	}
}
