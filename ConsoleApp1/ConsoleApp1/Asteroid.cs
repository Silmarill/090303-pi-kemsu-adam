using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ConsoleApp1 {
  internal class Asteroid {
    Asteroid uwuAsteroid = _asteroidEmitter.Spawn();

    if (uwuAsteroid.State == Depleted) {
      _asteroidEmitter.Recycle(uwuAsteroid);
  }
}