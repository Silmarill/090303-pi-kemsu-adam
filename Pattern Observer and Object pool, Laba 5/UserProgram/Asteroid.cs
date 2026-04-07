using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Pattern_Observer_and_Object_pool__Laba_5.PoolObject;

namespace Pattern_Observer_and_Object_pool__Laba_5 {

  public class MainProgram {
    
    static void Main() {
      AsteroidEmitter emitter = new AsteroidEmitter(5);
      List<Asteroid> activeAsteroid = new List<Asteroid>();
      Asteroid asteroid = new Asteroid();

      Asteroid firstAsteroid = emitter.Spawn();
      Asteroid secondAsteroid = emitter.Spawn();
      Asteroid thirdAsteroid = emitter.Spawn();

      while (true) {
        Console.WriteLine($"Нажми Enter для продолжения или Esc для выхода");

        ConsoleKeyInfo key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Escape) {
          break;
        }
      }
    }
  }
}
